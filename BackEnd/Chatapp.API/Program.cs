using System.Globalization;
using Chatapp.API;
using Chatapp.Core.Serilog;
using Serilog;

namespace Chatapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Bootstrap(() =>
            {
                CreateWebHostBuilder(args).Build().Run();
            });
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) =>
            {

                configuration.WriteToDebug(typeof(Program), context.Configuration, services);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
            })
            .UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateOnBuild = false;
                options.ValidateScopes = false;
            });
    }
}
