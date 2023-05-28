using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using Xamarin.Essentials;
using Android.Media;
using Plugin.LocalNotification;
using static Microsoft.Maui.ApplicationModel.Permissions;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Plugin.LocalNotification.AndroidOption;
using Permissions = Microsoft.Maui.ApplicationModel.Permissions;
using PermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus;
using Android.App;

namespace NoiseAlertApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        MauiProgram.IServiceTest Services;

        int clicked = 0;

        [ObservableProperty]
        double decibels;

        [ObservableProperty]
        double opacity = 0.9;

        [ObservableProperty]
        string buttonText = "Start";

        [ObservableProperty]
        bool isCrirical;

        [ObservableProperty]
        int alertFreq = 1;

        [ObservableProperty]
        int noiseThreshold = 60;

        private const int SampleRate = 44100;
        private const ChannelIn ChannelConfig = ChannelIn.Mono;


        private AudioRecord audioRecord;
        private AudioTrack audioTrack;

        private bool isStreaming;

        System.Timers.Timer timer = new System.Timers.Timer();
        double lastDecibels = 0.0;

        NotificationRequest fine = new NotificationRequest
        {
            NotificationId = 1337,
            Title = "Below Threshold",
            Silent = true
        };


        NotificationRequest tooLoud = new NotificationRequest
        {
            NotificationId = 1338,
            Title = "Heavy Noise Alert!",
            Description = "Please wear protective gear!"
        };
        private Microphone mic = new Microphone();

        public MainViewModel(MauiProgram.IServiceTest Services_)
        {
            Decibels = 0.0;
            Services = Services_;
            isCrirical = false;
        }

        async Task GetNotificationPermission()
        {
            var status = await Permissions.CheckStatusAsync<NotificationPermission>();
            if (!status.Equals(PermissionStatus.Granted))
            {
                await Permissions.RequestAsync<NotificationPermission>();
            }
        }

        [RelayCommand]
        async void Click()
        {
            await mic.RequestAsync();
            await GetNotificationPermission();
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
        void HandleTimer()
        {
            LocalNotificationCenter.Current.ClearAll();
            Console.WriteLine("Interval Called");
            if (lastDecibels > NoiseThreshold)
            {
                tooLoud.Title = "Heavy Noise Alert: "+Decibels+" dB! Please wear protective Gear!";
                if (IsCrirical)
                {
                    tooLoud.Android = new AndroidOptions
                    {
                        Priority = AndroidPriority.Max
                    };
                }
                LocalNotificationCenter.Current.Show(tooLoud);
            }
            else
            {
                fine.Title = "Below Threshold " + Decibels + " dB";
                LocalNotificationCenter.Current.Show(fine);

            }

        }
        public void StartStreaming()
        {
            Services.Start();
            Decibels = 0.0;
            int minBufferSize = AudioRecord.GetMinBufferSize(SampleRate, ChannelConfig, Encoding.Pcm16bit);
            audioRecord = new AudioRecord(AudioSource.Mic, SampleRate, ChannelConfig, Encoding.Pcm16bit, minBufferSize);

            int maxBufferSize = AudioTrack.GetMinBufferSize(SampleRate, ChannelOut.Stereo, Encoding.Pcm16bit);
            audioTrack = new AudioTrack(Android.Media.Stream.Music, SampleRate, ChannelOut.Stereo, Encoding.Pcm16bit, maxBufferSize, AudioTrackMode.Stream);
            timer.Interval = AlertFreq * 1000;
            timer.Elapsed += (sender, e) => HandleTimer();
            timer.Start();
            isStreaming = true;
            audioRecord.StartRecording();
            //audioTrack.Play();
            _ = ProcessAudioDataAsync();
        }

        public void StopStreaming()
        {
            Services.Stop();
            timer.Stop();
            isStreaming = false;
            audioRecord.Stop();
            audioTrack.Stop();
            audioRecord.Release();
            audioTrack.Release();
            Decibels = 0.0;
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

                    Decibels = Math.Round(decibels, 2);
                    lastDecibels = decibels;
                }

                audioTrack.Write(buffer, 0, bytesRead);
                //timer check

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
            double decibels = 0.0;
            if (sum != 0.0)
            {
                double rms = Math.Sqrt(sum / (length / 2));
                decibels = 20 * Math.Log10(rms);
            }

            return decibels;
        }

    }
}

