using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AudioMixingApp.Effects;
using AudioMixingApp.Models;
using NAudio.Wave;

namespace AudioMixingApp.ViewModels;

public class MixingPageViewModel
{
    public List<Song> Songs { get; set; }

    // public int CurrentValue { get; set; }
    public int SongDuration { get; set; }

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

        SongDuration = (int)audioFile.TotalTime.TotalSeconds;

        _timer.Elapsed += (sender, eventArgs) =>
        {
            CurrentValue = (int)audioFile.CurrentTime.TotalSeconds;
            Trace.WriteLine(CurrentValue);
            Trace.WriteLine(SongDuration);
        };
    }
    
    
    
    
    
    
    private int _CurrentValue = 10;
    public int CurrentValue
    {
        get => _CurrentValue;
        set
        {
            if (_CurrentValue != value)
            {
                _CurrentValue = value;
                OnPropertyChanged("CurrentValue"); 
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}