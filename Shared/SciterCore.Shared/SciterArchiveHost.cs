using System.Linq;
using System.Reflection;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore
{
    /// <summary>
    /// A <see cref="SciterHost"/> with built-in <see cref="SciterArchive"/> support.
    /// </summary>
    public class SciterArchiveHost : SciterHost
    {
        
#if NETCORE
        public SciterArchiveHost()
		{
			var archiveAttribute = GetType().GetCustomAttributes<SciterHostArchiveAttribute>(inherit: true).FirstOrDefault();
			
			Archive = new SciterArchive(archiveAttribute?.BaseUrl ?? SciterArchive.DEFAULT_ARCHIVE_URI)
				.Open();
		}
#endif
		
        public SciterArchiveHost(string baseUri = SciterArchive.DEFAULT_ARCHIVE_URI)
        {
            Archive = new SciterArchive(baseUri ?? SciterArchive.DEFAULT_ARCHIVE_URI)
                .Open();
        }

        protected SciterArchive Archive { get; }

        protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
        {
            // load resource from SciterArchive
            Archive?.GetItem(args.Uri, res =>
            {
                if (res.IsSuccessful)
                    Sciter.SciterApi.SciterDataReady(Window.Handle, res.Path, res.Data, (uint) res.Size);
            });

            // call base to ensure LibConsole is loaded
            return base.OnLoadData(sender: sender, args: args);
        }
    }
}