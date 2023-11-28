using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using AudioMixingApp.Models;
using AudioMixingApp.ViewModels;
using Microsoft.Maui.Controls;


namespace AudioMixingApp.Views
{
    public partial class SongsPage : ContentPage
    {
        private readonly Player _player;
        public List<Song> Songs { get; set; } = new List<Song>();
        public SongsViewModel viewModel;

        public SongsPage(Player player)
        {
            _player = player;
            InitializeComponent();
            BindingContext = new SongsViewModel();
            viewModel = new SongsViewModel();
        }

        private async void OnAddSongClicked(object sender, EventArgs e)
        {
            string artist;
            string title;
            string filePath;

            do
            {
                artist = await DisplayPromptAsync("Add Song", "Enter Artist name:");

                if (artist == null) return; // User canceled, exit method

                title = await DisplayPromptAsync("Add Song", "Enter Title:");

                if (title == null) return; // Check if the user canceled

                // Show an error message if the artist or title is empty
                if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(title))
                {
                    await DisplayAlert("Error", "Please enter a valid artist and title.", "OK");
                }
            }
            while (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(title));

            FileResult fileResult;

            do
            {
                fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Pick an audio file",
                });

                // Check if the user canceled
                if (fileResult == null)
                {
                    return; // Exit the method if the user canceled
                }

                // File validation on .mp3 extension
                if (!fileResult.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                {
                    // Error message, wrong file
                    await DisplayAlert("Error", "Please select a valid .mp3 file.", "OK");
                }
            }
            while (!fileResult.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));

            // Get the file path and save it in the existing 'filePath' variable
            filePath = fileResult.FullPath;

            // Save the file to this path
            string documentsPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\Songs\";

            if (!Directory.Exists(documentsPath)) Directory.CreateDirectory(documentsPath);

            string destinationPath = Path.Combine(documentsPath, Path.GetFileName(filePath));

            // Copy the file to the destination path
            File.Copy(filePath, destinationPath, true);

            // Add to the collection
            var viewModel = (SongsViewModel)BindingContext;
            var newSong = new Song { Title = title, Artist = artist, FilePath = destinationPath };

            viewModel.Songs.Add(newSong);

            // add song to JSON file
            await viewModel.AddSongToJsonFile(newSong);
        }

        

        public class SongListWrapper
        {
            public List<Song> Songs { get; set; } = new List<Song>();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            _player.AddToQueue("test2.mp3");
        }
    }
}
