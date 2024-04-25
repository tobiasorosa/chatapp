using Chatapp.Core.Extensions;
using System.Reflection;

namespace Chatapp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private List<string> AllowedOrigins { get; }
        private static string CorsPolicyName => "DefaultCorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRepositories(Assembly.GetExecutingAssembly()); // Assuming you have the AddRepositories extension method
            services.ConfigureMediatr();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.BuilderSwagger();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app
            .BuilderCompression()
            .BuilderRouting()
            .BuilderCors(CorsPolicyName)
            .BuilderWebSockets(AllowedOrigins.ToArray())
            .BulderAuthentication()
            .BuilderEndpoints();
        }
    }
}
