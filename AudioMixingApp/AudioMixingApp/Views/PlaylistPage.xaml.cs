namespace AudioMixingApp.Views;
using AudioMixingApp.ViewModels;

public partial class PlaylistPage : ContentPage
{
	public PlaylistPage()
	{
		InitializeComponent();
		BindingContext = new PlaylistsViewModel();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}