using AudioMixingApp.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AudioMixingApp.ViewModels
{
    internal class PlaylistsViewModel
    {
        ObservableCollection<Playlist> playlistsCollection;

        public ObservableCollection<Playlist> PlaylistsCollection
        {
            get { return playlistsCollection; }
            set
            {
                playlistsCollection = value;
                OnPropertyChanged(nameof(PlaylistsCollection));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PlaylistsViewModel()
        {
            if (File.Exists($@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\AudioMixingApp\playlists.json"))
            {
                List<Playlist> playlists = JsonConvert.DeserializeObject<List<Playlist>>(Utils.GetJSON());
                PlaylistsCollection = new ObservableCollection<Playlist>();
                foreach (var playlist in playlists)
                {
                    // Create a new Playlist object
                    Playlist newPlaylist = new() { Name = playlist.Name };

                    // Iterate through songs in the playlist and add them to the Playlist object
                    foreach (var song in playlist.Songs)
                    {
                        newPlaylist.Songs.Add(new Song
                        {
                            Title = song.Title,
                            Artist = song.Artist,
                            FilePath = song.FilePath
                        });
                    }

                    // Add the newly created Playlist object to the Playlists collection
                    PlaylistsCollection.Add(newPlaylist);
                }
            }
        }
    }
}
