using AudioMixingApp.Models;
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
        Circle.WidthRequest = CircleParent.Width;
        Circle.HeightRequest = CircleParent.Width;
    }
}