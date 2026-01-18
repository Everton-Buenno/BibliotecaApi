namespace BibliotecaApi.Infrastructure.Data;

public static class ConfigurationHelper
{
    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
}