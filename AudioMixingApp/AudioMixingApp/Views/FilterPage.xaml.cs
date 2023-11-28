using AudioMixingApp.Views;

namespace AudioMixingApp;

public partial class FilterPage : ContentPage
{
	public FilterPage(Player player)
	{
		InitializeComponent();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}