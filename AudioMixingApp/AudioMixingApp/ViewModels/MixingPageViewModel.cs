using System.Diagnostics;
using AudioMixingApp.Effects;
using AudioMixingApp.Models;
using NAudio.Wave;

namespace AudioMixingApp.ViewModels;

public class MixingPageViewModel
{
    public List<Song> Songs { get; set; }
    
    public float CurrentValue { get; set; }

    private readonly System.Timers.Timer _timer;
    private WaveOutEvent waveOut;

    public MixingPageViewModel()
    {
        _timer = new System.Timers.Timer();
        _timer.Interval = 1000;
        _timer.Start();
        
        waveOut = new WaveOutEvent();
    }

    public void PlaySound(string path, float volume)
    {
        if (waveOut.PlaybackState == PlaybackState.Playing)
        {
            waveOut.Stop();
            return;
        }
        
        AudioFileReader audioFile = new AudioFileReader(path);
        
        waveOut.Init(audioFile);
        waveOut.Play();
        waveOut.Volume = volume;
        
        _timer.Elapsed += (sender, eventArgs) =>
        {
            CurrentValue = audioFile.CurrentTime.Seconds;
            Trace.WriteLine(waveOut.PlaybackState);
        };
    }
}