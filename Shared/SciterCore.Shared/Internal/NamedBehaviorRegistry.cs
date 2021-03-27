using System;
using System.Collections.Concurrent;

namespace SciterCore.Internal
{
    internal sealed class NamedBehaviorRegistry : ConcurrentDictionary<string, Type>
    {
        private static readonly Lazy<NamedBehaviorRegistry>
            Lazy =
                new Lazy<NamedBehaviorRegistry>
                    (() => new NamedBehaviorRegistry());

        public static NamedBehaviorRegistry Instance => Lazy.Value;

        private NamedBehaviorRegistry() { }
    }
}