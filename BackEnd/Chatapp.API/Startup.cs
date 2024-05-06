using Chatapp.Core.Extensions;

namespace Chatapp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            AllowedOrigins = Configuration["Origins"]?.Split(";")?.ToList() ?? new List<string>();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private List<string> AllowedOrigins { get; }
        private static string CorsPolicyName => "DefaultCorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureMediatr()
                .ConfigureSwagger()
                .ConfigureAuthorization()
                .ConfigureSignalr()
                .ConfigureCors(CorsPolicyName, AllowedOrigins.ToArray())
                .ConfigureControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
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
            .BuilderRouting()
            .BuilderCors(CorsPolicyName)
            .BuilderWebSockets(AllowedOrigins.ToArray())
            .BulderAuthentication()
            .BuilderEndpoints(CorsPolicyName);
        }
    }
}
