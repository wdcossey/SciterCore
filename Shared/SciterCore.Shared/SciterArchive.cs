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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using System.Linq;
using System.Reflection;

namespace SciterCore
{
	public class SciterArchive : IDisposable
	{
        private static readonly Sciter.SciterApi Api = Sciter.Api;
		private IntPtr _handle;
		private GCHandle _pinnedArray;

		public const string DEFAULT_ARCHIVE_URI = "archive://app/";

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
			Close();
		}

        #endregion

        #region Open Archive

        public SciterArchive Open(string resourceName = "SciterResource")
        {
			return OpenAsync(resourceName: resourceName).GetAwaiter().GetResult();
		}

		public SciterArchive Open(Assembly assembly, string resourceName = "SciterResource")
		{
			return OpenAsync(assembly: assembly, resourceName: resourceName).GetAwaiter().GetResult();
		}

		public Task<SciterArchive> OpenAsync(string resourceName = "SciterResource", StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
		{
			var assembly = Assembly.GetEntryAssembly();
			if (assembly?.GetManifestResourceNames().Any(a => a.Equals(resourceName, comparisonType: comparisonType)) != true)
            {
				assembly = Assembly.GetExecutingAssembly();
			}

			return OpenAsync(assembly: assembly, resourceName: resourceName);
		}

		public async Task<SciterArchive> OpenAsync(Assembly assembly, string resourceName = "SciterResource")
		{
			ArchiveAlreadyOpened();

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new InvalidOperationException($"Could not load manifest resource stream ({resourceName}).");
				}

				var buffer = new byte[stream.Length];

				await stream.ReadAsync(buffer, 0, buffer.Length);

				return Open(buffer: buffer);
			}
		}
		
		public SciterArchive Open(byte[] buffer)
		{
			ArchiveAlreadyOpened();

			_pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			_handle = Api.SciterOpenArchive(_pinnedArray.AddrOfPinnedObject(), Convert.ToUInt32(buffer.Length));
			Debug.Assert(_handle != IntPtr.Zero);

			return this;
		}

        #endregion

        #region Close Archive

        public void Close()
		{
			ArchiveNotOpened();
			
			Api.SciterCloseArchive(_handle);
			_handle = IntPtr.Zero;
			_pinnedArray.Free();
		}

        #endregion

        #region Get Archive Item

        public SciterArchive GetItem(Uri uri, Action<byte[], string> onFound)
		{
			var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);
			
			var data = this?.GetItem(actualUri);

			if(data != null)
				onFound?.Invoke(data, actualUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped));
			
			return this;
		}

		public SciterArchive GetItem(string uriString, Action<byte[], string> onFound)
		{
			var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
			return GetItem(uri, onFound: onFound);
		}
		
		public byte[] GetItem(Uri uri)
		{
			var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);

			if (!IsOpen || !actualUri.GetLeftPart(UriPartial.Scheme).Equals(this.Uri.GetLeftPart(UriPartial.Scheme)))
				return null;
			
			var path = actualUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);

			var found = Api.SciterGetArchiveItem(_handle, path, out var dataPtr, out var dataLength);

			if (!found) 
				return null;
				
			var result = new byte[dataLength];
			Marshal.Copy(dataPtr, result, 0, Convert.ToInt32(dataLength));
			return result;
		}

		public byte[] GetItem(string uriString)
		{
			var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
			var actualUri = uri.IsAbsoluteUri ? uri : new Uri(this.Uri, uri);
			return GetItem(actualUri);
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
