using System.Diagnostics;
using AudioMixingApp.ViewModels;
using NAudio.Wave;

namespace AudioMixingApp.Views;

public partial class MixingPage : ContentPage
{
    public MixingPage()
    {
        BindingContext = new MixingPageViewModel();
        InitializeComponent();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        DjPanel.WidthRequest = DjPanel.Height * 2.5;
    }

    /// <summary>
    /// Change the volume of the first song that's playing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void SliderVolume1_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        vm.ChangeVolume((float)e.NewValue);
    }

    /// <summary>
    /// Demo event to play an audio file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        
        vm.AddSong("Possessed - The Exorcist.mp3");
        vm.PlaySound();
    }

    /// <summary>
    /// When the user lets go of the slider, update the current playtime to the newly chosen playtime
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Slider_OnDragCompleted(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        
        vm.UpdateCurrentTime(((Slider)sender).Value);
        vm.PauseSliderUpdates = false;
    }

    /// <summary>
    /// When the user starts dragging, tell the viewmodel to stop updating the sliders value
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Slider_OnDragStarted(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        vm.PauseSliderUpdates = true;
    }

    private void SongsPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SongsPage());
    }

    private void FilterPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new FilterPage());
    }

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}