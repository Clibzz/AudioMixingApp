namespace AudioMixingApp.Views;

using AudioMixingApp.Models;
using AudioMixingApp.ViewModels;

public partial class PlaylistPage : ContentPage
{
    private readonly Player _playerA, _playerB;
    public PlaylistPage(Player playerA, Player playerB)
	{
		InitializeComponent();
		BindingContext = new PlaylistsViewModel();
        _playerA = playerA;
        _playerB = playerB;
    }

    public PlaylistPage()
    {
        InitializeComponent();
        BindingContext = new PlaylistsViewModel();
    }

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }

    private void MixingPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PlayerPage());
    }

    private void PlaylistPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PlaylistPage(_playerA, _playerB));
    }

    /// <summary>
    /// Button for adding a playlist to a queue
    /// </summary>
    /// <param name="sender">The button component</param>
    /// <param name="player">The player to add to</param>
    private void AddToQueue(object sender, Player player)
    {
        Button clickedButton = (Button)sender;

        // Here you get the selected playlist 
        Playlist selectedPlaylist = (Playlist)clickedButton.BindingContext;

        if (player != null)
        {
            foreach (Song song in selectedPlaylist.Songs)
            {
                player.AddToQueue(song);
                // Alert that playlist has been added to the queue
                DisplayAlert("Success", $"Playlist {selectedPlaylist.Name} is added to the queue successfully.", "OK");
            }
        }
        else
        {
            DisplayAlert("Error", "Failed to add playlist to the queue.", "OK");
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
}