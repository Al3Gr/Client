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
        private string pickerSelection;

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

        public string PickerSelection
        {
            get => pickerSelection;
            set
            {
                pickerSelection = value;
                NotifyPropertyChanged(nameof(PickerSelection));
            }
        }

        public ICommand Search { get; set; }
        public ICommand SearchMore { get; set; }

        public PostsViewModel(Page pageBinded, string queryTag)
        {
            this.pageBinded = pageBinded;
            this.queryTag = queryTag;
            PickerSelection = queryTag;
            pageNumber = 0;

            Search = new Command(SearchClicked);
            SearchMore = new Command(SearchMoreHandler);

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
            if (PickerSelection != queryTag)
                pageBinded.BindingContext = new PostsViewModel(pageBinded, PickerSelection);
        }

        private void SearchMoreHandler()
        {
            DownloadPost((++pageNumber) * 10);
        }

    }
}
