using System;

namespace SciterCore.Internal
{
    public interface IHostWindowResolver
    {
        bool ContainsKey(Type type);
        
        SciterWindow GetWindow(Type type);
    }
}