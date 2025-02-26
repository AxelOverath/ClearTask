using Microsoft.Extensions.Configuration;

namespace ClearTask.Data;

public static class AppConfig
{
    private static IConfiguration _configuration;

    public static string ApiUri => _configuration?["ApiUri"];
   
    public static void LoadConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}
