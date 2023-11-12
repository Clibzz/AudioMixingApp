using AudioMixingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged("Songs");
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
            new Song { Title = "Baboon" },
            new Song { Title = "Capuchin Monkey" },
            // Add more monkeys as needed...
        };
        }
    }
}
