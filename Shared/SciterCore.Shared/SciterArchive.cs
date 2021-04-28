// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using System.Linq;
using System.Reflection;

namespace SciterCore
{
	public class SciterArchive : IDisposable
	{
        private static readonly ISciterApi SciterApi = Sciter.SciterApi;
		private IntPtr _handle;
		private GCHandle _pinnedArray;

		public const string DEFAULT_ARCHIVE_URI = "this://app/";

		public Uri Uri { get; private set; }

		public bool IsOpen => _handle != IntPtr.Zero;

		#region Constructor(s)

        public SciterArchive(string uri = DEFAULT_ARCHIVE_URI)
		{
			this.Uri = new Uri($"{uri}", UriKind.Absolute);
		}
		
		public SciterArchive(Uri baseUri)
		{
			this.Uri = baseUri;
		}

        #endregion

        #region Interface Implemenations 

        public void Dispose()
		{
			CloseInternal();
		}

        #endregion

        #region Open Archive

        internal void OpenInternal(string resourceName)
        {
	        OpenInternalAsync(resourceName: resourceName).GetAwaiter().GetResult();
		}

        internal void OpenInternal(Assembly assembly, string resourceName)
		{
			OpenInternalAsync(assembly: assembly, resourceName: resourceName).GetAwaiter().GetResult();
		}

		internal Task OpenInternalAsync(string resourceName, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
		{
			var assembly = Assembly.GetEntryAssembly();
			if (assembly?.GetManifestResourceNames().Any(a => a.Equals(resourceName, comparisonType: comparisonType)) != true)
            {
				assembly = Assembly.GetExecutingAssembly();
			}

			return OpenInternalAsync(assembly: assembly, resourceName: resourceName);
		}

		internal async Task OpenInternalAsync(Assembly assembly, string resourceName)
		{
			ArchiveAlreadyOpened();
			
			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
					throw new InvalidOperationException($"Could not load manifest resource stream ({resourceName}).");

				var buffer = new byte[stream.Length];

				await stream.ReadAsync(buffer, 0, buffer.Length);

				OpenInternal(buffer: buffer);
			}
		}
		
		internal void OpenInternal(byte[] buffer)
		{
			TryOpenInternal(buffer: buffer);
		}
		
		internal bool TryOpenInternal(byte[] buffer)
		{
			ArchiveAlreadyOpened();
			_pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			_handle = SciterApi.SciterOpenArchive(_pinnedArray.AddrOfPinnedObject(), System.Convert.ToUInt32(buffer.Length));
			return !_handle.Equals(IntPtr.Zero);
		}

        #endregion

        #region Close Archive

        internal void CloseInternal()
		{
			ArchiveNotOpened();
			
			SciterApi.SciterCloseArchive(_handle);
			_handle = IntPtr.Zero;
			_pinnedArray.Free();
		}

        #endregion

        #region Get Archive Item

        internal void GetItemInternal(Uri uri, Action<ArchiveGetItemResult> onGetResult)
		{
			var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);
			
			var data = GetItemInternal(actualUri);

			onGetResult?.Invoke(new ArchiveGetItemResult(data, actualUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped), data != null));
		}

        internal void GetItemInternal(string uriString, Action<ArchiveGetItemResult> onGetResult)
		{
			var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
			GetItemInternal(uri, onGetResult: onGetResult);
		}
		
        internal byte[] GetItemInternal(string uriString)
        {
	        var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
	        var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);
	        return GetItemInternal(actualUri);
        }
        
        internal byte[] GetItemInternal(Uri uri)
        {
	        TryGetItemInternal(uri: uri, out var result);
			return result;
		}
        
        internal bool TryGetItemInternal(Uri uri, out byte[] data)
        {
	        data = null;
	        
			var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);

			if (!IsOpen || !actualUri.GetLeftPart(UriPartial.Scheme).Equals(this.Uri.GetLeftPart(UriPartial.Scheme)))
				return false;
			
			var path = actualUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);

			var found = SciterApi.SciterGetArchiveItem(_handle, path, out var dataPtr, out var dataLength);

			if (!found) 
				return false;
				
			var buffer = new byte[dataLength];
			Marshal.Copy(dataPtr, buffer, 0, System.Convert.ToInt32(dataLength));
			data = buffer;
			
			return true;
		}
        
        internal bool TryGetItemInternal(string uriString, out byte[] data)
        {
	        var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
	        var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);
	        return TryGetItemInternal(actualUri, out data);
		}

        #endregion

        #region Private Methods

        private void ArchiveNotOpened()
		{
			if(_handle == IntPtr.Zero)
				throw new InvalidOperationException("Archive not opened.");
		}
		
		private void ArchiveAlreadyOpened()
		{
			if(_handle != IntPtr.Zero)
				throw new InvalidOperationException("Archive already opened.");
		}

		#endregion
	}
}
