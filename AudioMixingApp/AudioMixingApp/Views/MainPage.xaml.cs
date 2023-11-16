using AudioMixingApp.ViewModels;

namespace AudioMixingApp.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void AboutPageButton_OnClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new AboutPage());
	}
	
	private void MixingPageButton_OnClicked(object sender, EventArgs e)
	{
		var mixingPage = new MixingPage();
		mixingPage.BindingContext = new MixingPageViewModel();
		Navigation.PushAsync(mixingPage);
	}

    private void SongsPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SongsPage());
    }
}

