using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Enums;
using SciterCore.Extensions;
using SciterCore.Internal;
using SciterCore.Interop;
using SciterCore.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region Private methods

        private static IServiceCollection AddHostEventHandler<THost>(this IServiceCollection services)
            where THost : SciterHost
        {
            var hostEventHandlerAttribute =
                typeof(THost).GetCustomAttributes<SciterHostEventHandlerAttribute>().FirstOrDefault();

            if (hostEventHandlerAttribute == null)
                return services;

            if (HostEventHandlerRegistry.Instance.TryAdd(typeof(THost),
                hostEventHandlerAttribute.EventHandlerType))
            {
                services.Add(ServiceDescriptor.Describe(hostEventHandlerAttribute.EventHandlerType,
                    hostEventHandlerAttribute.EventHandlerType, ServiceLifetime.Transient));
            }

            return services;
        }

        private static IServiceCollection AddHostEventHandler<THost, TPrimaryEventHandler>(this IServiceCollection services)
            where THost : SciterHost
            where TPrimaryEventHandler : SciterEventHandler
        {
            if (HostEventHandlerRegistry.Instance.TryAdd(typeof(THost), typeof(TPrimaryEventHandler)))
            {
                //services.Replace(ServiceDescriptor.Describe(typeof()))
                services.Add(ServiceDescriptor.Describe(typeof(TPrimaryEventHandler), typeof(TPrimaryEventHandler), ServiceLifetime.Transient));
            }
            return services;
        }

        private static IServiceCollection AddHostWindow<THost>(this IServiceCollection services)
            where THost : SciterHost
        {
            var hostWindowAttribute =
                typeof(THost).GetCustomAttributes<SciterHostWindowAttribute>().FirstOrDefault();

            if (hostWindowAttribute?.WindowType != null)
                services.AddTransient(hostWindowAttribute.WindowType);

            HostWindowRegistry.Instance.TryAdd(typeof(THost), hostWindowAttribute?.WindowType ?? typeof(SciterWindow));

            services.AddTransient<SciterWindow>(provider =>
            {
                var result = ActivatorUtilities.CreateInstance(provider, hostWindowAttribute?.WindowType ?? typeof(SciterWindow)) as
                        SciterWindow;

                result
                    ?.CreateMainWindow(hostWindowAttribute?.Width ?? 800, hostWindowAttribute?.Height ?? 600)
                    ?.SetTitle(hostWindowAttribute?.Title ?? "SciterCore");


                return result;
            });

            return services;
        }

        #endregion

        #region Public methods

        #region AddSciterBehavior

        public static IServiceCollection AddSciterBehavior<THandler>(this IServiceCollection services)
            where THandler : SciterEventHandler
        {
            var behaviourName = typeof(THandler).GetBehaviourName();

            NamedBehaviorRegistry.Instance.TryAdd(behaviourName, typeof(THandler));
            //BehaviorRegistry.Instance.Add(behaviourName, typeof(THandler));

            services.Add(ServiceDescriptor.Describe(serviceType:typeof(THandler), implementationType: typeof(THandler), lifetime: ServiceLifetime.Transient));
            return services;
        }

        public static IServiceCollection AddSciterBehavior<THandler>(this IServiceCollection services, Func<IServiceProvider, THandler> implementationFactory)
            where THandler : SciterEventHandler
        {
            var behaviourName = typeof(THandler).GetBehaviourName();

            NamedBehaviorRegistry.Instance.TryAdd(behaviourName, typeof(THandler));
            //BehaviorRegistry.Instance.Add(behaviourName, typeof(THandler));

            services.Add(ServiceDescriptor.Describe(serviceType: typeof(THandler), implementationFactory: implementationFactory, lifetime: ServiceLifetime.Transient));
            return services;
        }

        #endregion

        #region AddSciterArchives...

        public static IServiceCollection AddSciterArchivesFromAssembly(this IServiceCollection services,
            Assembly assembly)
        {
            var attributes = assembly?.GetCustomAttributes<SciterCoreArchiveAttribute>()?.ToList();

            if (attributes?.Any() == true)
                foreach (var attribute in attributes)
                {
                    services.Add(ServiceDescriptor.Describe(
                        serviceType: typeof(LazySciterArchive),
                        implementationFactory: provider => new LazySciterArchive(attribute.Uri, assembly,
                            attribute.ResourceName, attribute.InitScripts), lifetime: ServiceLifetime.Singleton));
                }

            return services;
        }

        public static IServiceCollection AddSciterArchivesFromAssemblyWithType(this IServiceCollection services, Type type)
        {
            return services.AddSciterArchivesFromAssembly(type?.Assembly);
        }

        #endregion

        #region AddSciter

        public static IServiceCollection AddSciter<TPrimaryHost>(
            this IServiceCollection services,
            Action<SciterHostOptions> hostOptions = null,
            Action<SciterWindowOptions> windowOptions = null)
            where TPrimaryHost : SciterHost
        {
            services
                //.AddSingleton<HostEventHandlerRegistry>((provider) => HostEventHandlerRegistry.Instance)
                .AddSingleton<IHostWindowResolver, HostWindowResolver>()
                .AddSingleton<INamedBehaviorResolver, NamedBehaviorResolver>()
                .AddHostWindow<TPrimaryHost>()
                .AddHostEventHandler<TPrimaryHost>();

            services.AddTransient<TPrimaryHost>(
                provider =>
                {
                    using var scope = provider.CreateScope();
                    var scopedServiceProvider = scope.ServiceProvider;

                    var sciterHostOptions = new SciterHostOptions(scopedServiceProvider);
                    var sciterWindowOptions = new SciterWindowOptions(scopedServiceProvider);
                    hostOptions?.Invoke(sciterHostOptions);

                    /*THost result;

                    if (typeof(THost).Implements<SciterArchiveHost>())
                    {
                        var archiveAttribute = typeof(THost)
                            .GetCustomAttributes<SciterHostArchiveAttribute>(inherit: true).FirstOrDefault();

                        result = ActivatorUtilities.CreateInstance<THost>(provider);

                        //Archive = new SciterArchive(archiveAttribute?.BaseUrl ?? SciterArchive.DEFAULT_ARCHIVE_URI)
                        //    .Open();
                    }
                    else
                    {
                        result = ActivatorUtilities.CreateInstance<THost>(provider);
                    }*/

                    if (sciterHostOptions.ArchiveUri != null) { }

                    var result = ActivatorUtilities.CreateInstance<TPrimaryHost>(scopedServiceProvider);

                    var archives = scopedServiceProvider.GetServices<LazySciterArchive>();

                    foreach (var archive in archives)
                    {
                        archive.Open();
                        result.AttachedArchives.TryAdd(archive.Uri.Scheme, archive);

                        if (archive.InitScripts?.Any() != true)
                            continue;

                        foreach (var initScript in archive.InitScripts)
                        {
                            var byteArray = Encoding.UTF8.GetBytes($"include \"{initScript}\";");
                            var pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
                            var pointer = pinnedArray.AddrOfPinnedObject();
                            Sciter.SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_INIT_SCRIPT,
                                pointer);
                            pinnedArray.Free();
                        }
                    }


                    var namedBehaviorResolver = scopedServiceProvider.GetService<INamedBehaviorResolver>();

                    if (namedBehaviorResolver != null)
                        result.SetBehaviorResolver(namedBehaviorResolver);

                    var hostWindowResolver = scopedServiceProvider.GetService<IHostWindowResolver>();

                    var sciterWindow = hostWindowResolver?.GetWindow(typeof(TPrimaryHost));

                    if (sciterWindow != null && windowOptions != null)
                    {
                        windowOptions?.Invoke(sciterWindowOptions);

                        if (!string.IsNullOrWhiteSpace(sciterWindowOptions?.Title))
                            sciterWindow.SetTitle(sciterWindowOptions.Title);

                        if (sciterWindowOptions.Height.HasValue && sciterWindowOptions.Width.HasValue)
                        {
                            //sciterWindow.SetDimensions();
                        }

                        switch (sciterWindowOptions.Position)
                        {
                            case SciterWindowPosition.CenterScreen:
                                sciterWindow.CenterWindow();
                                break;
                            case SciterWindowPosition.Custom:
                                break;
                            case SciterWindowPosition.Default:
                            default:
                                break;
                                ;
                        }
                    }

                    result?.SetupWindow(sciterWindow);
                    //result?.Window.TryLoadPage(uri: new Uri("this://app/index.html"));

                    HostEventHandlerRegistry.Instance
                        .TryGetValue(typeof(TPrimaryHost), out var hostEventHandlerType);

                    if (hostEventHandlerType != null && scopedServiceProvider
                        .GetService(hostEventHandlerType) is SciterEventHandler hostEventHandler)
                        result?.AttachEventHandler(hostEventHandler);

                    //Must load the `Home Page` after setting up the Window and attaching the Hosts' EventHandler
                    if (sciterHostOptions.HomePageUri != null)
                    {
                        var homePageUri = sciterHostOptions.HomePageUri;

                        if (result is SciterArchiveHost && !sciterHostOptions.HomePageUri.IsAbsoluteUri && sciterHostOptions.ArchiveUri != null)
                            homePageUri = new Uri(sciterHostOptions.ArchiveUri, sciterHostOptions.HomePageUri);

                        result?.Window.TryLoadPage(uri: homePageUri);
                    }

                    result
                        ?.OnCreated
                        ?.Invoke(result, new HostCreatedEventArgs(sciterWindow));

                    return result;
                });

            services.AddTransient<SciterHost, TPrimaryHost>(provider => provider.GetRequiredService<TPrimaryHost>());

            services.AddSingleton<SciterApplication>(provider =>
                ActivatorUtilities.CreateInstance<SciterApplication>(provider, provider.GetRequiredService<TPrimaryHost>()));

            return services;
        }

        public static IServiceCollection AddSciter<TPrimaryHost, TPrimaryEventHandler>(this IServiceCollection services,
            Action<SciterHostOptions> hostOptions = null, Action<SciterWindowOptions> windowOptions = null)
            where TPrimaryHost : SciterHost
            where TPrimaryEventHandler : SciterEventHandler
        {
            services.AddHostEventHandler<TPrimaryHost, TPrimaryEventHandler>();
            services.AddSciter<TPrimaryHost>(hostOptions, windowOptions);
            return services;
        }

        #endregion

        #endregion
    }
}