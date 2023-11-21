namespace AudioMixingApp.Views;
using AudioMixingApp.ViewModels;

public partial class SongsPage : ContentPage
{
	public SongsPage()
	{
		InitializeComponent();
		BindingContext = new SongsViewModel();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}