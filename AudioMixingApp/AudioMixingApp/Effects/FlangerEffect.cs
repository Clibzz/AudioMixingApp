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
            _song = source;
            _flangerFactor = flangerFactor;

            int flangerBufferLength = (int)(WaveFormat.SampleRate * 0.02); // Adjust the length of the flanger buffer for desired effect
            _flangerBuffer = new float[flangerBufferLength];
        }

        public void IncreaseFlanger()
        {
            _flangerFactor = (float)Math.Round(Math.Min(_flangerFactor + 0.1, 0.5), 1); // Maximum flanger factor set to 0.5
        }

        public void DecreaseFlanger()
        {
            _flangerFactor = (float)Math.Round(Math.Max(_flangerFactor - 0.1, 0.0), 1); //Minimum flanger factor set
        }

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
