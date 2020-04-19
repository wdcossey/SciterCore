using System;
using SciterCore.Extensions;

namespace SciterCore
{
    internal struct EventHandlerRegistry
    {
        private SciterEventHandler _eventHandler;

        private EventHandlerRegistry(string name)
            : this()
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
            EventHandler = eventHandler;
        }

        internal string Name { get; }

        internal Type Type { get; }

        internal SciterEventHandler EventHandler
        {
            get
            {
                return GetOrCreateEventHandler(_eventHandler);
            }

            private set
            {
                _eventHandler = value;
            }
        }

        private SciterEventHandler GetOrCreateEventHandler(SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
            {
                eventHandler = (SciterEventHandler)Activator.CreateInstance(Type);
                eventHandler.Name = $"Create by registered native behavior factory: {Name}";
            }
            return eventHandler;
        }

    }
}
