using AudioMixingApp.Effects;
using NAudio.Wave;

namespace AudioMixingApp.Models;

public class Player
{
    public float FadeVolume = 0.5f;
    // The output device.
    public WaveOutEvent Output { get; set; } = new();
    
    // Event triggered when the next song should play.
    public event EventHandler<StoppedEventArgs> NextSongEvent;

    // The song.
    public AudioFileReader PlayingSong { get; set; }

    // The queue for the songs.
    public Queue<Song> SongQueue = new();
    
    // Event triggered when the queue should be updated on the frontend
    public event EventHandler QueueUpdated;

    public ReverbEffect Reverb {  get; set; }
    public Equalizer Equalizer { get; set; }
    public FlangerEffect Flanger { get; set; }
    public PitchshiftEffect Pitchshifter { get; set; }

    // Counter to give the songs in the queue a unique number
    private int _idCounter;

    /// <summary>
    /// Method <c>AddToQueue</c> adds a song to the queue.
    /// </summary>
    /// <param name="song">the name of the mp3 file that represents the song.</param>
    public void AddToQueue(Song song)
    {
        // Gets the path to the song.
        if (!Directory.Exists(song.FilePath))
        {
            // Add the song to the queue as a new song object.
            SongQueue.Enqueue(new Song() {Artist = song.Artist, FilePath = song.FilePath, Id = _idCounter, Title = song.Title, Duration = song.Duration});
            _idCounter++;
            
            //Update the queue on the frontend
            OnQueueUpdated();
        }
    }

    /// <summary>
    /// Method <c>RemoveFromQueue</c> removes a specific song from the queue.
    /// </summary>
    /// <param name="id">the id of the mp3 file that represents the song.</param>
    /// source: https://kodify.net/csharp/queue/remove/
    public void RemoveFromQueue(int id)
    {
        SongQueue = new(SongQueue.Where(song => song.Id != id));
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
        //Update the queue on the frontend
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
        Pitchshifter = new PitchshiftEffect(Flanger, 1.0f);
        Output.Init(Pitchshifter);

        // Start playback of the queued song.
        Output.Play();

        // Subscribe to the PlaybackStopped event to go to the next song if the song has ended using recursion.
        // source: https://stackoverflow.com/questions/11272872/naudio-how-to-tell-playback-is-completed
        // Output.PlaybackStopped += (sender, e) => PlaySongFromQueue();
        Output.PlaybackStopped += NextSongEvent;
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