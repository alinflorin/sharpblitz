using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharpBlitz.Common.Config;

namespace SharpBlitz
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Config
            {
            });
            services.AddCommon();
            services.AddRoslynAnalyzer();
            services.AddRoslynCompiler();
            services.AddRoslynRunner();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            var cfg = app.Services.GetRequiredService<Config>();
            cfg.BaseUrl = app.Services.GetRequiredService<NavigationManager>().BaseUri;
            SharpBlitzStatic.Init(app.Services);
        }
    }
}
