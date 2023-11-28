using NAudio.Wave;
using static System.Environment;

namespace AudioMixingApp
{
    public class Player
    {
        // The output device.
        public WaveOutEvent Output { get; set; }
        // The song.
        public AudioFileReader PlayingSong {  get; set; }
        // The queue for the songs.
        public Queue<string> SongQueue {  get; set; }
        public Player()
        {
            Output = new();
            SongQueue = new();
        }

        /// <summary>
        /// Method <c>AddToQueue</c> adds a song to the queue.
        /// </summary>
        /// <param name="songName">the name of the mp3 file that represents the song.</param>
        public void AddToQueue(string songName)
        {
            // Gets the path to the song.
            string documentsPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\Songs\";
            
            if (!Directory.Exists(documentsPath)) Directory.CreateDirectory(documentsPath);
            
            string song = documentsPath + songName;
            if (!File.Exists(song)) return;
            
            // Adds the path to the song to the queue.
            SongQueue.Enqueue(song);
        }

        /// <summary>
        /// Method <c>RemoveFromQueue</c> removes a specific song from the queue.
        /// </summary>
        /// <param name="songName">the name of the mp3 file that represents the song.</param>
        /// source: https://kodify.net/csharp/queue/remove/
        public void RemoveFromQueue(string songName) 
        {
            // gets the path to the song.
            string projectDirectory = Directory.GetParent(CurrentDirectory).Parent.Parent.FullName + @"\";
            string pathToSongs = projectDirectory + @"Songs\";
            string song = pathToSongs + songName;
            // Removes the song from the queue.
            SongQueue = new(SongQueue.Where(x => x != song));
        }

        /// <summary>
        /// Method <c>PlaySongFromQueue</c> plays the next song in the queue.
        /// </summary>
        public void PlaySongFromQueue()
        {
            // Run the logic if there are 1 or more songs in the queue.
            if (SongQueue.Count <= 0) return;

            // Get the next song in the queue.
            string song = SongQueue.Dequeue();
            // If the function gets called while a song is playing, stop playing the song so that the next song can play.
            if (PlayingSong != null)
            {
                Output.Stop();
            }

            // Prepare the song for playback.
            PlayingSong = new(song);
            Output.Init(PlayingSong);

            // Start playback of the queued song.
            Output.Play();

            // Subscribe to the PlaybackStopped event to go to the next song if the song has ended using recursion.
            // source: https://stackoverflow.com/questions/11272872/naudio-how-to-tell-playback-is-completed
            Output.PlaybackStopped += (sender, e) => PlaySongFromQueue();
        }

        /// <summary>
        /// Method <c>TogglePlayback</c> toggles the playback state of the song that is currently playing.
        /// </summary>
        public void TogglePlayback()
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
        }

        /// <summary>
        /// Method <c>SkipSong</c> goes to the next song by stopping the output so that the PlaybackStopped event is called.
        /// </summary>
        public void SkipSong()
        {
            Output.Stop();
        }
    }
}
