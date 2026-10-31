using Serilog;

namespace API.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(this ConfigureHostBuilder host)
        {
            host.UseSerilog((context, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console() 
                    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
            });
        }
    }
}
