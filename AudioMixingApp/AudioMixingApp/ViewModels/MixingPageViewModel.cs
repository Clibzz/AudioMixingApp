using System.ComponentModel;
using AudioMixingApp.Models;
using NAudio.Wave;

namespace AudioMixingApp.ViewModels;

public class MixingPageViewModel : INotifyPropertyChanged
{
    private int _currentTimeA, _totalTimeA, _currentTimeB, _totalTimeB;

    private string _currentTimeStringA = "00:00",
        _totalTimeStringA = "00:00",
        _currentTimeStringB = "00:00",
        _totalTimeStringB = "00:00";

    private readonly System.Timers.Timer _timer;
    private readonly Player _playerA, _playerB;

    public MixingPageViewModel()
    {
        _timer = new System.Timers.Timer
        {
            Interval = 100
        };
        _timer.Start();

        _playerA = new Player();
        _playerB = new Player();
    }

    /// <summary>
    /// Get the player instance from the character representing it
    /// </summary>
    /// <param name="playerChar">'A' or 'B'</param>
    /// <returns>PlayerA or PlayerB respectively</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Player GetPlayer(char playerChar)
    {
        if (playerChar != 'A' && playerChar != 'B')
            throw new ArgumentOutOfRangeException();

        return playerChar == 'A' ? _playerA : _playerB;
    }
    
    //Property changed event to update the frontend
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    //////////////////////////////////////
    ////// PLAYBACK FUNCTIONALITIES //////
    //////////////////////////////////////

    /// <summary>
    /// Play the song and update the slider values and labels
    /// </summary>
    public void PlaySound(char playerChar)
    {
        Player player = GetPlayer(playerChar);
        //Play or pause if a song is already playing
        if (player.PlayingSong != null)
        {
            player.TogglePlayback();
        }
        else
        {
            //Get a song from the queue
            player.PlaySongFromQueue();

            //If it couldn't get a song from the queue, it means the queue is empty, so stop.
            if (player.PlayingSong == null) return;

            int totalTime = (int)player.PlayingSong.TotalTime.TotalSeconds;
            string totalTimeString = player.PlayingSong.TotalTime.ToString(@"hh\:mm\:ss");

            if (playerChar == 'A')
            {
                TotalTimeA = totalTime;
                TotalTimeStringA = totalTimeString;
            }
            else
            {
                TotalTimeB = totalTime;
                TotalTimeStringB = totalTimeString;
            }

            _timer.Elapsed += (sender, eventArgs) =>
            {
                if (PauseSliderUpdatesA) return;

                int currentTime = (int)player.PlayingSong.CurrentTime.TotalSeconds;
                string currentTimeString = player.PlayingSong.CurrentTime.ToString(@"hh\:mm\:ss");

                if (playerChar == 'A')
                {
                    CurrentTimeA = currentTime;
                    CurrentTimeStringA = currentTimeString;
                }
                else
                {
                    CurrentTimeB = currentTime;
                    CurrentTimeStringB = currentTimeString;
                }
            };
        }
    }

    /// <summary>
    /// Update the current time of the slider and start playing the song if its stopped
    /// </summary>
    /// <param name="playerChar">'A' or 'B' for which player to execute the function to</param>
    /// <param name="time"></param>
    public void UpdateCurrentTime(char playerChar, double time)
    {
        Player player = GetPlayer(playerChar);

        player.PlayingSong.CurrentTime = TimeSpan.FromSeconds(time);

        if (player.Output.PlaybackState == PlaybackState.Stopped &&
            player.PlayingSong.CurrentTime != player.PlayingSong.TotalTime)
        {
            player.Output.Play();
        }
    }

    public void ChangeVolume(char player, float volume)
    {
        GetPlayer(player).Output.Volume = volume;
    }

    public void SkipSong(char player)
    {
        GetPlayer(player).SkipSong();
    }

    //////////////////////
    ////// PLAYER A //////
    //////////////////////

    public bool PauseSliderUpdatesA { get; set; }

    public int CurrentTimeA
    {
        get => _currentTimeA;
        set
        {
            if (_currentTimeA != value)
            {
                _currentTimeA = value;
                OnPropertyChanged(nameof(CurrentTimeA));
            }
        }
    }

    public int TotalTimeA
    {
        get => _totalTimeA;
        set
        {
            if (_totalTimeA != value)
            {
                _totalTimeA = value;
                OnPropertyChanged(nameof(TotalTimeA));
            }
        }
    }

    public string CurrentTimeStringA
    {
        get => _currentTimeStringA;
        set
        {
            if (_currentTimeStringA != value)
            {
                _currentTimeStringA = value;
                OnPropertyChanged(nameof(CurrentTimeStringA));
            }
        }
    }

    public string TotalTimeStringA
    {
        get => _totalTimeStringA;
        set
        {
            if (_totalTimeStringA != value)
            {
                _totalTimeStringA = value;
                OnPropertyChanged(nameof(TotalTimeStringA));
            }
        }
    }

    //////////////////////
    ////// PLAYER B //////
    //////////////////////

    public bool PauseSliderUpdatesB { get; set; }

    public int CurrentTimeB
    {
        get => _currentTimeB;
        set
        {
            if (_currentTimeB != value)
            {
                _currentTimeB = value;
                OnPropertyChanged(nameof(CurrentTimeB));
            }
        }
    }

    public int TotalTimeB
    {
        get => _totalTimeB;
        set
        {
            if (_totalTimeB != value)
            {
                _totalTimeB = value;
                OnPropertyChanged(nameof(TotalTimeB));
            }
        }
    }

    public string CurrentTimeStringB
    {
        get => _currentTimeStringB;
        set
        {
            if (_currentTimeStringB != value)
            {
                _currentTimeStringB = value;
                OnPropertyChanged(nameof(CurrentTimeStringB));
            }
        }
    }

    public string TotalTimeStringB
    {
        get => _totalTimeStringB;
        set
        {
            if (_totalTimeStringB != value)
            {
                _totalTimeStringB = value;
                OnPropertyChanged(nameof(TotalTimeStringB));
            }
        }
    }
}