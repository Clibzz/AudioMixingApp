using AudioMixingApp.Effects;
using NAudio.Wave;

namespace AudioMixingApp;

using Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new PlayerPage());
    }
}
