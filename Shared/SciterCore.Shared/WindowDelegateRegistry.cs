using System.Collections.Concurrent;
using SciterCore.Interop;

namespace SciterCore
{
    internal static class WindowDelegateRegistry
    {
        private static ConcurrentDictionary<SciterWindow, SciterXDef.SCITER_WINDOW_DELEGATE> Registry { get; } =
            new ConcurrentDictionary<SciterWindow, SciterXDef.SCITER_WINDOW_DELEGATE>();

        internal static SciterXDef.SCITER_WINDOW_DELEGATE Get(SciterWindow window)
        {
            return Registry.TryGetValue(window, out var result) ? result : null;
        }

        internal static SciterXDef.SCITER_WINDOW_DELEGATE Set(SciterWindow window,
            SciterXDef.SCITER_WINDOW_DELEGATE @delegate)
        {
            if (Registry.ContainsKey(window))
                return Get(window);

            Registry.TryAdd(window, @delegate);

            return @delegate;
        }

        internal static void Remove(SciterWindow window)
        {
            if (Registry.ContainsKey(window))
                Registry.TryRemove(window, out var @delegate);
        }
    }
}