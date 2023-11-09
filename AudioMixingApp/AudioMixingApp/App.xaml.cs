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
        string filePath = "";
        var audioFile = new AudioFileReader(filePath);
        //Pas de flangerFactor naar wens aan tussen 0.0 en 0.5
        var flangerEffect = new FlangerEffect(audioFile, flangerFactor: 0.1f);
        Console.WriteLine("bleep");
        var output = new WaveOutEvent();
        output.Init(flangerEffect);
        output.Play();   
    }
}
