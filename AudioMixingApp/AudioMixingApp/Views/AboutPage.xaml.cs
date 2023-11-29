namespace AudioMixingApp.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void MixingPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MixingPage());
    }

    private void PlaylistPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PlaylistPage());
    }
}