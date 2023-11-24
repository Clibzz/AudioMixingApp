namespace AudioMixingApp;
using AudioMixingApp.Effects;
using AudioMixingApp.Views;
using NAudio.Wave;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MainPage());
    }
}
