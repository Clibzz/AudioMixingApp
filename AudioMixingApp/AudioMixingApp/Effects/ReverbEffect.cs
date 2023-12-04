using NAudio.Wave;

namespace AudioMixingApp.Effects
{
    public class ReverbEffect : ISampleProvider
    {
        // The song.
        private ISampleProvider _song { get; set; }
        // The buffer that contains reverbed audio samples. Increasing this will increase the echo effect. It's like increasing the size of a room.
        private float[] _reverbBuffer { get; set; }
        // The index in the reverb buffer.
        private int _reverbBufferPosition { get; set; }
        // The factor of reverb that will be applied to the song. 
        public float ReverbFactor { get; set; }
        // The waveformat of the song. Contains information like the sample rate of the song, number of bits per sample, number of channels and the audio format.
        public WaveFormat WaveFormat { get { return _song.WaveFormat; } }

        public ReverbEffect(ISampleProvider source, float reverbFactor)
        {
            _song = source;
            ReverbFactor = reverbFactor;

            // Set the delaybuffer length to the samplerate / 10. The size will be 4800.
            int delayBufferLength = (int)(WaveFormat.SampleRate * 0.1);
            _reverbBuffer = new float[delayBufferLength];
        }

        /// <summary>
        /// This function reads a specified amount of samples from the song and applies reverb to the samples.
        /// </summary>
        /// <param name="buffer">The samples from the song</param>
        /// <param name="offset">The position from where the samples in the buffer will be read</param>
        /// <param name="count">The amount of audio samples that will be read</param>
        /// <returns>The amount of read samples</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            // Gets the samples from the song.
            int readSamples = _song.Read(buffer, offset, count);
            for (int i = 0; i < readSamples; i++)
            {
                // Adds the reverbed sample to a sample in the buffer.
                buffer[i] += _reverbBuffer[_reverbBufferPosition] * ReverbFactor;

                // Apply feedback to the delay buffer. The delay buffer is a circular buffer. Because of this the echo effect is created.
                _reverbBuffer[_reverbBufferPosition] = buffer[i] + _reverbBuffer[_reverbBufferPosition] * 0.5f;
                _reverbBufferPosition++;
                // Start at index 0 in the buffer when the final index is reached. The buffer starting over at position 0 when the end is reached is why the echo effect can be created.
                if (_reverbBufferPosition == _reverbBuffer.Length)
                    _reverbBufferPosition = 0;
            }
            return readSamples;
        }
    }
}
