using System;
// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace SciterCore
{
    public static class SciterHostExtensions
    {
        #region Inspector

        public static SciterHost ConnectToInspector(this SciterHost host)
        {
            host?.ConnectToInspectorInternal();
            return host;
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