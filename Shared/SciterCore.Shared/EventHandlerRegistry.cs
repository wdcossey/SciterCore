using System;
using SciterCore.Extensions;

namespace SciterCore
{
    internal class EventHandlerRegistry
    {
        private readonly SciterEventHandler _eventHandler;

        private EventHandlerRegistry(string name)
        {
            Name = name;
        }

        internal EventHandlerRegistry(Type type, string name = null)
            : this(name: type.GetBehaviourName(behaviorName: name))
        { 
            Type = type;
        }

        internal EventHandlerRegistry(SciterEventHandler eventHandler, string name = null)
            : this(name: eventHandler.GetBehaviourName(behaviorName: name))
        {
            _eventHandler = eventHandler;
        }

        internal string Name { get; }

        internal Type Type { get; }

        internal SciterEventHandler Create(SciterHost host = null)
        {
            if (_eventHandler == null)
                return (Activator.CreateInstance(Type) as SciterEventHandler)?
                    .SetName($"Created by registered native behavior factory: {Name}")
                    .SetHost(host);
            
            _eventHandler.SetHost(host);
            return _eventHandler;
        }
    }
}
