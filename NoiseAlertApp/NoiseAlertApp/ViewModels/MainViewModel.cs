using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Plugin.AudioRecorder;


namespace NoiseAlertApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService();

        private readonly AudioPlayer audioPlayer = new AudioPlayer();

        int clicked = 0;

        [ObservableProperty]
        double opacity = 0.9;

        [ObservableProperty]
        string buttonText = "Start";

        [ObservableProperty]
        double alertFreq = 1;

        [ObservableProperty]
        double noiseThreshold = 60;

        [RelayCommand]
        void Click()
        {
            clicked++;
            if (clicked % 2 == 0)
            {
                ButtonText = "Start";
                Opacity = 0.9;
                if (audioRecorderService.IsRecording)
                {
                    audioRecorderService.StopRecording();
                    audioPlayer.Play(audioRecorderService.GetAudioFilePath());
                }
            }
            else
            {
                ButtonText = "Stop";
                Opacity = 0.0;
                if (!audioRecorderService.IsRecording)
                {
                    audioRecorderService.StartRecording();
                }
            }
        }
    }
}

