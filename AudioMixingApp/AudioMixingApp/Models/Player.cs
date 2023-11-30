using AudioMixingApp.Effects;
using NAudio.Wave;

namespace AudioMixingApp.Models;

public class Player
{
    // The output device.
    public WaveOutEvent Output { get; set; } = new();

    // The song.
    public AudioFileReader PlayingSong { get; set; }

    // The queue for the songs.
    public Queue<Song> SongQueue = new();
    public event EventHandler QueueUpdated;

    public ReverbEffect Reverb {  get; set; }
    public Equalizer Equalizer { get; set; }
    public FlangerEffect Flanger { get; set; }

    /// <summary>
    /// Method <c>AddToQueue</c> adds a song to the queue.
    /// </summary>
    /// <param name="song">the name of the mp3 file that represents the song.</param>
    public void AddToQueue(Song song)
    {
        // Gets the path to the song.
        if (!Directory.Exists(song.FilePath))
        {
            // Adds the path to the song to the queue.
            SongQueue.Enqueue(song);
        }
        OnQueueUpdated();
    }

    /// <summary>
    /// Method <c>RemoveFromQueue</c> removes a specific song from the queue.
    /// </summary>
    /// <param name="filepath">the file path of the mp3 file that represents the song.</param>
    /// source: https://kodify.net/csharp/queue/remove/
    public void RemoveFromQueue(string filepath)
    {
        SongQueue = new(SongQueue.Where(song => song.FilePath != filepath));
        OnQueueUpdated();
    }

    /// <summary>
    /// Method <c>PlaySongFromQueue</c> plays the next song in the queue.
    /// </summary>
    public void PlaySongFromQueue()
    {
        // Run the logic if there are 1 or more songs in the queue.
        if (SongQueue.Count <= 0) return;

        // Get the next song in the queue.
        Song song = SongQueue.Dequeue();
        OnQueueUpdated();
        // If the function gets called while a song is playing, stop playing the song so that the next song can play.
        if (PlayingSong != null)
        {
            Output.Stop();
        }

        // Prepare the song for playback.
        PlayingSong = new(song.FilePath);
        Reverb = new ReverbEffect(PlayingSong, 0.0f);
        Equalizer = new Equalizer(Reverb);
        Flanger = new FlangerEffect(Equalizer, 0.0f);
        Output.Init(Flanger);

        // Start playback of the queued song.
        Output.Play();

        // Subscribe to the PlaybackStopped event to go to the next song if the song has ended using recursion.
        // source: https://stackoverflow.com/questions/11272872/naudio-how-to-tell-playback-is-completed
        Output.PlaybackStopped += (sender, e) => PlaySongFromQueue();
    }
    
    /// <summary>
    /// Trigger the event to update the queue to the frontend
    /// </summary>
    private void OnQueueUpdated()
    {
        QueueUpdated?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Method <c>TogglePlayback</c> toggles the playback state of the song that is currently playing.
    /// </summary>
    public PlaybackState TogglePlayback()
    {
        // Pause the song if the song is not paused.
        if (Output.PlaybackState == PlaybackState.Playing)
        {
            Output.Pause();
        }
        // Play the song if the song is paused.
        else
        {
            Output.Play();
        }
        return Output.PlaybackState;
    }

    /// <summary>
    /// Pause the currently playing song
    /// </summary>
    public void Pause()
    {
        Output.Pause();
    }

    /// <summary>
    /// Method <c>SkipSong</c> goes to the next song by stopping the output so that the PlaybackStopped event is called.
    /// </summary>
    public void SkipSong()
    {
        Output.Stop();
    }
}