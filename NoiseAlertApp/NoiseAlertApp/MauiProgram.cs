using Microsoft.Extensions.Logging;
using NoiseAlertApp.ViewModels;
using Syncfusion.Maui.Core.Hosting;
namespace NoiseAlertApp;

public static class MauiProgram
{
    public interface IServiceTest
    {
        void Start();
        void Stop();
    }
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<IServiceTest, DemoServices>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}