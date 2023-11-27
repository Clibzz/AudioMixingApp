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
        public SongsPage()
        {
            InitializeComponent();
            BindingContext = new SongsViewModel();
        }

        private async void OnAddSongClicked(object sender, EventArgs e)
        {
            string artist;
            string title;
            string filePath;

            do
            {
                artist = await DisplayPromptAsync("Add Song", "Enter Artist");

                if (artist == null) return; // User canceled, exit method

                title = await DisplayPromptAsync("Add Song", "Enter Title");

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

            // Voeg het nummer toe aan het JSON-bestand
            await AddSongToJsonFile(newSong);
        }

        private async Task AddSongToJsonFile(Song song)
        {
            string JsonFilePath = "C:\\Users\\moniq\\C#2_herkansing\\AudioMixingApp\\AudioMixingApp\\AudioMixingApp\\songs.json";

            try
            {
                string jsonContent;

                // Lees de bestaande gegevens uit het JSON-bestand, als het bestand bestaat
                if (File.Exists(JsonFilePath))
                {
                    jsonContent = await File.ReadAllTextAsync(JsonFilePath);
                }
                else
                {
                    jsonContent = "{\"songs\": []}";
                }

                // Deserialiseer de JSON-inhoud naar een SongListWrapper-object
                var songListWrapper = JsonSerializer.Deserialize<SongListWrapper>(jsonContent) ?? new SongListWrapper();

                // Voeg het nieuwe nummer toe aan de lijst
                songListWrapper.Songs.Add(song);

                // Serialiseer het bijgewerkte object terug naar JSON
                string updatedJsonContent = JsonSerializer.Serialize(songListWrapper, new JsonSerializerOptions { WriteIndented = true });

                // Schrijf de bijgewerkte JSON terug naar het bestand
                await File.WriteAllTextAsync(JsonFilePath, updatedJsonContent);

                // Toon een waarschuwingsvenster met de inhoud van het bijgewerkte JSON-bestand
                await DisplayAlert("JSON Updated", "JSON content after update:\n" + updatedJsonContent, "OK");
            }
            catch (Exception ex)
            {
                // Handel eventuele fouten af (bijv. geen schrijfrechten)
                await DisplayAlert("Error", $"Error writing to JSON file: {ex.Message}", "OK");
            }
        }

        public class SongListWrapper
        {
            public List<Song> Songs { get; set; } = new List<Song>();
        }

    }
}
