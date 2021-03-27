using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SciterCore.Helpers
{
    internal class SciterWindowResolver : Dictionary<Type, Type>, ISciterWindowResolver
    {
        public SciterWindowResolver()
        {
            
        }
    }
}