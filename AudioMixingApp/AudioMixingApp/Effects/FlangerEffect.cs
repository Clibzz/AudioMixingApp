using NAudio.Wave;
using System;

public class FlangerEffect : ISampleProvider
{
    private readonly ISampleProvider source;
    private readonly int delayMilliseconds;
    private readonly float feedback;
    private readonly int sampleRate;
    private readonly float modulationFrequency; // Frequency of the modulation LFO

    private int delaySamples;
    private float[] delayBuffer;
    private int delayBufferPosition;

    // Extra field to store the current flanger factor
    private float flangerFactor;

    public FlangerEffect(ISampleProvider source, int delayMilliseconds, float feedback, int sampleRate, float modulationFrequency)
    {
        this.source = source;
        this.delayMilliseconds = delayMilliseconds;
        this.feedback = feedback;
        this.sampleRate = sampleRate;
        this.modulationFrequency = modulationFrequency;

        this.delaySamples = (int)((delayMilliseconds / 1000.0) * sampleRate);
        this.delayBuffer = new float[delaySamples];
        this.delayBufferPosition = 0;

        // Initialize flanger factor 
        this.flangerFactor = 0.5f; 
    }

    // Method to adjust the flanger factor dynamically
    public void AdjustFlangerFactor(float factor)
    {
        // Ensure the factor is within the valid range (0.0 to 1.0)
        this.flangerFactor = Math.Max(0.0f, Math.Min(1.0f, factor));
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samplesRead = source.Read(buffer, offset, count);

        for (int n = 0; n < samplesRead; n++)
        {
            float input = buffer[offset + n];
            float modulation = (float)Math.Sin(2 * Math.PI * modulationFrequency * n / sampleRate); 

          
            int modulatedDelaySamples = delaySamples + (int)(modulation * delaySamples / 2 * flangerFactor);
            int delayIndex = (delayBufferPosition + modulatedDelaySamples) % delaySamples;

            float delayedSample = delayBuffer[delayIndex];
            delayBuffer[delayBufferPosition] = input + delayedSample * feedback;
            buffer[offset + n] += delayedSample;

            delayBufferPosition++;
            if (delayBufferPosition == delaySamples)
            {
                delayBufferPosition = 0;
            }
        }

        return samplesRead;
    }

    public WaveFormat WaveFormat => source.WaveFormat;
}
