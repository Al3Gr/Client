using Client.Views.ViewModels;

namespace Client.Views;

public partial class PostsPage : ContentPage
{
	public PostsPage()
	{
		InitializeComponent();

		BindingContext = new PostsViewModel(this, "");

	}
}