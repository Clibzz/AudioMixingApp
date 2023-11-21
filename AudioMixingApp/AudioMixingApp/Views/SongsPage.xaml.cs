using System;
using System.IO;
using System.Threading.Tasks;
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
            string title = await DisplayPromptAsync("Add Song", "Enter Title");
            string artist = await DisplayPromptAsync("Add Song", "Enter Artist");

            // Use Xamarin.Essentials FilePicker to pick any file
            FileResult fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an audio file"
            });

            if (fileResult != null)
            {
                // Get the file path
                string filePath = fileResult.FullPath;

                // Save the file to the desired path
                string documentsPath = $@"C:\Users\{Environment.UserName}\Documents\AudioMixingApp\Songs\";
                string destinationPath = Path.Combine(documentsPath, Path.GetFileName(filePath));

                // Copy the file to the destination path
                File.Copy(filePath, destinationPath, true);

                // Now you have the title, artist, and file path, and you can add them to your collection.
                var viewModel = (SongsViewModel)BindingContext;
                viewModel.Songs.Add(new Song { Title = title, Artist = artist });
            }
        }
    }
}
