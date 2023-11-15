using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AudioMixingApp.Effects;
using AudioMixingApp.Models;
using NAudio.Wave;

namespace AudioMixingApp.ViewModels;

public class MixingPageViewModel : INotifyPropertyChanged
{
    private int _currentValue;
    private int _songDuration;
    private readonly System.Timers.Timer _timer;
    private readonly WaveOutEvent _waveOut;
    private AudioFileReader _currentAudio;

    public MixingPageViewModel()
    {
        _timer = new System.Timers.Timer();
        _timer.Interval = 500;
        _timer.Start();

        _waveOut = new WaveOutEvent();
    }


    public void PlaySound(string path, float volume)
    {
        if (_waveOut.PlaybackState == PlaybackState.Playing)
        {
            _waveOut.Pause();
            return;
        }

        if (_waveOut.PlaybackState == PlaybackState.Paused)
        {
            _waveOut.Play();
            return;
        }

        _currentAudio = new AudioFileReader(path);

        _waveOut.Init(_currentAudio);
        _waveOut.Play();
        _waveOut.Volume = volume;

        SongDuration = (int)_currentAudio.TotalTime.TotalSeconds;

        _timer.Elapsed += (sender, eventArgs) =>
        {
            if (!PauseSliderUpdates)
            {
                CurrentValue = (int)_currentAudio.CurrentTime.TotalSeconds;
            }
        };
    }

    public void SetTime(double time)
    {
        _currentAudio.CurrentTime = TimeSpan.FromSeconds(time);

        if (_waveOut.PlaybackState == PlaybackState.Stopped && _currentAudio.CurrentTime != _currentAudio.TotalTime)
        {
            _waveOut.Play();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool PauseSliderUpdates { get; set; }

    public int CurrentValue
    {
        get => _currentValue;
        set
        {
            _currentValue = value;
            OnPropertyChanged(nameof(CurrentValue));
        }
    }

    public int SongDuration
    {
        get => _songDuration;
        set
        {
            _songDuration = value;
            OnPropertyChanged(nameof(SongDuration));
        }
    }
}