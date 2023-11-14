using AudioMixingApp.Effects;
using NAudio.Wave;

namespace AudioMixingApp;
using Views;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		
		string inputFilePath =
			"C:/Users/Yanni/Downloads/@Pantera-Cowboys-from-Hell.mp3"; // Replace with your input audio file path

		ISampleProvider audioFile = new AudioFileReader(inputFilePath);
        
		Equalizer equalizer = new Equalizer(audioFile);
  
		WaveOutEvent waveOut = new WaveOutEvent();
		waveOut.Init(equalizer);
		waveOut.Play();
		waveOut.Volume = 0.5f;

		MainPage = new NavigationPage(new MainPage());
	}
}
