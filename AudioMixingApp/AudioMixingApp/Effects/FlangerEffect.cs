using NAudio.Wave;

namespace AudioMixingApp.Effects
{
    class FlangerEffect : ISampleProvider
    {
        //The song that gets imported 
        private ISampleProvider _song { get; set; }

        private float[] _flangerBuffer { get; set; }
        private int _flangerBufferPosition { get; set; }
        private float _flangerFactor { get; set; }
        public WaveFormat WaveFormat { get { return _song.WaveFormat; } }
       
        public FlangerEffect(ISampleProvider source, float flangerFactor)
        {
            //The song
            _song = source;
            //Factor to change the flanger effect
            _flangerFactor = flangerFactor;
            //Adjust the length of the Flanger buffer 
            int flangerBufferLength = (int)(WaveFormat.SampleRate * 0.02); 
            _flangerBuffer = new float[flangerBufferLength];

            _flangerBufferPosition = 0;
        }
        /// <summary>
        /// Method IncreaseFlanger() increases the flanger effect.
        /// The maximum is 0.5
        /// </summary>
        public void IncreaseFlanger()
        {
            _flangerFactor = (float)Math.Round(Math.Min(_flangerFactor + 0.1, 0.5), 1); 
        }

        /// <summary>
        /// Method DecreaseFlanger(): decreases the flanger effect by changing the factor
        /// </summary>
        public void DecreaseFlanger()
        {
            _flangerFactor = (float)Math.Round(Math.Max(_flangerFactor - 0.1, 0.0), 1); 
        }

        /// <summary>
        /// This function reads the song 
        /// </summary>
        /// <param name="buffer">The song that is going to be played</param>
        /// <param name="offset">The position from where the samples in the buffer will be read</param>
        /// <param name="count">The amount of audio samples that will be read</param>
        /// <returns>The amount of read samples</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int readSamples = _song.Read(buffer, offset, count);
           
            for (int i = 0; i < readSamples; i++)
            {
                buffer[i] += _flangerBuffer[_flangerBufferPosition] * _flangerFactor;

                _flangerBuffer[_flangerBufferPosition] = buffer[i] + _flangerBuffer[_flangerBufferPosition] * 0.5f;
                _flangerBufferPosition++;

                if (_flangerBufferPosition == _flangerBuffer.Length)
                    _flangerBufferPosition = 0;
            }
            return readSamples;
        }
    }
}
