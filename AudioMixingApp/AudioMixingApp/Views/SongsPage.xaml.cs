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
        private readonly Player _playerA, _playerB;
        public SongsViewModel viewModel;

        public SongsPage(Player playerA, Player playerB)
        {
            _playerA = playerA;
            _playerB = playerB;
            InitializeComponent();
            BindingContext = new SongsViewModel();
            viewModel = new SongsViewModel();
        }

        private async void OnAddMultipleSongsClicked(object sender, EventArgs e)
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { "public.audio", "public.mp3" } },
                { DevicePlatform.Android, new[] { "audio/mpeg", "audio/*", "application/octet-stream" } },
                { DevicePlatform.WinUI, new[] { ".mp3" } },
                { DevicePlatform.Tizen, new[] { "audio/*" } },
                { DevicePlatform.macOS, new[] { "public.audio", "public.mp3" } },
                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a mp3 file",
                FileTypes = customFileType,
            };

            IEnumerable<FileResult> selectedFiles = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Please select mp3 files",
                FileTypes = customFileType,
            });

            if (selectedFiles == null || !selectedFiles.Any())
            {
                // User canceled the selection
                return;
            }

            var viewModel = (SongsViewModel)BindingContext;

            foreach (FileResult fileResult in selectedFiles)
            {
                // Get the selected file path
                string filePath = fileResult.FullPath;

                // Get metadata from the file using TagLib#
                var file = TagLib.File.Create(filePath);
                string artist = file.Tag.FirstPerformer;
                string title = file.Tag.Title;

                TimeSpan seconds = file.Properties.Duration;

                //format hh:mm:ss
                string formattedDuration = $"{(int)seconds.TotalHours:D2}:{seconds.Minutes:D2}:{seconds.Seconds:D2}";

                string format = "hh\\:mm\\:ss";

                TimeSpan duration = TimeSpan.ParseExact(formattedDuration, format, null);

                // Save the file to this path
                string documentsPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\Songs\";

                if (!Directory.Exists(documentsPath)) Directory.CreateDirectory(documentsPath);

                string destinationPath = Path.Combine(documentsPath, Path.GetFileName(filePath));

                // Copy the file to the destination path
                File.Copy(filePath, destinationPath, true);

                // Create a new Song object
                var newSong = new Song { Title = title, Artist = artist, FilePath = destinationPath, Duration = duration};

                // Add to the collection
                viewModel.Songs.Add(newSong);

                // Save the song to JSON
                await viewModel.AddSongToJsonFile(newSong);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                // MIME type validation
                if (!string.Equals(fileResult.ContentType, "audio/mpeg", StringComparison.OrdinalIgnoreCase))
                {
                    // Error message, wrong MIME type
                    await DisplayAlert("Error", "Please select a valid audio file.", "OK");
                    continue; // Restart the loop to prompt the user again
                }
            }
            while (!fileResult.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase));

            // Get the file path and save it in the existing 'filePath' variable
            filePath = fileResult.FullPath;

            var file = TagLib.File.Create(filePath);
            TimeSpan seconds = file.Properties.Duration;

            //format hh:mm:ss
            string formattedDuration = $"{(int)seconds.TotalHours:D2}:{seconds.Minutes:D2}:{seconds.Seconds:D2}";

            string format = "hh\\:mm\\:ss";

            TimeSpan duration = TimeSpan.ParseExact(formattedDuration, format, null);

            // Save the file to this path
            string documentsPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\Songs\";

            if (!Directory.Exists(documentsPath)) Directory.CreateDirectory(documentsPath);

            string destinationPath = Path.Combine(documentsPath, Path.GetFileName(filePath));

            // Copy the file to the destination path
            File.Copy(filePath, destinationPath, true);

            // Add to the collection
            var viewModel = (SongsViewModel)BindingContext;
            var newSong = new Song { Title = title, Artist = artist, FilePath = destinationPath, Duration = duration };

            viewModel.Songs.Add(newSong);

            // add song to JSON file
            await viewModel.AddSongToJsonFile(newSong);
        }

        /// <summary>
        /// A wrapper class that encapsulates a list of songs.
        /// Necessary for successful JSON deserialization into a list of songs,
        /// as it provides a structured representation of the JSON data.
        /// </summary>
        public class SongListWrapper
        {
            public List<Song> Songs { get; set; } = new List<Song>();
        }
        
        /// <summary>
        /// Button for adding a song to a queue. A or B
        /// </summary>
        /// <param name="sender">The button component</param>
        /// <param name="player">The player to add to</param>
        private async void AddToQueue(object sender, Player player)
        {
            Button clickedButton = (Button)sender;

            // Here u get the selected song object
            Song selectedSong = (Song)clickedButton.BindingContext;

            if (player != null)
            {
                player.AddToQueue(selectedSong);

                //Alert that song has been added to the queue
                await DisplayAlert("Success", $"{selectedSong.Title} is added to the queue successfully.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to add song to the queue.", "OK");
            }
        }
        
        private void AddToQueueA_OnClicked(object sender, EventArgs e)
        {
            AddToQueue(sender, _playerA);
        }
        
        private void AddToQueueB_OnClicked(object sender, EventArgs e)
        {
            AddToQueue(sender, _playerB);
        }

        /// <summary>
        /// Add a song to a playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void PlaylistBtn_OnClicked(Object sender, EventArgs e)
        {
            string playlistName = await Application.Current.MainPage.DisplayPromptAsync("New Playlist", "Enter the playlist name", "OK", "Cancel", keyboard: Keyboard.Text);

            Button clickedButton = (Button)sender;

            // Here u get the selected song object
            Song selectedSong = (Song)clickedButton.BindingContext;
            if (playlistName != null)
            {
                await viewModel.AddSongToPlaylist(playlistName, selectedSong);
            }
        }

    }
}
