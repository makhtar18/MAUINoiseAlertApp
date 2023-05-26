using NAudio.Wave;

public class MyAudioAnalyzer
{
    public double CalculateDecibels(string filePath)
    {
        using (var audioFile = new AudioFileReader(filePath))
        {
            Console.WriteLine("Calculating db");
            var buffer = new float[audioFile.WaveFormat.SampleRate * audioFile.WaveFormat.Channels];
            var maxDecibels = 0.0;

            while (audioFile.Read(buffer, 0, buffer.Length) > 0)
            {
                var sum = 0.0;

                // Calculate the root mean square (RMS) of the audio samples
                for (var i = 0; i < buffer.Length; i++)
                {
                    sum += buffer[i] * buffer[i];
                }

                var rms = Math.Sqrt(sum / buffer.Length);

                // Convert RMS to decibels using the formula: dB = 20 * log10(rms)
                var decibels = 20 * Math.Log10(rms) + 90;

                if (decibels > maxDecibels)
                {
                    maxDecibels = decibels;
                }
            }

            return maxDecibels;
        }
    }
}
