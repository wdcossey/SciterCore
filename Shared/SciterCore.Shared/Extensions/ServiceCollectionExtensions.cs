#if NETCORE

using System;
using System.Linq;
using System.Reflection;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Extensions;
using SciterCore.Helpers;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region Private methods

        private static IServiceCollection RegisterHostHandler<THost>(this IServiceCollection serviceCollection,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THost : SciterHost
        {
            var hostEventHandler =
                typeof(THost).GetCustomAttributes<SciterHostEventHandlerAttribute>().FirstOrDefault();

            if (hostEventHandler != null)
                serviceCollection.Add(
                    ServiceDescriptor.Describe(hostEventHandler.Type, hostEventHandler.Type, ServiceLifetime.Singleton));

            return serviceCollection;
        }
        
        /*private static IServiceCollection RegisterBehaviorHandlers<THost>(this IServiceCollection serviceCollection,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THost : SciterHost
        {
            var behaviorEventHandlers =
                typeof(THost).GetCustomAttributes<SciterHostBehaviorHandlerAttribute>();

            foreach (var behaviorEventHandler in behaviorEventHandlers)
            {
                serviceCollection.Add(
                    ServiceDescriptor.Describe(behaviorEventHandler.Type, behaviorEventHandler.Type,
                        ServiceLifetime.Singleton));
            }
            
            return serviceCollection;
        }*/

        private static IServiceCollection RegisterHostWindow<THost>(this IServiceCollection serviceCollection,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THost : SciterHost
        {
            var hostWindow =
                typeof(THost).GetCustomAttributes<SciterHostWindowAttribute>().FirstOrDefault();

            if (hostWindow != null)
                serviceCollection.Add(
                    ServiceDescriptor.Describe(hostWindow.Type, hostWindow.Type, ServiceLifetime.Singleton));

            return serviceCollection;
        }

        #endregion

        #region Public methods

        public static IServiceCollection AddSciterHost<THost>(this IServiceCollection serviceCollection,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where THost : SciterHost
        {
            serviceCollection
                .AddSingleton<ISciterWindowResolver, SciterWindowResolver>()
                .RegisterHostHandler<THost>(lifetime)
                //.RegisterBehaviorHandlers<THost>(lifetime)
                .RegisterHostWindow<THost>(lifetime)
                .Add(ServiceDescriptor.Describe(typeof(SciterHost), provider =>
                {
                    var instance = ActivatorUtilities.CreateInstance<THost>(provider) as SciterHost;
                    
                    var hostEventHandler =
                        typeof(THost).GetCustomAttributes<SciterHostEventHandlerAttribute>().FirstOrDefault();
                    
                    var behaviorHandlers =
                        typeof(THost).GetCustomAttributes<SciterHostBehaviorHandlerAttribute>();
                    
                    var hostWindow =
                        typeof(THost).GetCustomAttributes<SciterHostWindowAttribute>().FirstOrDefault();

                    if (hostWindow != null)
                    {
                        var sciterWindow = provider.GetRequiredService(hostWindow.Type);

                        if (sciterWindow.GetType() == typeof(SciterWindow))
                        {
                            (sciterWindow as SciterWindow)
                                ?.CreateMainWindow(hostWindow.Width ?? 800, hostWindow.Height ?? 600)
                                ?.SetTitle(hostWindow.Title ?? "SciterCore");
                        }
                            
                        instance?.SetupWindow(sciterWindow as SciterWindow);
                        
                        //if (hostWindow.Height.HasValue && hostWindow.Width.HasValue)
                        //    instance?.Window?.SetSize()
                    }

                    if (hostEventHandler != null)
                        instance?.AttachEventHandler(provider.GetRequiredService(hostEventHandler.Type) as SciterEventHandler);

                    foreach (var behaviorHandler in behaviorHandlers)
                        instance.RegisterBehaviorHandler(behaviorHandler.Type);

                    instance?.OnCreated?.Invoke(instance, new WindowCreatedEventArgs(instance.Window));

                    if (!string.IsNullOrWhiteSpace(hostWindow?.HomePage))
                        instance?.Window.TryLoadPage(uri: new Uri(hostWindow.HomePage));

                    return instance;

                }, lifetime));
                //.Add(ServiceDescriptor.Describe(typeof(SciterHost),typeof(THost), lifetime));

            return serviceCollection;
        }

        #endregion
    }
}

#endif