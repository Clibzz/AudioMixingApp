using NAudio.Dsp;
using NAudio.Wave;

namespace AudioMixingApp.Effects;

public class Equalizer : ISampleProvider
{
    private readonly ISampleProvider _source;

    private readonly BiQuadFilter _lowFilter;
    private readonly BiQuadFilter _midFilter;
    private readonly BiQuadFilter _highFilter;

    public Equalizer(ISampleProvider source)
    {
        _source = source;
        
        // Set up the initial filter parameters
        _lowFilter = BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 100, 1, 0);
        _midFilter = BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 1000, 1, 0);
        _highFilter = BiQuadFilter.PeakingEQ(source.WaveFormat.SampleRate, 5000, 1, 0);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samplesRead = _source.Read(buffer, offset, count);

        Process(buffer, offset, samplesRead);

        return samplesRead;
    }

    public WaveFormat WaveFormat => _source.WaveFormat;

    private void Process(float[] buffer, int offset, int count)
    {
        for (int i = 0; i < count; i++)
        {
            buffer[i + offset] = _lowFilter.Transform(buffer[i + offset]);
            buffer[i + offset] = _midFilter.Transform(buffer[i + offset]);
            buffer[i + offset] = _highFilter.Transform(buffer[i + offset]);
        }
    }

    public void SetLows(int value)
    {
        _lowFilter.SetPeakingEq(_source.WaveFormat.SampleRate, 100, 1, value);
    }

    public void SetMids(int value)
    {
        _lowFilter.SetPeakingEq(_source.WaveFormat.SampleRate, 1000, 1, value);
    }

    public void SetHighs(int value)
    {
        _lowFilter.SetPeakingEq(_source.WaveFormat.SampleRate, 5000, 1, value);
    }
}