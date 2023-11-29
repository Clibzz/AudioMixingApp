using AudioMixingApp.Views;

namespace AudioMixingApp;

public partial class EffectPage : ContentPage
{
	public EffectPage()
	{
		InitializeComponent();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}