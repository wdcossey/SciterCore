using System;

namespace SciterCore.Options
{
    public sealed class SciterHostOptions
    {
        private readonly IServiceProvider _serviceProvider;
        
        #region Internal properties

        internal Uri HomePageUri { get; set; }
        
        internal Uri ArchiveUri { get; set; }

        #endregion
        
        #region Contructor(s)

        internal SciterHostOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion
        
        #region Public methods

        #region SetHomePage

        public SciterHostOptions SetHomePage(string homePageUrl)
        {
            if (string.IsNullOrWhiteSpace(homePageUrl))
                throw new ArgumentNullException(nameof(homePageUrl));
            
            var homePageUri = new Uri(homePageUrl, UriKind.RelativeOrAbsolute);
            return SetHomePage(homePageUri);
        }

        public SciterHostOptions SetHomePage(Func<string> homePageUrlFunc)
        {
            if (homePageUrlFunc == null)
                throw new ArgumentNullException(nameof(homePageUrlFunc));
            
            return SetHomePage(homePageUrlFunc());
        }

        public SciterHostOptions SetHomePage(Func<IServiceProvider, string> homePageUrlFunc)
        {
            if (homePageUrlFunc == null)
                throw new ArgumentNullException(nameof(homePageUrlFunc));
            
            return SetHomePage(homePageUrlFunc(_serviceProvider));
        }

        public SciterHostOptions SetHomePage(Uri homePageUri)
        {
            if (homePageUri == null)
                throw new ArgumentNullException(nameof(homePageUri));
            
            HomePageUri = homePageUri;
            return this;
        }

        public SciterHostOptions SetHomePage(Func<Uri> homePageUriFunc)
        {
            if (homePageUriFunc == null)
                throw new ArgumentNullException(nameof(homePageUriFunc));
            
            return SetHomePage(homePageUriFunc());
        }

        public SciterHostOptions SetHomePage(Func<IServiceProvider, Uri> homePageUriFunc)
        {
            if (homePageUriFunc == null)
                throw new ArgumentNullException(nameof(homePageUriFunc));
            
            return SetHomePage(homePageUriFunc(_serviceProvider));
        }

        #endregion

        #region SetArchiveUri

        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(string baseUrl = SciterArchive.DEFAULT_ARCHIVE_URI)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));
            
            var archiveUri = new Uri(baseUrl, UriKind.RelativeOrAbsolute);
            return SetArchiveUri(archiveUri);
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUrlFunc"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Func<string> baseUrlFunc)
        {
            if (baseUrlFunc == null)
                throw new ArgumentNullException(nameof(baseUrlFunc));

            return SetArchiveUri(baseUrlFunc());
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUrlFunc"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Func<IServiceProvider, string> baseUrlFunc)
        {
            if (baseUrlFunc == null)
                throw new ArgumentNullException(nameof(baseUrlFunc));

            return SetArchiveUri(baseUrlFunc(_serviceProvider));
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Uri baseUri)
        {            
            if (baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));
            
            ArchiveUri = baseUri;
            return this;
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUriFunc"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Func<Uri> baseUriFunc)
        {
            if (baseUriFunc == null)
                throw new ArgumentNullException(nameof(baseUriFunc));
            
            return SetArchiveUri(baseUriFunc());
        }
        
        /// <summary>
        /// Only applies to <see cref="SciterArchiveHost"/>
        /// </summary>
        /// <param name="baseUriFunc"></param>
        /// <returns></returns>
        public SciterHostOptions SetArchiveUri(Func<IServiceProvider, Uri> baseUriFunc)
        {
            if (baseUriFunc == null)
                throw new ArgumentNullException(nameof(baseUriFunc));
            
            return SetArchiveUri(baseUriFunc(_serviceProvider));
        }

        #endregion

        #endregion
    }
}