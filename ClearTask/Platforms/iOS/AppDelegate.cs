using Foundation;
using UIKit;

namespace ClearTask
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void WillTerminate(UIApplication uiApplication)
        {
            base.WillTerminate(uiApplication);

            if (Microsoft.Maui.Controls.Application.Current is App app)
            {
                app.OnStop();
            }
        }

    }


}
