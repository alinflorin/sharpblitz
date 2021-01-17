using System;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace SharpBlitz
{
    public class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder() =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
