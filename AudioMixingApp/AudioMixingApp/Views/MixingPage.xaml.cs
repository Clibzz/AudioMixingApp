using System.Diagnostics;
using AudioMixingApp.ViewModels;

namespace AudioMixingApp.Views;

public partial class MixingPage : ContentPage
{
    public MixingPage()
    {
        InitializeComponent();
        BindingContext = new MixingPageViewModel();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        DjPanel.WidthRequest = DjPanel.Height * 2.5;
    }

    /// <summary>
    /// Demo event to play an audio file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;

        string inputFilePath =
            "C:/Users/Yanni/Downloads/@Pantera-Cowboys-from-Hell.mp3";

        vm.PlaySound(inputFilePath, 0.25f);
    }

    /// <summary>
    /// When the user lets go of the slider, update the current playtime to the newly chosen playtime
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Slider_OnDragCompleted(object sender, EventArgs e)
    {
        Trace.WriteLine(sender.GetType());
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        vm.SetTime(((Slider)sender).Value);
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