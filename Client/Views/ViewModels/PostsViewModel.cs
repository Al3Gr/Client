using Client.Exceptions;
using Client.Models;
using Client.Services;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Views.ViewModels
{
    public class PostsViewModel : INotifyPropertyChanged
    {

        private Page pageBinded;
        private string queryTag;
        private int pageNumber;

        private ObservableCollection<PhotoInfoModel> posts;
        private bool isLoading;
        private string searchQuery;

        public ObservableCollection<PhotoInfoModel> Posts
        {
            get => posts;
            set
            {
                posts = value;
                NotifyPropertyChanged(nameof(Posts));
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                NotifyPropertyChanged(nameof(IsLoading));
                NotifyPropertyChanged(nameof(IsNotLoading));
            }
        }

        public bool IsNotLoading
        {
            get => !isLoading;
        }

        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                NotifyPropertyChanged(nameof(SearchQuery));
            }
        }

        public ICommand Search { get; set; }
        public ICommand SearchMore { get; set; }
        public ICommand LikePost { get; set; }

        public PostsViewModel(Page pageBinded, string queryTag)
        {
            this.pageBinded = pageBinded;
            this.queryTag = queryTag;
            SearchQuery = queryTag;
            pageNumber = 0;

            Search = new Command(SearchClicked);
            SearchMore = new Command(SearchMoreHandler);
            LikePost = new Command(LikePostHandler);

            DownloadPost(0);
        }

        private async void DownloadPost(int skip)
        {
            IsLoading = true;

            try
            {
                var lista = await RestService.Instance.GetPosts(queryTag, skip);
                if (skip == 0)
                    Posts = new ObservableCollection<PhotoInfoModel>(lista);
                else
                    foreach (var post in lista)
                        Posts.Add(post);
            }
            catch (RestServiceException)
            {
                await App.Current.MainPage.DisplayAlert("Attenzione!", "Qualcosa è andato storto!", "Ok");
            }

            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchClicked()
        {
            if (SearchQuery != queryTag)
                pageBinded.BindingContext = new PostsViewModel(pageBinded, SearchQuery);
        }

        private void SearchMoreHandler()
        {
            DownloadPost((++pageNumber) * 10);
        }

        private async void LikePostHandler(object sender)
        {
            PhotoInfoModel post = sender as PhotoInfoModel;

            if (post == null)
                return;

            bool like = !post.HasMyLike;
            if (await RestService.Instance.LikePost(post._id.Id, like))
            {
                if (like)
                    post.likes.Add(UserService.Instance.Username);
                else
                    post.likes.Remove(UserService.Instance.Username);

                //notifico alla GUI
                post.NotifyHasMyLikeChanged();
            }
            else
                await App.Current.MainPage.DisplayAlert("Attenzione", "Qualcosa è andata storto!", "Ok");
        }

        public void Refresh()
        {
            pageBinded.BindingContext = new PostsViewModel(pageBinded, queryTag);
        }

    }
}
