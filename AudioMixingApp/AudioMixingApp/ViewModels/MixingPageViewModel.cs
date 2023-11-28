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
    private int _currentTime;
    private int _totalTime;
    private readonly System.Timers.Timer _timer;
    private string _currentTimeString = "00:00:00";
    private string _totalTimeString = "00:00:00";

    public readonly Player _player;

    public MixingPageViewModel()
    {
        _timer = new System.Timers.Timer
        {
            Interval = 100
        };
        _timer.Start();

        _player = new Player();
    }

    /// <summary>
    /// Add an audio file to the queue
    /// </summary>
    /// <param name="filename">filename including file extension</param>
    public void AddSong(string filename)
    {
        _player.AddToQueue(filename);
    }

    /// <summary>
    /// Play the song and update the slider values and labels
    /// </summary>
    public void PlaySound()
    {
        //Play or pause if a song is already playing
        if (_player.PlayingSong != null)
        {
            _player.TogglePlayback();
        }
        else
        {
            //Get a song from the queue
            _player.PlaySongFromQueue();

            //If it couldn't get a song from the queue, it means the queue is empty, so stop.
            if (_player.PlayingSong == null) return;

            TotalTime = (int)_player.PlayingSong.TotalTime.TotalSeconds;
            TotalTimeString = _player.PlayingSong.TotalTime.ToString(@"hh\:mm\:ss");

            _timer.Elapsed += (sender, eventArgs) =>
            {
                if (PauseSliderUpdates) return;

                CurrentTime = (int)_player.PlayingSong.CurrentTime.TotalSeconds;
                CurrentTimeString = _player.PlayingSong.CurrentTime.ToString(@"hh\:mm\:ss");
            };
        }
    }

    /// <summary>
    /// Update the current time of the slider and start playing the song if its stopped
    /// </summary>
    /// <param name="time"></param>
    public void UpdateCurrentTime(double time)
    {
        _player.PlayingSong.CurrentTime = TimeSpan.FromSeconds(time);

        if (_player.Output.PlaybackState == PlaybackState.Stopped && _player.PlayingSong.CurrentTime != _player.PlayingSong.TotalTime)
        {
            _player.Output.Play();
        }
    }

    public void ChangeVolume(float volume)
    {
        _player.Output.Volume = volume;
    }

    public void SkipSong()
    {
        _player.SkipSong();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool PauseSliderUpdates { get; set; }

    public int CurrentTime
    {
        get => _currentTime;
        set
        {
            if (_currentTime != value)
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
    }

    public int TotalTime
    {
        get => _totalTime;
        set
        {
            if (_totalTime != value)
            {
                _totalTime = value;
                OnPropertyChanged(nameof(TotalTime));
            }
        }
    }

    public string CurrentTimeString
    {
        get => _currentTimeString;
        set
        {
            if (_currentTimeString != value)
            {
                _currentTimeString = value;
                OnPropertyChanged(nameof(CurrentTimeString));
            }
        }
    }

    public string TotalTimeString
    {
        get => _totalTimeString;
        set
        {
            if (_totalTimeString != value)
            {
                _totalTimeString = value;
                OnPropertyChanged(nameof(TotalTimeString));
            }
        }
    }
}