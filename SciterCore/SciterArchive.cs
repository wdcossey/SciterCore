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
        private static readonly Sciter.SciterApi _api = Sciter.Api;
		private IntPtr _handle;
		private GCHandle _pinnedArray;

		private const string DEFAULT_URI = "archive://app/";

		public Uri Uri { get; private set; }

		public bool IsOpen => _handle != IntPtr.Zero;

		#region Constructor(s)

        public SciterArchive(string uri = DEFAULT_URI)
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

        public SciterArchive Open(string resourceName)
        {
			return OpenAsync(resourceName: resourceName).GetAwaiter().GetResult();
		}

		public SciterArchive Open(Assembly assembly, string resourceName)
		{
			return OpenAsync(assembly: assembly, resourceName: resourceName).GetAwaiter().GetResult();
		}

		public Task<SciterArchive> OpenAsync(string resourceName, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
		{
			var assembly = Assembly.GetEntryAssembly();
			if (assembly?.GetManifestResourceNames().Any(a => a.Equals(resourceName, comparisonType: comparisonType)) != true)
            {
				assembly = Assembly.GetExecutingAssembly();
			}

			return OpenAsync(assembly: assembly, resourceName: resourceName);
		}

		public async Task<SciterArchive> OpenAsync(Assembly assembly, string resourceName)
		{
			ArchiveAlreadyOpened();

			using (var stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new InvalidOperationException("Could not load manifest resource stream.");
				}

				byte[] buffer = new byte[stream.Length];

				await stream.ReadAsync(buffer, 0, buffer.Length);

				return Open(buffer);
			}
		}
		
		public SciterArchive Open(byte[] res_array)
		{
			ArchiveAlreadyOpened();

			_pinnedArray = GCHandle.Alloc(res_array, GCHandleType.Pinned);
			_handle = _api.SciterOpenArchive(_pinnedArray.AddrOfPinnedObject(), (uint) res_array.Length);
			Debug.Assert(_handle != IntPtr.Zero);

			return this;
		}

        #endregion

        #region Close Archive

        public void Close()
		{
			ArchiveNotOpened();
			
			_api.SciterCloseArchive(_handle);
			_handle = IntPtr.Zero;
			_pinnedArray.Free();
		}

        #endregion

        #region Get Archive Item

        public SciterArchive GetItem(Uri uri, Action<byte[], string> onFound)
		{
			byte[] data = this?.GetItem(uri);

			if(data != null)
			{
				onFound?.Invoke(data, uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped));
			}
			return this;
		}

		public SciterArchive GetItem(string uriString, Action<byte[], string> onFound)
		{
			var uri = new Uri(uriString);
			return GetItem(uri, onFound: onFound);
		}
		
		public byte[] GetItem(Uri uri)
		{
			if (IsOpen && uri.GetLeftPart(UriPartial.Scheme).Equals(this.Uri.GetLeftPart(UriPartial.Scheme)))
			{
				var path = uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);

				bool found = _api.SciterGetArchiveItem(_handle, path, out var dataPtr, out var dataLenth);

				if(found)
				{
					byte[] res = new byte[dataLenth];
					Marshal.Copy(dataPtr, res, 0, (int) dataLenth);

					return res;
				}
			}
			return null;
		}

		public byte[] GetItem(string uriString)
		{
			var uri = new Uri(uriString);
			return GetItem(uri);
		}

		[Obsolete("Use the GetItem(Uri) method")]
		public byte[] Get(string path)
		{
			var uri = new Uri(path);

			return GetItem(uri: uri);
		}

        #endregion

        #region Private Methods

        private void ArchiveNotOpened()
		{
			if(_handle == IntPtr.Zero)
			{
				throw new Exception("You haven't yet opened this archive.");
			}
		}
		
		private void ArchiveAlreadyOpened()
		{
			if(_handle != IntPtr.Zero)
			{
				throw new Exception("Archive already open.");
			}
		}

		#endregion
	}
}
