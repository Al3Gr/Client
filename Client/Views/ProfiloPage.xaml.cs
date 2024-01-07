namespace Client.Views;
using Client.Views.ViewModels;

public partial class ProfiloPage : ContentPage
{
	public ProfiloPage()
	{
		InitializeComponent();
		BindingContext = new ProfiloViewModel();
	}
}