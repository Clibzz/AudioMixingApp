using AudioMixingApp.Models;

namespace AudioMixingApp.Views;

public partial class EffectPage : ContentPage
{
	public EffectPage(Player player)
	{
		InitializeComponent();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}