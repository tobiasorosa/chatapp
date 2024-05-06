using Microsoft.Extensions.Configuration;
using Serilog.Formatting.Compact;
using Serilog;

namespace Chatapp.Core.Serilog
{
    public static class Configuration
    {
        public static void WriteToDebug(this LoggerConfiguration loggerConfiguration, Type assemblyName, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var applicationName = assemblyName.Namespace;

            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(serviceProvider)
                .WriteTo
                .Debug(new RenderedCompactJsonFormatter())
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationContext", applicationName);
        }
    }
}
