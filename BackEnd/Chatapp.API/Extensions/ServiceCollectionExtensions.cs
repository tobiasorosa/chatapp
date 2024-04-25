using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chatapp.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            // Get all public types from the assembly
            var types = assembly.GetExportedTypes();

            // Filter types that are repositories and their implementations
            var repositoryTypes = types.Where(t => t.IsInterface && t.Name.EndsWith("Repository"));
            var implementationTypes = types.Where(t => t.Name.EndsWith("Repository"));

            // Register each repository interface with its implementation
            foreach (var repositoryType in repositoryTypes)
            {
                var implementationType = implementationTypes.FirstOrDefault(t =>
                    repositoryType.IsAssignableFrom(t) && !t.IsInterface);

                if (implementationType != null)
                {
                    // Add repository interface with its implementation
                    services.AddSingleton(repositoryType, implementationType);
                }
                else
                {
                    throw new InvalidOperationException($"No implementation found for {repositoryType.Name}.");
                }
            }
        }
    }
}
