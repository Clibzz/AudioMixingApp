using AudioMixingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AudioMixingApp.Models;

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
                        Source = song.Source
                    });
                }

                // Add the newly created Playlist object to the Playlists collection
                PlaylistsCollection.Add(newPlaylist);
            }
        }
    }
}
