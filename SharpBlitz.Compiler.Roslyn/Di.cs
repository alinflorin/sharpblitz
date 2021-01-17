using SharpBlitz.Compiler.Abstractions;
using SharpBlitz.Compiler.Roslyn;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Di
    {
        public static void AddRoslynCompiler(this IServiceCollection services)
        {
            services.AddSingleton<ICompiler, RoslynCompiler>();
        }
    }
}
