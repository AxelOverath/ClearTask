using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ClearTask.Data;
using CommunityToolkit.Maui;
using Maui.NullableDateTimePicker;

namespace ClearTask
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit()
                .ConfigureNullableDateTimePicker();
            // Load appsettings.json as an embedded resource
            var assembly = Assembly.GetExecutingAssembly();
            var configStream = assembly.GetManifestResourceStream("ClearTask.appsettings.json");

            
            
                var configuration = new ConfigurationBuilder()
                    .AddJsonStream(configStream)
                    .Build();

                // Load configuration into static helper
                AppConfig.LoadConfiguration(configuration);
            



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
