using System;
using System.Collections.Concurrent;

namespace SciterCore
{
    public class ElementRegistry : ConcurrentDictionary<IntPtr, SciterElement>
    {
        private static readonly Lazy<ElementRegistry>
            Lazy =
                new Lazy<ElementRegistry>
                    (() => new ElementRegistry());

        public static ElementRegistry Instance => Lazy.Value;

        private ElementRegistry() { }
        
    }
}