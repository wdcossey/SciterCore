using System;

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

        internal EventHandlerRegistry(string name, Type type)
            : this(name: name)
        { 
            Type = type;
        }

        internal EventHandlerRegistry(string name, SciterEventHandler eventHandler)
            : this(name: name)
        {
            EventHandler = eventHandler;
        }

        internal string Name { get; }

        internal Type Type { get; }

        internal SciterEventHandler EventHandler
        {
            get
            {
                return _eventHandler ??
                    (_eventHandler = (SciterEventHandler)Activator.CreateInstance(Type, new object[] { $"Create by registered native behavior factory: {Name}" }));
            }

            private set
            {
                _eventHandler = value;
            }
        }

    }
}
