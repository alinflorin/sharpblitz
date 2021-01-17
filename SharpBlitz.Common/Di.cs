using SharpBlitz.Common;
using SharpBlitz.Common.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Di
    {
        public static void AddCommon(this IServiceCollection services)
        {
            services.AddSingleton<CachingHttpClient>();
            services.AddSingleton<IdeStore>();
        }
    }
}