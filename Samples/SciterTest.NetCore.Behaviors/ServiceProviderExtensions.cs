using System;
using SciterCore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider ConfigureHost<THost>(this IServiceProvider provider, Action<IServiceProvider, SciterHost> action)
        where THost : SciterHost
        {
            action.Invoke(provider, provider.GetService<THost>());
            return provider;
        }
    }
}