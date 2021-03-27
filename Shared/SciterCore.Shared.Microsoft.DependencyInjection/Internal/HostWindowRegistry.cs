using System;
using System.Collections.Concurrent;

namespace SciterCore.Internal
{
    internal sealed class HostWindowRegistry : ConcurrentDictionary<Type, Type>
    {
        private static readonly Lazy<HostWindowRegistry>
            Lazy =
                new Lazy<HostWindowRegistry>
                    (() => new HostWindowRegistry());

        public static HostWindowRegistry Instance => Lazy.Value;

        private HostWindowRegistry() { }
    }
}