using Client.Views.ViewModels;

namespace Client.Views;

public partial class PostsPage : ContentPage
{
	public PostsPage()
	{
		InitializeComponent();

		BindingContext = new PostsViewModel(this, "");

	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		PostsViewModel postsViewModel = BindingContext as PostsViewModel;
		postsViewModel.Refresh();
    }
}