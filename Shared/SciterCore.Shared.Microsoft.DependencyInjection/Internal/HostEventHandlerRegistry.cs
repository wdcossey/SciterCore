using System;
using System.Collections.Concurrent;

namespace SciterCore.Internal
{
    internal sealed class HostEventHandlerRegistry: ConcurrentDictionary<Type, Type>
    {
        private static readonly Lazy<HostEventHandlerRegistry>
            Lazy =
                new Lazy<HostEventHandlerRegistry>
                    (() => new HostEventHandlerRegistry());

        public static HostEventHandlerRegistry Instance => Lazy.Value;

        private HostEventHandlerRegistry() { }
    }
}