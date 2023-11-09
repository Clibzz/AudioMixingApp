namespace AudioMixingApp;
using AudioMixingApp.Effects;

using NAudio.Wave;
using Viewmodels;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new NavigationPage(new MainPage());
        string filePath = "";
        var audioFile = new AudioFileReader(filePath);
        var flangerEffect = new FlangerEffect(audioFile, flangerFactor: 0.4f); // (tussen 0.0 en 0.5)

        var output = new WaveOutEvent();
        output.Init(flangerEffect);
        output.Play();
    }
}
