using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using Xamarin.Essentials;
using Android.Media;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace NoiseAlertApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        int clicked = 0;

        [ObservableProperty]
        double maxDecibels;

        [ObservableProperty]
        double opacity = 0.9;

        [ObservableProperty]
        string buttonText = "Start";

        [ObservableProperty]
        int alertFreq = 1;

        [ObservableProperty]
        int noiseThreshold = 60;

        private const int SampleRate = 44100;
        private const ChannelIn ChannelConfig = ChannelIn.Mono;

        private AudioRecord audioRecord;
        private AudioTrack audioTrack;
        private bool isStreaming;

        private Microphone mic = new Microphone();

        public MainViewModel()
        {
            maxDecibels = 0.0;
            mic.RequestAsync();
        }

        [RelayCommand]
        void Click()
        {
            clicked++;
            if (clicked % 2 == 0)
            {
                ButtonText = "Start";
                Opacity = 0.9;
                StopStreaming();
            }
            else
            {
                ButtonText = "Stop";
                Opacity = 0.0;
                StartStreaming();
            }
        }

        public void StartStreaming()
        {
            int minBufferSize = AudioRecord.GetMinBufferSize(SampleRate, ChannelConfig, Encoding.Pcm16bit);
            audioRecord = new AudioRecord(AudioSource.Mic, SampleRate, ChannelConfig, Encoding.Pcm16bit, minBufferSize);

            int maxBufferSize = AudioTrack.GetMinBufferSize(SampleRate, ChannelOut.Stereo, Encoding.Pcm16bit);
            audioTrack = new AudioTrack(Android.Media.Stream.Music, SampleRate, ChannelOut.Stereo, Encoding.Pcm16bit, maxBufferSize, AudioTrackMode.Stream);

            isStreaming = true;
            audioRecord.StartRecording();
            audioTrack.Play();
            _ = ProcessAudioDataAsync();
        }

        public void StopStreaming()
        {
            isStreaming = false;
            audioRecord.Stop();
            audioTrack.Stop();
            audioRecord.Release();
            audioTrack.Release();
        }

        private async Task ProcessAudioDataAsync()
        {
            byte[] buffer = new byte[4096];

            while (isStreaming)
            {
                int bytesRead = await audioRecord.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    double decibels = CalculateDecibels(buffer, bytesRead);

                    MaxDecibels = decibels;
                }

                audioTrack.Write(buffer, 0, bytesRead);
            }
        }

        private double CalculateDecibels(byte[] audioData, int length)
        {
            double sum = 0;
            for (int i = 0; i < length; i += 2)
            {
                short sample = BitConverter.ToInt16(audioData, i);
                sum += sample * sample;
            }
            double rms = Math.Sqrt(sum / (length / 2));
            double decibels = 20 * Math.Log10(rms) ;

            return decibels;
        }

    }
}

