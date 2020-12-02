using System.Collections.Concurrent;
using SciterCore.Interop;

namespace SciterCore
{
    internal static class HostCallbackRegistry
    {
        private static ConcurrentDictionary<SciterHost, SciterXDef.SCITER_HOST_CALLBACK> Registry { get; } =
            new ConcurrentDictionary<SciterHost, SciterXDef.SCITER_HOST_CALLBACK>();

        internal static SciterXDef.SCITER_HOST_CALLBACK Get(SciterHost host)
        {
            return Registry.TryGetValue(host, out var result) ? result : null;
        }

        internal static SciterXDef.SCITER_HOST_CALLBACK Set(SciterHost host, 
            SciterXDef.SCITER_HOST_CALLBACK @callback)
        {
            if (Registry.ContainsKey(host))
                return Get(host);

            Registry.TryAdd(host, @callback);

            return @callback;
        }

        internal static void Remove(SciterHost host)
        {
            if (Registry.ContainsKey(host))
                Registry.TryRemove(host, out _);
        }
    }
}