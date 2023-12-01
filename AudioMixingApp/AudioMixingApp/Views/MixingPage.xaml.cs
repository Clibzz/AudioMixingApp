using AudioMixingApp.ViewModels;
using NAudio.Wave;

namespace AudioMixingApp.Views;

public partial class MixingPage
{
    private readonly MixingPageViewModel _viewModel;
    private readonly ImageSource _playImageSource, _pausedImageSource;
    
    public MixingPage()
    {
        _viewModel = new MixingPageViewModel();
        BindingContext = _viewModel;
        InitializeComponent();

        _playImageSource = ImageSource.FromFile("play.png");
        _pausedImageSource = ImageSource.FromFile("paused.png");
    }

    /// <summary>
    /// Set the dimensions of the DJ Panel to be an aspect ratio of 2 : 5
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        DjPanel.WidthRequest = DjPanel.Height * 2.5;
    }
    
    private void SongsPageButtonA_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SongsPage(_viewModel.GetPlayer('A')));
    }
    
    private void SongsPageButtonB_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SongsPage(_viewModel.GetPlayer('B')));
    }

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
    
    //////////////////////
    ////// PLAYER A //////
    //////////////////////
    
    private void FilterPageButtonA_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EffectPage(_viewModel.GetPlayer('A')));
    }
    
    private void VolumeSliderA_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        _viewModel.ChangeVolume('A', (float)e.NewValue);
    }
    
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        _viewModel.PlaySoundWithPauseCheck('A');

        ImageButton imageButton = (ImageButton)sender;
        
        imageButton.Source = _viewModel.GetPlaybackState('A') == PlaybackState.Playing
            ? _pausedImageSource
            : _playImageSource;
    }
    
    private void ProgressbarSliderA_OnDragCompleted(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime('A', ((Slider)sender).Value);
        _viewModel.PauseSliderUpdatesA = false;
    }

    private void ProgressbarSliderA_OnDragStarted(object sender, EventArgs e)
    {
        _viewModel.PauseSliderUpdatesA = true;
    }

    private void SkipButtonA_OnClicked(object sender, EventArgs e)
    {
        _viewModel.SkipSong('A');
    }

    private void DeleteButtonA_OnClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        _viewModel.DeleteFromQueue('A', int.Parse(button.ClassId));
    }

    /// <summary>
    /// Restarts the current playing song.
    /// </summary>
    private void PreviousButtonA_Clicked(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime('A', 0.0);
    }

    //////////////////////
    ////// PLAYER B //////
    //////////////////////

    private void FilterPageButtonB_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EffectPage(_viewModel.GetPlayer('B')));
    }
    
    private void VolumeSliderB_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        _viewModel.ChangeVolume('B', (float)e.NewValue);
    }
    
    private void PlayButtonB_Clicked(object sender, EventArgs e)
    {
        _viewModel.PlaySoundWithPauseCheck('B');
    }
    
    private void ProgressbarSliderB_OnDragCompleted(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime('B', ((Slider)sender).Value);
        _viewModel.PauseSliderUpdatesB = false;
    }

    private void ProgressbarSliderB_OnDragStarted(object sender, EventArgs e)
    {
        _viewModel.PauseSliderUpdatesB = true;
    }

    private void SkipButtonB_OnClicked(object sender, EventArgs e)
    { 
        _viewModel.SkipSong('B');
    }

    private void DeleteButtonB_OnClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        _viewModel.DeleteFromQueue('B', int.Parse(button.ClassId));
    }


    /// <summary>
    /// Restarts the current playing song.
    /// </summary>
    private void PreviousButtonB_Clicked(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime('B', 0.0);
    }
}