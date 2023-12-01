using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace AudioMixingApp.Effects
{
    public class PitchshiftEffect : ISampleProvider
    {
        // The song
        private readonly ISampleProvider _song;
        // The pitch value. 0.5f is one octave down, 1.0f is the normal pitch and 2.0f is an octave up.
        public float PitchValue { get; set; }
        // The pitchshifter. Source: https://github.com/naudio/NAudio/blob/master/Docs/SmbPitchShiftingSampleProvider.md
        private readonly SmbPitchShiftingSampleProvider _pitchShiftingSampleProvider;
        // The waveformat of the song. Contains information like the sample rate of the song, number of bits per sample, number of channels and the audio format.
        public WaveFormat WaveFormat { get { return _song.WaveFormat; } }

        public PitchshiftEffect(ISampleProvider song, float pitchValue)
        {
            _song = song;
            PitchValue = pitchValue;
            // Create a new instance of the SmbPitchShiftingSampleProvider and set the PitchFactor to the current pitchValue, which is 1.0f.
            _pitchShiftingSampleProvider = new (_song)
            {
                PitchFactor = pitchValue
            };
        }

        /// <summary>
        /// Change the pitch of the song.
        /// </summary>
        /// <param name="pitchValue">the new pitch of the song.</param>
        public void ChangePitchValue(float pitchValue)
        {
            _pitchShiftingSampleProvider.PitchFactor = pitchValue;
            PitchValue = pitchValue;
        }

        /// <summary>
        /// This function reads a specified amount of samples from the song and applies the pitch to the song.
        /// </summary>
        /// <param name="buffer">The samples from the song</param>
        /// <param name="offset">The position from where the samples in the buffer will be read</param>
        /// <param name="count">The amount of audio samples that will be read</param>
        /// <returns>The amount of read samples</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            // Apply the new pitch to the current playing songs samples.
            return _pitchShiftingSampleProvider.Read(buffer, offset, count);
        }
    }
}