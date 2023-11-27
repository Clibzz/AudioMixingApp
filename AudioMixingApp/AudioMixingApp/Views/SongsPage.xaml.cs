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
        private SongsViewModel _viewModel;
        public List<Song> Songs { get; set; } = new List<Song>();

        public SongsPage()
        {
            InitializeComponent();
            BindingContext = new SongsViewModel();
            _viewModel = new SongsViewModel();
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
            await AddSongToJsonFile(newSong);
        }

        private async Task AddSongToJsonFile(Song song)
        {
            string jsonFilePath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\songs.json";
            try
            {
                //check if json file exists
                if (!File.Exists(jsonFilePath))
                {
                    //Create default json structure
                    string defaultJsonContent = JsonSerializer.Serialize(new
                    {
                        Songs = new[]
                        {
                    new
                    {
                        Title = "",
                        Artist = "",
                        FilePath = ""
                    }
                }
                    }, new JsonSerializerOptions { WriteIndented = true });

                    // write default json structure to json file
                    await File.WriteAllTextAsync(jsonFilePath, defaultJsonContent);
                }

                string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                // Get JSON content and add it to song list
                var songListWrapper = JsonSerializer.Deserialize<SongListWrapper>(jsonContent) ?? new SongListWrapper();

                // add new song to list
                songListWrapper.Songs.Add(song);

                // Convert new list to json
                string updatedJsonContent = JsonSerializer.Serialize(songListWrapper, new JsonSerializerOptions { WriteIndented = true });

                //Write updated json to the json file
                await File.WriteAllTextAsync(jsonFilePath, updatedJsonContent);

                //Alert that song has been added
                await DisplayAlert("Added song", "the song has been succesfully added", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error writing to JSON file: {ex.Message}", "OK");
            }
        }


        public class SongListWrapper
        {
            public List<Song> Songs { get; set; } = new List<Song>();
        }

    }
}
