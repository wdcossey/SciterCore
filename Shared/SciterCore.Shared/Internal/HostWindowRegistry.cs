using System;
using System.Collections.Concurrent;

namespace SciterCore.Internal
{
    public class HostWindowRegistry : ConcurrentDictionary<Type, Type>
    {
        private static readonly Lazy<HostWindowRegistry>
            Lazy =
                new Lazy<HostWindowRegistry>
                    (() => new HostWindowRegistry());

        public static HostWindowRegistry Instance => Lazy.Value;

        private HostWindowRegistry() { }
    }
}