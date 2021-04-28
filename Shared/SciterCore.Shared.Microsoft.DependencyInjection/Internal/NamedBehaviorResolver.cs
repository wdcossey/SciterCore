using System;
using Microsoft.Extensions.DependencyInjection;

namespace SciterCore.Internal
{
    internal sealed class NamedBehaviorResolver : INamedBehaviorResolver
    {
        private readonly IServiceProvider _provider;

        public NamedBehaviorResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public bool ContainsKey(string name)
        {
            return NamedBehaviorRegistry.Instance.ContainsKey(key: name);
        }

        public SciterEventHandler GetBehaviorHandler(string name)
        {
            if (NamedBehaviorRegistry.Instance.TryGetValue(name, out var serviceType))
                return _provider?.GetRequiredService(serviceType) as SciterEventHandler;
            
            //TODO: Add a message here!
            throw new InvalidOperationException();
        }
    }
}
