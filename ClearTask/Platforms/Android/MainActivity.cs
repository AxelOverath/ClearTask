using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Controls;

namespace ClearTask
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Access your App class directly
            if (Microsoft.Maui.Controls.Application.Current is App app)
            {
                app.OnStop();
            }
        }


    }
}
