using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.AudioRecorder;
using System.Threading;


namespace NoiseAlertApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private AudioRecorderService audioRecorderService;

        private AudioPlayer audioPlayer = new AudioPlayer();

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

        private bool isRecording = false;

        private CancellationTokenSource cancellationTokenSource;

        public MainViewModel()
        {
            audioRecorderService = new AudioRecorderService();
            //audioRecorderService.AudioInputReceived += AudioInputReceived;
            maxDecibels = 0.0;
        }

        private void AudioInputReceived(string filePath)
        {
            Console.WriteLine("EventHandler ", filePath);
            var audioAnalyzer = new MyAudioAnalyzer();
            var decibels = audioAnalyzer.CalculateDecibels(filePath);
            MaxDecibels = decibels;
        }

        [RelayCommand]
        void Click()
        {
            clicked++;
            if (clicked % 2 == 0)
            {
                ButtonText = "Start";
                Opacity = 0.9;
                isRecording = false;
                //audioRecorderService.StopRecording();
                //audioPlayer.Play(audioRecorderService.GetAudioFilePath());
            }
            else
            {
                ButtonText = "Stop";
                Opacity = 0.0;
                isRecording = true;
                ContinuousRecording();
                //audioRecorderService.StartRecording();

            }
        }

        private async Task ContinuousRecording()
        {
            cancellationTokenSource = new CancellationTokenSource();

            while (isRecording)
            {
                // Execute your task here
                if (audioRecorderService.IsRecording)
                {
                    Console.WriteLine("Stopping Recording: " + DateTime.Now);
                    await audioRecorderService.StopRecording();
                    AudioInputReceived(audioRecorderService.GetAudioFilePath());
                    Console.WriteLine("Starting Recording alone: " + DateTime.Now);
                    await audioRecorderService.StartRecording();
                }
                else
                {
                    Console.WriteLine("Starting Recording: " + DateTime.Now);
                    await audioRecorderService.StartRecording();
                }

                // Delay for 10 seconds
                Console.WriteLine("Starting Delay: " + DateTime.Now + " " + audioRecorderService.IsRecording);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(AlertFreq), cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // Task was canceled, exit the loop
                    break;
                }

                Console.WriteLine("Stopping Delay: " + DateTime.Now + " " + audioRecorderService.IsRecording);
            }
        }

    }
}

