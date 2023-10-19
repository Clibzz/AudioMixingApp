namespace AudioMixingApp.Models;

interface IEffect
{
    public int Min { get; set; }
    public int Max { get; set; }
    public float Value { get; set; }
}