using System.Diagnostics;
using AudioMixingApp.Effects;
using NAudio.Wave;

namespace AudioMixingApp.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();

        string filePath = "C:/Users/Yanni/Downloads/@Pantera-Cowboys-from-Hell.mp3";

        AnalyzeLowFrequencies(filePath, 500);
    }
    
    static void AnalyzeLowFrequencies(string filePath, int lowFrequencyThreshold)
    {
        using (var reader = new AudioFileReader(filePath))
        {
            int sampleRate = reader.WaveFormat.SampleRate;
            int bufferSize = 1024; // You can adjust this based on your requirements

            float[] buffer = new float[bufferSize];
            int bytesRead;

            while ((bytesRead = reader.Read(buffer, 0, bufferSize)) > 0)
            {
                // Process the buffer and analyze the frequencies
                for (int i = 0; i < bytesRead; i++)
                {
                    // Convert the samples to frequencies using Fast Fourier Transform (FFT)
                    // Check if the frequency is below the specified threshold
                    float frequency = i * sampleRate / (float)bufferSize;
                    if (frequency < lowFrequencyThreshold)
                    {
                        // Do something with the low-frequency data
                        Console.WriteLine($"Low Frequency Sample: {buffer[i]}, Frequency: {frequency} Hz");
                    }
                }
            }
        }
    }

    
}