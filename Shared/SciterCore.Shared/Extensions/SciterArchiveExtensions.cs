using System;
using System.Reflection;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleNamedExpression

namespace SciterCore
{
    public static class SciterArchiveExtensions
    {
	    #region Open Archive

        public static SciterArchive Open(this SciterArchive archive, string resourceName = "SciterResource")
        {
	        archive?.OpenInternal(resourceName: resourceName);
	        return archive;
        }

        public static SciterArchive Open(this SciterArchive archive, Assembly assembly, string resourceName = "SciterResource")
		{
			archive?.OpenInternal(assembly: assembly, resourceName: resourceName);
			return archive;
		}

        public static async Task<SciterArchive> OpenAsync(this SciterArchive archive, string resourceName = "SciterResource", StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
		{
			await archive.OpenInternalAsync(resourceName: resourceName, comparisonType: comparisonType);
			return archive;
		}

        public static async Task<SciterArchive> OpenAsync(this SciterArchive archive, Assembly assembly, string resourceName = "SciterResource")
		{
			await archive.OpenInternalAsync(assembly: assembly, resourceName: resourceName).ConfigureAwait(false);
			return archive;
		}
		
        public static SciterArchive Open(this SciterArchive archive, byte[] buffer)
		{
			archive?.OpenInternal(buffer: buffer);
			return archive;
		}
		
        public static bool TryOpen(this SciterArchive archive, byte[] buffer)
		{
			return archive?.TryOpenInternal(buffer: buffer) == true;
		}

        #endregion

        #region Close Archive

        public static void Close(this SciterArchive archive)
		{
			archive?.CloseInternal();
		}

        #endregion

        #region Get Archive Item

        public static SciterArchive GetItem(this SciterArchive archive, Uri uri, Action<ArchiveGetItemResult> onGetResult)
		{
			archive?.GetItemInternal(uri: uri, onGetResult: onGetResult);
			return archive;
		}

        public static SciterArchive GetItem(this SciterArchive archive, string uriString, Action<ArchiveGetItemResult> onGetResult)
		{
			archive?.GetItemInternal(uriString: uriString, onGetResult: onGetResult);
			return archive;
		}
		
        public static byte[] GetItem(this SciterArchive archive, Uri uri)
		{
			return archive?.GetItemInternal(uri: uri);
		}

        public static byte[] GetItem(this SciterArchive archive, string uriString)
		{
			return archive?.GetItemInternal(uriString: uriString);
		}

        public static bool TryGetItem(this SciterArchive archive, Uri uri, out byte[] data)
        {
	        data = null;
			return archive?.TryGetItemInternal(uri: uri, data: out data) == true;
		}

        public static bool TryGetItem(this SciterArchive archive, Uri uri, Action<ArchiveGetItemResult> onGetResult)
        {
	        byte[] data = null;
	        var result = archive?.TryGetItemInternal(uri: uri, data: out data) == true;
	        onGetResult?.Invoke(new ArchiveGetItemResult(data, uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped), result && data != null));
	        return result;
        }

        public static bool TryGetItem(this SciterArchive archive, string uriString, Action<ArchiveGetItemResult> onGetResult)
        {
	        var actualUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
	        return TryGetItem(archive: archive, uri: actualUri, onGetResult: onGetResult);
        }

		#endregion
    }
}