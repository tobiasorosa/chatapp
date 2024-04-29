using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Chatapp.Repository.Context
{
    public sealed class ChatappContextFactory : IDesignTimeDbContextFactory<ChatappContext>
    {
        private readonly string _connectionString;
        public ChatappContextFactory()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            var configuration = new ConfigurationBuilder()
             .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Chatapp.API"))
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{environment}.json", optional: true)
             .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public ChatappContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatappContext>();

            var migrationsAssembly = typeof(ChatappContext).GetTypeInfo().Assembly.GetName().Name;

            optionsBuilder.UseNpgsql(_connectionString, options =>
            {
                options.MaxBatchSize(1000);
                options.EnableRetryOnFailure();
                options.MigrationsAssembly(migrationsAssembly);
                options.CommandTimeout(120);
            });

            optionsBuilder.EnableSensitiveDataLogging();

            return new ChatappContext(optionsBuilder.Options);
        }
    }
}
