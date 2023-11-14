﻿using AudioMixingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AudioMixingApp.ViewModels
{
    public class SongsViewModel
    {
        ObservableCollection<Song> songs;
        public ObservableCollection<Song> Songs
        {
            get { return songs; }
            set
            {
                songs = value;
                OnPropertyChanged(nameof(Songs));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SongsViewModel()
        {
            Songs = new ObservableCollection<Song>
            {
                new Song { Title = "Cowboys from hell", Artist = "Pantera" },
                new Song { Title = "Bleed", Artist = "Meshuggah" },
            };
        }
    }
}