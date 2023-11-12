namespace AudioMixingApp.Views;
using AudioMixingApp.ViewModels;

public partial class SongsPage : ContentPage
{
	public SongsPage()
	{
		InitializeComponent();
		BindingContext = new SongsViewModel();
	}
}