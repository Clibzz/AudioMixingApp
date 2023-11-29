using System.ComponentModel;

namespace AudioMixingApp.ViewModels;

public class FilterPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private double _reverbFactor;
    private double _highValue;
    private double _midValue;
    private double _lowValue;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public double ReverbFactor
    {
        get => _reverbFactor;
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
        get => _highValue;
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
        get => _midValue;
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
        get => _lowValue;
        set
        {
            if (_lowValue != value)
            {
                _lowValue = value;
                OnPropertyChanged(nameof(LowValue));
            }
        }
    }
}
