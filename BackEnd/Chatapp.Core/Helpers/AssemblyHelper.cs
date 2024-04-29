using System.Reflection;

namespace Chatapp.Core.Helpers
{
    public static class AssemblyHelper
    {
        public static IEnumerable<T> GetInstancesFromAssembly<T>(this Assembly assembly, Type obj)
        {
            return assembly.GetTypes()
                    .Where(p => obj.IsAssignableFrom(p) && !p.IsInterface)
                    .Select(typeItem => (T)Activator.CreateInstance(typeItem));
        }
    }
}
