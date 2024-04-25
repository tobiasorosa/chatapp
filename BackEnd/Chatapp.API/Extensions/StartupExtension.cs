using Chatapp.API;
using Chatapp.Core.Mediatr;
using Chatapp.Core.OpenApi.Schemas;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Chatapp.Core.Extensions
{
    public static class StartupExtension
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chatapp", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
                c.SchemaFilter<NamespaceSchemaFilter>();
            });

            return services;
        }

        public static IApplicationBuilder BuilderSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp API v1");
            });

            return app;
        }

        public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IApplicationBuilder BuilderCompression(this IApplicationBuilder app)
        {
            return app.UseResponseCompression();
        }

        public static IApplicationBuilder BuilderRouting(this IApplicationBuilder app)
        {
            return app.UseRouting();
        }

        public static IApplicationBuilder BuilderCors(this IApplicationBuilder app, string corsName)
        {
            return app.UseCors(corsName);
        }

        public static IApplicationBuilder BuilderWebSockets(this IApplicationBuilder app, params string[] allowedOrigins)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            allowedOrigins.ToList().ForEach(origin =>
            {
                webSocketOptions.AllowedOrigins.Add(origin);
            });

            return app.UseWebSockets(webSocketOptions);
        }

        public static IApplicationBuilder BulderAuthentication(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseAuthorization();
        }

        public static IApplicationBuilder BuilderEndpoints(this IApplicationBuilder app)
        {
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, string corsName, params string[] allowedOrigins)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(corsName,
                builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
        }
    }
}
