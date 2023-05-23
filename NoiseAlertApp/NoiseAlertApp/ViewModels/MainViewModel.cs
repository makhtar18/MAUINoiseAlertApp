using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;

namespace NoiseAlertApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        public MainViewModel()
        {
            
        }

        [ObservableProperty]
        double alertFreq = 1;

        [ObservableProperty]
        double noiseThreshold = 60;

        [RelayCommand]
        void OnClick()
        {
            
        }

    }
}

