using AudioMixingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static AudioMixingApp.Views.SongsPage;
using System.Text.Json;

namespace AudioMixingApp.ViewModels
{
    public class SongsViewModel
    {
        ObservableCollection<Song> songs;
        string jsonPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AudioMixingApp\songs.json";

        public ObservableCollection<Song> Songs
        {
            get { return songs; }
            set
            {
                if (songs != value)
                {
                    songs = value;
                    OnPropertyChanged(nameof(Songs));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SongsViewModel()
        {
            Songs = LoadSongsFromJsonFile(jsonPath);
        }

        /// <summary>
        /// loads all the songs from the json
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        private ObservableCollection<Song> LoadSongsFromJsonFile(string jsonFilePath)
        {
            ObservableCollection<Song> loadedSongs = new ObservableCollection<Song>();
           
            if (File.Exists(jsonPath))
            {
                string jsonContent = File.ReadAllText(jsonPath);

                // Deserialize the JSON content into a class representing the JSON structure
                var songListWrapper = JsonSerializer.Deserialize<SongListWrapper>(jsonContent);

                if (songListWrapper != null)
                {
                    foreach (var song in songListWrapper.Songs)
                    {
                        // Create a new Song instance
                        var newSong = new Song
                        {
                            Title = song.Title,
                            Artist = song.Artist,
                            FilePath = song.FilePath,
                            Duration = song.Duration
                        };
                        loadedSongs.Add(newSong);
                    }
                }
            }
            return loadedSongs;
        }

        /// <summary>
        /// Method that adds song(s) to the JSON file
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task AddSongToJsonFile(Song song)
        {
            string jsonFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AudioMixingApp\songs.json";

            // Check if json file exists
            if (!File.Exists(jsonFilePath))
            {
                // Create default json structure with the provided song
                string defaultJsonContent = JsonSerializer.Serialize(new
                {
                    Songs = new[]
                    {
                        new
                        {
                            Title = song.Title,
                            Artist = song.Artist,
                            FilePath = song.FilePath,
                            Duration = song.Duration
                        }
                    }
                }, new JsonSerializerOptions { WriteIndented = true });

                // Write default json structure to json file
                await File.WriteAllTextAsync(jsonFilePath, defaultJsonContent);
            }
            else
            {
                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                // Get JSON content and add it to the song list
                var songListWrapper = JsonSerializer.Deserialize<SongListWrapper>(jsonContent) ?? new SongListWrapper();

                // Check if the song is not already in the list before adding it
                if (!songListWrapper.Songs.Any(s => s.Title == song.Title && s.Artist == song.Artist && s.FilePath == song.FilePath))
                {
                    // Add the new song to the list
                    songListWrapper.Songs.Add(song);

                    // Convert the new list to json
                    string updatedJsonContent = JsonSerializer.Serialize(songListWrapper, new JsonSerializerOptions { WriteIndented = true });

                    // Write the updated json to the json file
                    await File.WriteAllTextAsync(jsonFilePath, updatedJsonContent);
                }
            }
        }

        /// <summary>
        /// Add a song to a playlist / create a playlist
        /// </summary>
        /// <param name="playlistName">The name of the (new) playlist</param>
        /// <param name="song">The song that has to be added to the playlist</param>
        /// <returns>A task that writes the json data to the playlists.json file</returns>
        public async Task AddSongToPlaylist(string playlistName, Song song)
        {
            string jsonFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AudioMixingApp\playlists.json";
            // Read the playlist.json file
            string existingJsonContent = File.Exists(jsonFilePath) ? await File.ReadAllTextAsync(jsonFilePath) : "";

            // Create a JsonSerializerOptions allowing trailing commas (basically it means that there's a comma after the last element in an array)
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            // Deserialize existing data or create an entire new list for the playlists
            var playlists = string.IsNullOrEmpty(existingJsonContent)
                ? new List<Playlist>()
                : JsonSerializer.Deserialize<List<Playlist>>(existingJsonContent, options);

            // Try to look for the playlist, if it doesn't exist, create a new one
            Playlist inputPlaylist = playlists.Find(p => p.Name == playlistName);
            if (inputPlaylist == null)
            {
                inputPlaylist = new Playlist { Name = playlistName, Songs = new List<Song>() };
                playlists.Add(inputPlaylist);
            }

            // Add the song to the given playlist
            inputPlaylist.Songs.Add(song);

            // Serialize the updated playlist data
            string updatedJsonData = JsonSerializer.Serialize(playlists, new JsonSerializerOptions { WriteIndented = true });

            // Add the updated json to the playlists.json
            await File.WriteAllTextAsync(jsonFilePath, updatedJsonData);
        }
    }
}
