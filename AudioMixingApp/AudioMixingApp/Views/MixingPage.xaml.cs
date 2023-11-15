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

    private void PlayButtonA_Clicked(object sender, EventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;

        string inputFilePath =
            "C:/Users/Yanni/Downloads/@Pantera-Cowboys-from-Hell.mp3";

        vm.PlaySound(inputFilePath, 0.25f);
    }


    private void TimeSliderA_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        MixingPageViewModel vm = (MixingPageViewModel)BindingContext;
        vm.SetTime(e.NewValue);
    }
}