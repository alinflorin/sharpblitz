using SharpBlitz.Runner.Abstractions;
using SharpBlitz.Runner.Roslyn;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Di
    {
        public static void AddRoslynRunner(this IServiceCollection services)
        {
            services.AddSingleton<IRunner, RoslynRunner>();
        }
    }
}