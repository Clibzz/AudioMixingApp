using System.ComponentModel;

namespace AudioMixingApp.ViewModels;

public class EffectPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private double _reverbFactor;
    private double _highValue;
    private double _midValue;
    private double _lowValue;
    private double _flangerFactor;
    private double _pitchValue;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public double ReverbFactor
    {
        get => Math.Round(_reverbFactor, 4);
        set
        {
            if (_reverbFactor != value)
            {
                _reverbFactor = value;
                OnPropertyChanged(nameof(ReverbFactor));
            }
        }
    }

    public double HighValue
    {
        get => Math.Round(_highValue, 4);
        set
        {
            if (_highValue != value)
            {
                _highValue = value;
                OnPropertyChanged(nameof(HighValue));
            }
        }
    }

    public double MidValue
    {
        get => Math.Round(_midValue, 4);
        set
        {
            if (_midValue != value)
            {
                _midValue = value;
                OnPropertyChanged(nameof(MidValue));
            }
        }
    }

    public double LowValue
    {
        get => Math.Round(_lowValue, 4);
        set
        {
            if (_lowValue != value)
            {
                _lowValue = value;
                OnPropertyChanged(nameof(LowValue));
            }
        }
    }

    public double FlangerFactor
    {
        get => Math.Round(_flangerFactor, 4);
        set
        {
            if (_flangerFactor != value)
            {
                _flangerFactor = value;
                OnPropertyChanged(nameof(FlangerFactor));
            }
        }
    }

    public double PitchValue
    {
        get => Math.Round(_pitchValue, 4);
        set
        {
            if (_pitchValue != value)
            {
                _pitchValue = value;
                OnPropertyChanged(nameof(PitchValue));
            }
        }
    }
}
