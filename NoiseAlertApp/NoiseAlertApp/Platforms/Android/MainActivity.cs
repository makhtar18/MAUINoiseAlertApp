using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using static NoiseAlertApp.MauiProgram;

namespace NoiseAlertApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    // private Context mContext;
    public static MainActivity ActivityCurrent { get; set; }
    public MainActivity()
    {

        ActivityCurrent = this;

    }

}
[Service]
public class DemoServices : Service, IServiceTest //we implement our service (IServiceTest) and use Android Native Service Class
{
    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]//we catch the actions intents to know the state of the foreground service
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        if (intent.Action == "START_SERVICE")
        {
            RegisterNotification();//Proceed to notify
        }
        else if (intent.Action == "STOP_SERVICE")
        {
            StopForeground(true);//Stop the service
            StopSelfResult(startId);
        }

        return StartCommandResult.NotSticky;
    }

    //Start and Stop Intents, set the actions for the MainActivity to get the state of the foreground service
    //Setting one action to start and one action to stop the foreground service
    public void Start()
    {
        //Console.WriteLine("Started");
        Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(DemoServices));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        //Console.WriteLine("Stopped");
        Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
        stopIntent.SetAction("STOP_SERVICE");
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    public void RegisterNotification()
    {
        NotificationChannel channel = new NotificationChannel("ServiceChannel", "ServiceDemo", NotificationImportance.Max);
        NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
        manager.CreateNotificationChannel(channel);
        Notification notification = new Notification.Builder(this, "ServiceChannel")
           .SetContentTitle("Running in background")
           .SetOngoing(true)
           .Build();

        StartForeground(100, notification);
    }
}

