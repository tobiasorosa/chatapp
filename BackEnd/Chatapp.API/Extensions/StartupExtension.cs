using Chatapp.Core.Hubs;
using Chatapp.Core.Mediatr;
using Chatapp.Core.OpenApi.Schemas;
using Chatapp.Core.Web.Conventions;
using Chatapp.Core.WebSocket.Providers;
using MediatR;
using MessagePack;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.Internal;
using System;
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

        public static IApplicationBuilder BuilderEndpoints(this IApplicationBuilder app, string corsPolicyName)
        {
            return app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hubs/chat")
                    .RequireCors(corsPolicyName);

                endpoints.MapControllers()
                    .RequireCors(corsPolicyName);
            });
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, string corsName, params string[] allowedOrigins)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(corsName,
                builder =>
                {
                    builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
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

        public static IApplicationBuilder BuilderRouting(this IApplicationBuilder app)
        {
            return app.UseRouting();
        }

        public static IApplicationBuilder BuilderCors(this IApplicationBuilder app, string corsName)
        {
            return app.UseCors(corsName);
        }

        public static IApplicationBuilder BulderAuthentication(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseAuthorization();
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services
            .AddAuthorization()
            .AddAuthentication();

            return services;
        }

        public static IServiceCollection ConfigureSignalr(this IServiceCollection services)
        {
            services.AddConnections();

            services
                .AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.IgnoreNullValues = true;
                })
                .AddMessagePackProtocol(options =>
                {
                    options.SerializerOptions = MessagePackSerializerOptions.Standard
                                                                            .WithSecurity(MessagePackSecurity.UntrustedData);
                });

            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            return services;
        }

        public static IMvcBuilder ConfigureControllers(this IServiceCollection services)
        {
            return services
                .AddControllers()
                .AddControllersAsServices();
        }
    }
}
