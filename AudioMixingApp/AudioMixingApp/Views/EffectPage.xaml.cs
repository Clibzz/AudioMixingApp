using AudioMixingApp.Models;
using AudioMixingApp.ViewModels;
using System.Diagnostics;

namespace AudioMixingApp.Views;

public partial class EffectPage : ContentPage
{
    private readonly Player _player;
	public EffectPage(Player player)
	{
        _player = player;
        BindingContext = new EffectPageViewModel();
        ((EffectPageViewModel)BindingContext).ReverbFactor = _player.Reverb.ReverbFactor;
        ((EffectPageViewModel)BindingContext).HighValue = _player.Equalizer.HighValue;
        ((EffectPageViewModel)BindingContext).MidValue = _player.Equalizer.MidValue;
        ((EffectPageViewModel)BindingContext).LowValue = _player.Equalizer.LowValue;
        ((EffectPageViewModel)BindingContext).FlangerFactor = _player.Flanger.FlangerFactor;
        ((EffectPageViewModel)BindingContext).PitchValue = _player.Pitchshifter.PitchValue;
        InitializeComponent();
	}

    private void AboutPageButton_OnClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }

    private void ReverbSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Reverb.ReverbFactor = (float)e.NewValue;
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

    private void FlangerSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Flanger.AdjustFlangerFactor((float)e.NewValue / 10);
    }

    private void PitchshiftSlider_OnValueChanged(object sender, ValueChangedEventArgs e)
    {
        _player.Pitchshifter.ChangePitchValue((float)e.NewValue);
    }
}