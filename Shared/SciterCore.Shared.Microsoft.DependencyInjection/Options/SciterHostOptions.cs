using System;

namespace SciterCore.Options
{
    public sealed class SciterHostOptions
    {
        #region Public properties
        
        #endregion

        #region Internal properties

        internal Uri HomePageUri { get; set; }
        
        internal Uri ArchiveUri { get; set; }

        #endregion
        
        #region Contructor(s)

        public SciterHostOptions() { }

        #endregion
        
        #region Public methods

        public SciterHostOptions SetHomePage(string homePageUrl)
        {
            HomePageUri = new Uri(homePageUrl, UriKind.RelativeOrAbsolute);
            return this;
        }

        public SciterHostOptions SetHomePage(Uri homePageUri)
        {
            HomePageUri = homePageUri;
            return this;
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(string baseUrl = SciterArchive.DEFAULT_ARCHIVE_URI)
        {
            ArchiveUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);
            return this;
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Uri baseUrl)
        {
            ArchiveUri = baseUrl;
            return this;
        }
        
        #endregion

    }
    
}