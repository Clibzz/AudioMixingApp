using System.Diagnostics;
using AudioMixingApp.ViewModels;
using NAudio.Wave;

namespace AudioMixingApp.Views;

public partial class MixingPage : ContentPage
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
    
    private void SongsPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SongsPage(_viewModel._player));
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
        Navigation.PushAsync(new FilterPage(_viewModel._player));
    }
    
    private void VolumeSliderA_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        _viewModel.ChangeVolume((float)e.NewValue);
    }
    
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        _viewModel.PlaySound();
    }
    
    private void ProgressbarSliderA_OnDragCompleted(object sender, EventArgs e)
    {
        _viewModel.UpdateCurrentTime(((Slider)sender).Value);
        _viewModel.PauseSliderUpdates = false;
    }

    private void ProgressbarSliderA_OnDragStarted(object sender, EventArgs e)
    {
        _viewModel.PauseSliderUpdates = true;
    }

    private void SkipButtonA_OnClicked(object sender, EventArgs e)
    {
        _viewModel.SkipSong();
    }
    
    //////////////////////
    ////// PLAYER B //////
    //////////////////////
    
    private void FilterPageButtonB_OnClicked(object sender, EventArgs e)
    {
        // Navigation.PushAsync(new FilterPage(_viewModel._player));
        Trace.WriteLine(sender);
    }
    
    private void VolumeSliderB_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        // _viewModel.ChangeVolume((float)e.NewValue);
        Trace.WriteLine(sender);
    }
    
    private void PlayButtonB_Clicked(object sender, EventArgs e)
    {
        // _viewModel.PlaySound();
        Trace.WriteLine(sender);
    }
    
    private void ProgressbarSliderB_OnDragCompleted(object sender, EventArgs e)
    {
        // _viewModel.UpdateCurrentTime(((Slider)sender).Value);
        // _viewModel.PauseSliderUpdates = false;
        Trace.WriteLine(sender);
    }

    private void ProgressbarSliderB_OnDragStarted(object sender, EventArgs e)
    {
        // _viewModel.PauseSliderUpdates = true;
        Trace.WriteLine(sender);
    }

    private void SkipButtonB_OnClicked(object sender, EventArgs e)
    {
        // _viewModel.SkipSong();
        Trace.WriteLine(sender);
    }
    
}