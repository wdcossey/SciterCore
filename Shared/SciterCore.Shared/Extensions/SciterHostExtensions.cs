using System;
using System.Threading.Tasks;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArgumentsStyleOther

// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace SciterCore
{
    public static class SciterHostExtensions
    {
        #region Inspector

        public static Task<SciterHost> ConnectToInspectorAsync(this SciterHost host)
        {
            host?.ConnectToInspectorInternalAsync();
            return Task.FromResult(host);
        }
        
        public static SciterHost ConnectToInspector(this SciterHost host)
        {
            return host?.ConnectToInspectorAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Attaches a window level event-handler: it receives every event for all elements of the page.
        /// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
        /// You can only attach a single event-handler.
        /// </summary>
        public static SciterHost AttachEventHandler(this SciterHost host, SciterEventHandler eventHandler)
        {
            host?.AttachEventHandlerInternal(eventHandler: eventHandler);
            return host;
        }
        
        /// <summary>
        /// Attaches a window level event-handler: it receives every event for all elements of the page.
        /// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
        /// You can only attach a single event-handler.
        /// </summary>
        public static SciterHost AttachEventHandler<THandler>(this SciterHost host)
            where THandler: SciterEventHandler
        {
            return host?.AttachEventHandler(eventHandler: Activator.CreateInstance<THandler>());
        }

        /// <summary>
        /// Attaches a window level event-handler: it receives every event for all elements of the page.
        /// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
        /// You can only attach a single event-handler.
        /// </summary>
        public static SciterHost AttachEventHandler<THandler>(this SciterHost host, Func<THandler> eventHandlerFunc)
            where THandler: SciterEventHandler
        {
            return host?.AttachEventHandler(eventHandler: eventHandlerFunc.Invoke());
        }
        
        /// <summary>
        /// Attaches a window level event-handler: it receives every event for all elements of the page.
        /// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
        /// You can only attach a single event-handler.
        /// </summary>
        public static bool TryAttachEventHandler(this SciterHost host, SciterEventHandler eventHandler)
        {
            return host?.TryAttachEventHandlerInternal(eventHandler: eventHandler) == true;
        }
        
        /// <summary>
        /// Detaches the event-handler previously attached with AttachEvh()
        /// </summary>
        public static SciterHost DetachEventHandler(this SciterHost host)
        {
            host?.DetachEventHandlerInternal();
            return host;
        }
        
        /// <summary>
        /// Detaches the event-handler previously attached with AttachEvh()
        /// </summary>
        public static bool TryDetachEventHandler(this SciterHost host)
        {
            return host?.TryDetachEventHandlerInternal() == true;
        }
        
        #endregion
        #region Notification

        /// <summary>
        /// <para>Sciter cross-platform alternative for posting a message in the message queue.</para>
        /// <para>It will be received as a SC_POSTED_NOTIFICATION notification by this <see cref="SciterHost"/> instance.</para>
        /// <para>Override <see cref="SciterHost.OnPostedNotification"/> to handle it.</para>
        /// </summary>
        /// <param name="host"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <param name="timeout">
        /// If timeout is > 0 this methods SENDs the message instead of POSTing and this is the timeout for waiting the processing of the message. Leave it as 0 for actually POSTing the message.
        /// </param>
        public static IntPtr PostNotification(this SciterHost host, IntPtr wparam, IntPtr lparam, uint timeout = 0)
        {
            return host?.PostNotificationInternal(wparam: wparam, lparam: lparam, timeout: timeout) ?? IntPtr.Zero;
        }

        #endregion
        
        #region Behavior Factory
		
        public static SciterHost RegisterBehaviorHandler(this SciterHost host, Type eventHandlerType, string behaviorName = null)
        {
            host?.RegisterBehaviorHandlerInternal(eventHandlerType: eventHandlerType, behaviorName: behaviorName);
            return host;
        }

        public static SciterHost RegisterBehaviorHandler<TType>(this SciterHost host, string behaviorName = null)
            where TType : SciterEventHandler
        {
            host.RegisterBehaviorHandlerInternal<TType>(behaviorName: behaviorName);
            return host;
        }

        public static SciterHost RegisterBehaviorHandler<THandler>(this SciterHost host, THandler eventHandler, string behaviorName = null)
            where THandler : SciterEventHandler
        {
            host.RegisterBehaviorHandlerInternal<THandler>(eventHandler: eventHandler, behaviorName: behaviorName);
            return host;
        }

        public static SciterHost RegisterBehaviorHandler<THandler>(this SciterHost host, Func<THandler> eventHandlerFunc, string behaviorName = null)
            where THandler : SciterEventHandler
        {
            host.RegisterBehaviorHandlerInternal<THandler>(eventHandlerFunc: eventHandlerFunc, behaviorName: behaviorName);
            return host;
        }

        #endregion
    }
}