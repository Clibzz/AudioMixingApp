using System.Diagnostics;
using NAudio.Wave;

namespace AudioMixingApp.Effects;

public class Equalizer : ISampleProvider
{
    private readonly ISampleProvider source;
    private readonly int windowSize;
    private readonly int channels;
    private readonly float[] buffer;

    public WaveFormat WaveFormat => source.WaveFormat;

    public Equalizer(ISampleProvider source, int cutoffFrequency)
    {
        this.source = source;
        this.channels = source.WaveFormat.Channels;

        // Calculate the number of samples for the moving average window
        int sampleRate = source.WaveFormat.SampleRate;
        this.windowSize = (int)(sampleRate / (float)cutoffFrequency);

        this.buffer = new float[windowSize * channels];
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samplesRead = source.Read(buffer, offset, count);

        for (int i = 0; i < samplesRead; i += channels)
        {
            ApplyMovingAverage(buffer, offset + i, channels);
        }

        return samplesRead;
    }

    private void ApplyMovingAverage(float[] buffer, int offset, int channels)
    {
        for (int c = 0; c < channels; c++)
        {
            float sum = 0.0f;

            // Calculate the average of the samples in the window
            for (int i = offset - windowSize * channels; i <= offset; i += channels)
            {
                if (i >= 0)
                {
                    sum += buffer[i + c];
                }
            }

            // Update the current sample with the moving average
            this.buffer[c] = sum / windowSize;
        }

        // Copy the filtered samples back to the original buffer
        for (int c = 0; c < channels; c++)
        {
            buffer[offset + c] = this.buffer[c];
        }
    }
}