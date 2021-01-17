using SharpBlitz.CodeAnalysis.Abstractions;
using SharpBlitz.CodeAnalysis.Roslyn;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Di
    {
        public static void AddRoslynAnalyzer(this IServiceCollection services)
        {
            services.AddSingleton<ICodeAnalyzer, RoslynCodeAnalyzer>();
        }
    }
}
