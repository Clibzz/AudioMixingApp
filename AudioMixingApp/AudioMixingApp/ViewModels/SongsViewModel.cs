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
        string jsonPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\songs.json";

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

        public async Task AddSongToJsonFile(Song song)
        {
            string jsonFilePath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\songs.json";

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
    }
}
