#if NETCORE

namespace SciterCore.Internal
{
    public interface INamedBehaviorResolver
    {
        bool ContainsKey(string name);
        
        SciterEventHandler GetBehaviorHandler(string name);
    }
}

#endif
