using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Events;
using Serilog;
using Serilog.Formatting.Compact;
using Chatapp.Core.Web.Extensions;

namespace Chatapp.Core.Serilog
{
    public static class Logger
    {
        public static int Bootstrap(Action action, LogEventLevel minimunLogEventLevel = LogEventLevel.Warning)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.Debug(new RenderedCompactJsonFormatter())
            .CreateBootstrapLogger();

            try
            {
                action();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static void EnrichFromHttpRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            // Set all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available. You're not sending sensitive data in a querystring right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            var requestBody = httpContext.ReadRequestBodyAsync().Result;

            diagnosticContext.Set("RequestBody", requestBody);

            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint is object)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }

        public static LogEventLevel GetLeveByAttribute<T>(HttpContext httpContext, double _, Exception ex) where T : ActionFilterAttribute
        {
            if (ex != null || httpContext.Response.StatusCode > 499)
            {
                return LogEventLevel.Error;
            }

            var endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;

            return endpoint?.Metadata?.GetMetadata<T>() is null ?
                LogEventLevel.Debug :
                LogEventLevel.Information;
        }
    }
}
