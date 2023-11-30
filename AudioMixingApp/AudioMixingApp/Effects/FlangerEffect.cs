using NAudio.Wave;
using System;

public class FlangerEffect : ISampleProvider
{
    private readonly ISampleProvider source;
    private readonly int delaySamples;
    private readonly float[] delayBuffer;
    private int delayBufferPosition;
    public float FlangerFactor;

    public FlangerEffect(ISampleProvider source, float flangerFactor)
    {
        //Input song
        this.source = source;

        //Adjustable factor to adjust intensivity of flanger. Min = 0.0, Max = 1.0
        this.FlangerFactor = Math.Max(0.0f, Math.Min(1.0f, flangerFactor));

        // Set delay time in milliseconds
        int delayMilliseconds = 20; 

        // Calculate delay samples based on the delay time and sample rate
        this.delaySamples = (int)((delayMilliseconds / 1000.0) * source.WaveFormat.SampleRate);

        // Create a buffer to store delayed samples based on the calculated delay samples
        this.delayBuffer = new float[delaySamples];

        // Initialize the position in the delay buffer
        this.delayBufferPosition = 0;
    }

    /// <summary>
    ///  Method that adjust the flanger factor. Adjusting flanger intensivity 
    /// </summary>
    /// <param name="factor"></param>
    public void AdjustFlangerFactor(float factor)
    {
        //valid range (0.0 to 1.0)
        this.FlangerFactor = Math.Max(0.0f, Math.Min(1.0f, factor));
    }

    /// <summary>
    ///  Reads and processes audio samples
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int Read(float[] buffer, int offset, int count)
    {
        // Reads input audio
        int samplesRead = source.Read(buffer, offset, count);

        for (int n = 0; n < samplesRead; n++)
        {
            float input = buffer[offset + n];
            // Calculate modulation value with sinus
            float modulation = (float)Math.Sin(2 * Math.PI * FlangerFactor * n / source.WaveFormat.SampleRate);

            // Calculates amount of delayed samples
            int modulatedDelaySamples = delaySamples + (int)(modulation * delaySamples / 2);
            // Calculates index in delayBuffer
            int delayIndex = (delayBufferPosition + modulatedDelaySamples) % delaySamples;
            // Get delayed sample
            float delayedSample = delayBuffer[delayIndex];
            // Add delayed signal to original signal
            buffer[offset + n] += delayedSample;
            // Updates the position of the delay buffer
            delayBuffer[delayBufferPosition] = input;

            delayBufferPosition = (delayBufferPosition + 1) % delaySamples;
        }

        // Returns amount of samples
        return samplesRead;
    }

    // Get method for retrieving audio, part of the ISampleProvider interface
    public WaveFormat WaveFormat => source.WaveFormat;
}
  