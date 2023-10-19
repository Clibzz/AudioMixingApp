namespace AudioMixingApp.Models.Effects;

class Equalizer : IEffect
{
    public int Min { get; set; }
    public int Max { get; set; }
    public float Value { get; set; }

    public Equalizer(int min, int max, float value)
    {
        Min = min;
        Max = max;
        Value = value;
    }

    public override string ToString()
    {
        return $"{base.ToString()}: Min:{Min}, Max:{Max}, Value:{Value}";
    }
}