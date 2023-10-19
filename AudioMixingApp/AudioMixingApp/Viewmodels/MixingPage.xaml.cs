using AudioMixingApp.Models;

namespace AudioMixingApp.Viewmodels;

public partial class MixingPage : ContentPage
{
    public MixingPage()
    {
        InitializeComponent();

        // x:DataType="eff:Equalizer"
    }

    private void Slider_OnValueChanged(object sender, ValueChangedEventArgs args)
    {
        Song song = (Song)this.BindingContext;
        song.Effects[0].Value = (float)args.NewValue;
        SliderLabel.Text = args.NewValue.ToString("n2");
    }

    private void Entry_OnCompleted(object sender, EventArgs args)
    {
        if (double.TryParse(((Entry)sender).Text, out var value)) {

            value = Math.Clamp(value, ValueSlider.Minimum, ValueSlider.Maximum);
            
            ValueSlider.Value = value;
            ((Entry)sender).Text = value.ToString("n2");

            Song song = (Song)this.BindingContext;
            song.Effects[0].Value = (float)value;
        }
        else
        {
            SliderLabel.Text = ValueSlider.Value.ToString("n2");
        }
    }
}