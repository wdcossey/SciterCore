using System;
using SciterCore;

namespace SciterCoreShared.Extensions
{
    public static class WindowExtensions
    {
        public static THost CreateHost<THost>(this SciterWindow window)
            where THost : SciterHost, new()
        {
            return (THost)Activator.CreateInstance(type: typeof(THost), new object[] { window });
        }
    }
}
