using NAudio.Wave;
using System;

public class FlangerEffect : ISampleProvider
{
    private readonly ISampleProvider source;
    private readonly int delayMilliseconds;
    private readonly float feedback;
    private readonly int sampleRate;
    private readonly float modulationFrequency; 

    private int delaySamples;
    private float[] delayBuffer;
    private int delayBufferPosition;

    private float flangerFactor;

    public FlangerEffect(ISampleProvider source, int delayMilliseconds, float feedback, int sampleRate, float modulationFrequency)
    {
        //The audio souirce where the flanger effect will be applied
        this.source = source;
        //Desired delay time for the effect in ms
        this.delayMilliseconds = delayMilliseconds;
        //The feedback factor determines how much of the delayed signal is fed back into the audio
        this.feedback = feedback;
        //Sample rate per second
        this.sampleRate = sampleRate;
        //Low frequency Oscillator (LFO)
        this.modulationFrequency = modulationFrequency;

        //Calculates the number of samples corresponding to the desired delay time
        this.delaySamples = (int)((delayMilliseconds / 1000.0) * sampleRate);
        //Createa a buffer to store delayed samples based on the calculated delay samples
        this.delayBuffer = new float[delaySamples];
        //initialize the position in the delay buffer
        this.delayBufferPosition = 0;

        // Initialize flanger factor 
        this.flangerFactor = 0.5f; //default factor value

        /*
         * delayMilliseconde, feedback en modulationFrequency have direct influence on the flanger effect
         * sampleRate is for audio frequency
         */
    }

    // Method to adjust the flanger factor dynamically
    public void AdjustFlangerFactor(float factor)
    {
        // Ensure the factor is within the valid range (0.0 to 1.0)
        this.flangerFactor = Math.Max(0.0f, Math.Min(1.0f, factor));
    }

    public int Read(float[] buffer, int offset, int count)
    {
        //reads input audio
        int samplesRead = source.Read(buffer, offset, count);

        for (int n = 0; n < samplesRead; n++)
        {
            float input = buffer[offset + n];
            //calculate modulation value with sinus
            /*
             * Met de sinusfunctie kan er een golfvorm worden gegenereerd die varieert tussen -1 en 1. 
             * Dit is de modulatiesignaal om de vertraging van het flangereffect te variëren
             *
             * 2 * pi: zorgt ervoor dat de sinusgolf een volledige cyclus (360 graden) doorloopt.
             * modulationFrequency: de frequentie van de sinusgolf die de modulatie bepaalt.
             * n: huidige steekproef index in de for lus
             */
            float modulation = (float)Math.Sin(2 * Math.PI * modulationFrequency * n / sampleRate); 

            //calculates amount of delayed samples
            int modulatedDelaySamples = delaySamples + (int)(modulation * delaySamples / 2 * flangerFactor);
            //calculates index in delayBuffer
            int delayIndex = (delayBufferPosition + modulatedDelaySamples) % delaySamples;
            //get delayed sample
            float delayedSample = delayBuffer[delayIndex];
            //add delayed signal to original signal with feedback
            delayBuffer[delayBufferPosition] = input + delayedSample * feedback;
            //adds the de delayed signal to outputbuffer
            buffer[offset + n] += delayedSample;
            //updates de position of the delay buffer
            delayBufferPosition++;
            if (delayBufferPosition == delaySamples)
            {
                delayBufferPosition = 0;
            }
        }

        //returns amount of samples
        return samplesRead;
    }

    //Get method voor ophalen van audio, hoort bij de interface van ISampleProvider
    public WaveFormat WaveFormat => source.WaveFormat;
}
