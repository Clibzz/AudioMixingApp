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
            "C:/Users/Yanni/Downloads/@Pantera-Cowboys-from-Hell.mp3";

        AudioFileReader audioFile = new AudioFileReader(inputFilePath);

        audioFile.CurrentTime = TimeSpan.FromSeconds(100);

        Equalizer equalizer = new Equalizer(audioFile);
        equalizer.SetLows(30);

        ReverbEffect reverbEffect = new ReverbEffect(equalizer, 0.2f);

        WaveOutEvent waveOut = new WaveOutEvent();
        waveOut.Init(reverbEffect);
        waveOut.Play();
        waveOut.Volume = 0.1f;

        MainPage = new NavigationPage(new MainPage());
    }
}