namespace AudioMixingApp.Models.Effects;

class Equalizer : Effect
{
    public float low;
    public float mid;
    public float high;

    public Equalizer(float low, float mid, float high)
    {
        this.low = low;
        this.mid = mid;
        this.high = high;
    }
}
