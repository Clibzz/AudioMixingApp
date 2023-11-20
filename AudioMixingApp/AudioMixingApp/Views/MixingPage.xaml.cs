using System.Diagnostics;
using AudioMixingApp.ViewModels;
using NAudio.Wave;

namespace AudioMixingApp.Views;

public partial class MixingPage : ContentPage
{
    /*TODO: Get song in a less hardcoded way*/
    private WaveOutEvent output;
    public MixingPage()
    {
        InitializeComponent();
        BindingContext = new MixingPageViewModel();
        output = new WaveOutEvent();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        DjPanel.WidthRequest = DjPanel.Height * 2.5;
    }

    /*TODO: Bind to the actual play button*/
    private void TestBtn_Clicked(object sender, EventArgs e)
    {
        /*TODO: Change hardcoded file path*/
        string filePath = @"C:\xampp\htdocs\AudioMixingApp\AudioMixingApp\AudioMixingApp\Maneskin-Beggin-Lyrics.mp3";
        var audioFile = new AudioFileReader(filePath);

        if (output.PlaybackState != PlaybackState.Playing)
        {
            output.Init(audioFile);
            output.Play();
        }
    }

    /// <summary>
    /// Change the volume of the first song that's playing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void SliderVolume1_OnDragCompleted(object sender, ValueChangedEventArgs e)
    {
        if (output != null)
        {
            output.Volume = (float)e.NewValue;
        }
    }

    /// <summary>
    /// Demo event to play an audio file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        
        vm.AddSong("cfh.mp3");
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
}