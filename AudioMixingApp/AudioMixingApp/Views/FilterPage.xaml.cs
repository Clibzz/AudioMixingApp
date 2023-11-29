using AudioMixingApp.Models;
using AudioMixingApp.ViewModels;
using System.Diagnostics;

namespace AudioMixingApp.Views;

public partial class FilterPage : ContentPage
{
    private readonly Player _player;
	public FilterPage(Player player)
	{
        _player = player;
        BindingContext = new FilterPageViewModel();
        ((FilterPageViewModel)BindingContext).ReverbFactor = _player.Reverb.ReverbFactor;
        ((FilterPageViewModel)BindingContext).HighValue = _player.Equalizer.HighValue;
        ((FilterPageViewModel)BindingContext).MidValue = _player.Equalizer.MidValue;
        ((FilterPageViewModel)BindingContext).LowValue = _player.Equalizer.LowValue;
        InitializeComponent();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }

    private void ReverbSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Reverb.ReverbFactor = (float)e.NewValue;
        Trace.WriteLine(_player.Reverb.ReverbFactor);
    }

    private void HighsSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Equalizer.SetHighs((float)e.NewValue);
    }

    private void MidsSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Equalizer.SetMids((float)e.NewValue);
    }

    private void LowsSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Equalizer.SetLows((float)e.NewValue);
    }
}