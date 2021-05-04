using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SciterCore.Internal
{
    internal sealed class LazySciterArchive : SciterArchive
    {
        public Assembly Assembly { get; }
        
        public string ResourceName { get; }
        public string[] InitScripts { get; }

        #region Constructor(s)
        
        public LazySciterArchive(string uri, Assembly assembly, string resourceName = "SciterResource", params string[] initScripts)
            : base(uri: uri)
        {
            Assembly = assembly;
            ResourceName = resourceName;
            InitScripts = initScripts;
        }

        #endregion
        
        #region Open Archive

        public void Open()
        {
            OpenAsync().GetAwaiter().GetResult();
        }

        public Task OpenAsync()
        {
            return OpenInternalAsync(assembly: Assembly, resourceName: ResourceName);
        }

        #endregion

    }
}