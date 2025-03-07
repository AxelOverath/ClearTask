using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ClearTask
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        public void OnStop()
        {
            Console.WriteLine("App is stopping...");
        }

    }
}
