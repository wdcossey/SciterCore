using System;
using System.Threading.Tasks;
using SciterCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void RunSciter(this IServiceProvider provider)
        {
            var app = provider.GetRequiredService<SciterApplication>();
            app.Run();
        }

        public static Task RunSciterAsync(this IServiceProvider provider)
        {
            var app = provider.GetRequiredService<SciterApplication>();
            return app.RunAsync();
        }
    }
}