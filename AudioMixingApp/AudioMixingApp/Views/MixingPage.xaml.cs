using System.Diagnostics;
using AudioMixingApp.ViewModels;

namespace AudioMixingApp.Views;

public partial class MixingPage
{
    private readonly MixingPageViewModel _viewModel;
    
    public MixingPage()
    {
        _viewModel = new MixingPageViewModel();
        BindingContext = _viewModel;
        InitializeComponent();
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

    private void PlaylistsPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PlaylistPage());
    }

    private void FadeSlider_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        float volumeA = (float)e.NewValue;
        float volumeB = 1.0f - volumeA;

        _viewModel.ChangeVolume('A', volumeA);
        _viewModel.ChangeVolume('B', volumeB);
    }

    //////////////////////
    ////// PLAYER A //////
    //////////////////////

    private void EffectPageButtonA_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EffectPage(_viewModel.GetPlayer('A')));
    }
    
    private void VolumeSliderA_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        _viewModel.ChangeVolume('A', (float)e.NewValue);
    }
    
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        _viewModel.PlaySound('A');
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
    
    //////////////////////
    ////// PLAYER B //////
    //////////////////////
    
    private void EffectPageButtonB_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EffectPage(_viewModel.GetPlayer('B')));
        Trace.WriteLine(sender);
    }
    
    private void VolumeSliderB_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        _viewModel.ChangeVolume('B', (float)e.NewValue);
    }
    
    private void PlayButtonB_Clicked(object sender, EventArgs e)
    {
        _viewModel.PlaySound('B');
        Trace.WriteLine(sender);
    }
    
    private void ProgressbarSliderB_OnDragCompleted(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime('B', ((Slider)sender).Value);
        _viewModel.PauseSliderUpdatesB = false;
        Trace.WriteLine(sender);
    }

    private void ProgressbarSliderB_OnDragStarted(object sender, EventArgs e)
    {
        _viewModel.PauseSliderUpdatesB = true;
        Trace.WriteLine(sender);
    }

    private void SkipButtonB_OnClicked(object sender, EventArgs e)
    {
        _viewModel.SkipSong('B');
    }
}