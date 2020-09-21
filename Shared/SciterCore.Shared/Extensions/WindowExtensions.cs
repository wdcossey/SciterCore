using System;

namespace SciterCore
{
    public static class WindowExtensions
    {
        public static THost CreateHost<THost>(this SciterWindow window)
            where THost : SciterHost, new()
        {
            return (THost)Activator.CreateInstance(type: typeof(THost), new object[] { window });
        }

        /// <summary>
        /// Loads the page resource from the given URL or file path
        /// </summary>
        /// <param name="window"></param>
        /// <param name="uri">URL or file path of the page</param>
        public static SciterWindow LoadPage(this SciterWindow window, Uri uri)
		{
			window?.LoadPageInternal(uri: uri);
			return window;
		}

        /// <summary>
        /// Loads the page resource from the given URL or file path
        /// </summary>
        /// <param name="window"></param>
        /// <param name="uri">URL or file path of the page</param>
        public static bool TryLoadPage(this SciterWindow window, Uri uri)
        {
	        return window?.TryLoadPageInternal(uri: uri) == true;
        }

        /// <summary>
        /// Loads HTML input from a string
        /// </summary>
        /// <param name="window"></param>
        /// <param name="html">HTML of the page to be loaded</param>
        /// <param name="baseUrl">Base Url given to the loaded page</param>
        public static SciterWindow LoadHtml(this SciterWindow window, string html, string baseUrl = null)
		{
			window?.LoadHtmlInternal(html: html, baseUrl: baseUrl);
			return window;
		}

        /// <summary>
        /// Loads HTML input from a string
        /// </summary>
        /// <param name="window"></param>
        /// <param name="html">HTML of the page to be loaded</param>
        /// <param name="baseUrl">Base Url given to the loaded page</param>
        public static bool TryLoadHtml(this SciterWindow window, string html, string baseUrl = null)
		{
			return window?.TryLoadHtmlInternal(html: html, baseUrl: baseUrl) == true;
		}
    }
}
