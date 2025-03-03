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
            // Add any cleanup or save state logic here
            RunPowerShellScript();
        }

    private void RunPowerShellScript()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", "restore_network_config.ps1");

                // Check if script exists
                if (!File.Exists(scriptPath))
                {
                    Console.WriteLine("PowerShell script not found: " + scriptPath);
                    return;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe", // On Windows, this should resolve correctly
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        Console.WriteLine("PowerShell Output: " + output);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("PowerShell Error: " + error);
                    }
                }
            }
            else
            {
                Console.WriteLine("PowerShell scripts can only run on Windows.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error executing PowerShell script: " + ex.Message);
        }
    }

}
}
