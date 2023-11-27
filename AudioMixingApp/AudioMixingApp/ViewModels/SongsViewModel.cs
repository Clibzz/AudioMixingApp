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
                    Console.WriteLine("Songs collection updated");
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

            try
            {
                if (File.Exists(jsonPath))
                {
                    string jsonContent = File.ReadAllText(jsonPath);

                    // Deserialize the JSON content into a class representing your JSON structure
                    var songListWrapper = JsonSerializer.Deserialize<SongListWrapper>(jsonContent);

                    if (songListWrapper != null)
                    {
                        foreach (var song in songListWrapper.Songs)
                        {
                            // Create a new Song instance and set its properties
                            var newSong = new Song
                            {
                                Title = song.Title,
                                Artist = song.Artist,
                                FilePath = song.FilePath
                            };

                            loadedSongs.Add(newSong);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, JSON parsing errors)
                Console.WriteLine($"Error loading songs from JSON file: {ex.Message}");
            }

            return loadedSongs;
        }
    }
}
