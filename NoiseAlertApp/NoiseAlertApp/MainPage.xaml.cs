using NoiseAlertApp.ViewModels;

namespace NoiseAlertApp;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

}


