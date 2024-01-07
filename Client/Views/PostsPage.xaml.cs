using Client.Views.ViewModels;

namespace Client.Views;

public partial class PostsPage : ContentPage
{
	public PostsPage()
	{
		InitializeComponent();

		pickerTags.ItemsSource = new List<string>() { "prova", "test" };

		BindingContext = new PostsViewModel(this, "");

	}
}