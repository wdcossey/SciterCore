using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SciterCore.Helpers
{
    public class SciterWindowResolver : Dictionary<Type, Type>, ISciterWindowResolver
    {
        public SciterWindowResolver()
        {
            
        }
    }
}