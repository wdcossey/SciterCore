using System;

namespace SciterCore.Internal
{
    internal sealed class HostWindowResolver : IHostWindowResolver
    {
        private readonly IServiceProvider _provider;

        public HostWindowResolver(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public bool ContainsKey(Type type)
        {
            return HostWindowRegistry.Instance.ContainsKey(key: type);
        }

        public SciterWindow GetWindow(Type hostType)
        {
            if (HostWindowRegistry.Instance.TryGetValue(hostType, out var serviceType))
                return _provider?.GetService(serviceType) as SciterWindow;
            
            //TODO: Add a message here!
            throw new InvalidOperationException();
        }
    }
}