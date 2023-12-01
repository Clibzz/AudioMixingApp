using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
    
    public ObservableCollection<Song> PlayerAQueue { get; } = new();
    public ObservableCollection<Song> PlayerBQueue { get; } = new();

    public MixingPageViewModel()
    {
        _timer = new System.Timers.Timer
        {
            Interval = 100
        };
        _timer.Start();

        _playerA = new Player();
        _playerB = new Player();

        _playerA.QueueUpdated += UpdateQueueA;
        _playerB.QueueUpdated += UpdateQueueB;

        _playerA.NextSongEvent += (_, _) => PlaySound('A');
        _playerB.NextSongEvent += (_, _) => PlaySound('B');
    }
    
    /// <summary>
    /// Event to trigger when queue A should be updated on the frontend
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateQueueA(object sender, EventArgs e)
    {
        PlayerAQueue.Clear();
        foreach (var item in _playerA.SongQueue)
        {
            PlayerAQueue.Add(item);
        }
        OnPropertyChanged(nameof(PlayerAQueue));
    }
    
    /// <summary>
    /// Event to trigger when queue B should be updated on the frontend
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateQueueB(object sender, EventArgs e)
    {
        PlayerBQueue.Clear();
        foreach (var item in _playerB.SongQueue)
        {
            PlayerBQueue.Add(item);
        }
        OnPropertyChanged(nameof(PlayerBQueue));
    }

    /// <summary>
    /// Get the player instance from the character representing it
    /// </summary>
    /// <param name="playerChar">'A' or 'B'</param>
    /// <returns>PlayerA or PlayerB respectively</returns>
    /// <exception cref="ArgumentException"></exception>
    public Player GetPlayer(char playerChar)
    {
        if (playerChar != 'A' && playerChar != 'B')
            throw new ArgumentException();

        return playerChar == 'A' ? _playerA : _playerB;
    }

    public void DeleteFromQueue(char playerChar, int id)
    {
        GetPlayer(playerChar).RemoveFromQueue(id);
    }
    
    //Property changed event to update the frontend
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PlaybackState GetPlaybackState(char playerChar)
    {
        return GetPlayer(playerChar).Output.PlaybackState;
    }

    //////////////////////////////////////
    ////// PLAYBACK FUNCTIONALITIES //////
    //////////////////////////////////////

    public void PlaySoundWithPauseCheck(char playerChar)
    {
        Player player = GetPlayer(playerChar);
        //Play or pause if a song is already playing
        if (player.PlayingSong != null)
        {
            player.TogglePlayback();
        }
        else
        {
            PlaySound(playerChar);
        }
    }

    /// <summary>
    /// Play the song and update the slider values and labels
    /// </summary>
    /// <param name="playerChar">'A' or 'B'</param>
    private void PlaySound(char playerChar)
    {
        Player player = GetPlayer(playerChar);

        //Get a song from the queue
        player.PlaySongFromQueue();

        //If it couldn't get a song from the queue, it means the queue is empty, so stop.
        if (player.PlayingSong == null) return;

        PauseSliderUpdatesA = false;
        PauseSliderUpdatesB = false;

        int totalTime = (int)player.PlayingSong.TotalTime.TotalSeconds;
        string totalTimeString = player.PlayingSong.TotalTime.ToString(@"hh\:mm\:ss");

        Trace.WriteLine(player.PlayingSong.TotalTime.TotalSeconds);
        Trace.WriteLine(totalTimeString);

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
            int currentTime = (int)player.PlayingSong.CurrentTime.TotalSeconds;
            string currentTimeString = player.PlayingSong.CurrentTime.ToString(@"hh\:mm\:ss");

            if (playerChar == 'A' && !PauseSliderUpdatesA)
            {
                CurrentTimeA = currentTime;
                CurrentTimeStringA = currentTimeString;
            }
            else if (playerChar == 'B' && !PauseSliderUpdatesB)
            {
                CurrentTimeB = currentTime;
                CurrentTimeStringB = currentTimeString;
            }
        };
    }

    /// <summary>
    /// Update the current time of the slider and start playing the song if its stopped
    /// </summary>
    /// <param name="playerChar">'A' or 'B'</param>
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

    /// <summary>
    /// Change the volume of a player
    /// </summary>
    /// <param name="player">'A' or 'B'</param>
    /// <param name="volume">number from 0 to 1</param>
    public void ChangeVolume(char player, float volume)
    {
        GetPlayer(player).Output.Volume = volume;
    }

    /// <summary>
    /// Skip the currently playing song and play the next in the queue
    /// </summary>
    /// <param name="player">'A' or 'B'</param>
    public void SkipSong(char player)
    {
        GetPlayer(player).SkipSong();
        if (GetPlayer(player).SongQueue.Count == 0)
        {
            if (player == 'A')
            {
                CurrentTimeA = TotalTimeA;
                CurrentTimeStringA = _totalTimeStringA;
                PauseSliderUpdatesA = true;
            }
            else
            {
                CurrentTimeB = TotalTimeB;
                CurrentTimeStringB = _totalTimeStringB;
                PauseSliderUpdatesB = true;
            }
        }
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