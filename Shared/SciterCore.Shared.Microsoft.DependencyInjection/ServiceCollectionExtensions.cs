﻿using System;
using System.Linq;
using System.Reflection;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Enums;
using SciterCore.Extensions;
using SciterCore.Internal;
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

                //Only setup the window if it's from the attribute
                //if (hostWindowAttribute != null)
                //{
                    result
                        ?.CreateMainWindow(hostWindowAttribute?.Width ?? 800, hostWindowAttribute?.Height ?? 600)
                        ?.SetTitle(hostWindowAttribute?.Title ?? "SciterCore");
                //}

                return result;
            });

            return services;
        }

        #endregion

        #region Public methods

        public static IServiceCollection AddSciterBehavior<THandler>(this IServiceCollection services)
            where THandler : SciterEventHandler
        {
            var behaviourName = typeof(THandler).GetBehaviourName();
                
            NamedBehaviorRegistry.Instance.TryAdd(behaviourName, typeof(THandler));
            //BehaviorRegistry.Instance.Add(behaviourName, typeof(THandler));
            
            services.Add(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), ServiceLifetime.Transient));
            return services;
        }
        
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
                    var scopedServiceProvider = provider.CreateScope().ServiceProvider;

                    var sciterHostOptions = new SciterHostOptions();
                    var sciterWindowOptions = new SciterWindowOptions();
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
                    
                    if (sciterHostOptions.ArchiveUri != null)
                        ;

                    var result = ActivatorUtilities.CreateInstance<TPrimaryHost>(scopedServiceProvider);
                    
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

                        result?.SetupWindow(sciterWindow);
                        //result?.Window.TryLoadPage(uri: new Uri("this://app/index.html"));
                    }

                    HostEventHandlerRegistry.Instance
                        .TryGetValue(typeof(TPrimaryHost), out var hostEventHandlerType);

                    if (hostEventHandlerType != null && scopedServiceProvider
                        .GetService(hostEventHandlerType) is SciterEventHandler hostEventHandler)
                        result?.AttachEventHandler(hostEventHandler);

                    //Must load the `Home Page` after setting up the Window and attaching the Hosts' EventHandler
                    if (sciterHostOptions.HomePageUri != null)
                    {
                        var homePageUri = sciterHostOptions.HomePageUri;
                        
                        if (!sciterHostOptions.HomePageUri.IsAbsoluteUri && sciterHostOptions.ArchiveUri != null)
                            homePageUri = new Uri(sciterHostOptions.ArchiveUri, sciterHostOptions.HomePageUri);

                        result?.Window.TryLoadPage(uri: homePageUri);
                    }

                    //if (!string.IsNullOrWhiteSpace(hostWindow?.HomePage))
                    //    result?.Window.TryLoadPage(uri: new Uri(hostWindow.HomePage));

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
            //services.Replace(ServiceDescriptor.Describe(typeof()))
            return services;
        }

        /*public static IServiceCollection AddSciterWithDefaultHost<TPrimaryEventHandler>(this IServiceCollection services,
            Action<SciterHostOptions> sciterHostOptions = null)
            where TPrimaryEventHandler : SciterEventHandler
        {
            services.AddSciter<SciterHost>(sciterHostOptions);
            
            //services.Replace(ServiceDescriptor.Describe(typeof()))
            return services;
        }

        public static IServiceCollection AddSciterWithArchiveHost<TPrimaryEventHandler>(this IServiceCollection services,
            Action<SciterHostOptions> sciterHostOptions = null)
            where TPrimaryEventHandler : SciterEventHandler
        {
            services.AddSciter<SciterHost>(sciterHostOptions);
            
            //services.Replace(ServiceDescriptor.Describe(typeof()))
            return services;
        }*/
        
        #endregion
    }
}