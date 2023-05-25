﻿using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Plugin.AudioRecorder;
using Android.Media;


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
        double alertFreq = 1;

        [ObservableProperty]
        double noiseThreshold = 60;

        public MainViewModel()
        {
            audioRecorderService = new AudioRecorderService();
            audioRecorderService.AudioInputReceived += AudioInputReceived;
            maxDecibels = 0.0;
        }

        private void AudioInputReceived(object sender, string filePath)
        {
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
                audioRecorderService.StopRecording();
                audioPlayer.Play(audioRecorderService.GetAudioFilePath());
            }
            else
            {
                ButtonText = "Stop";
                Opacity = 0.0;
                audioRecorderService.StartRecording();
            }
        }
    }
}

