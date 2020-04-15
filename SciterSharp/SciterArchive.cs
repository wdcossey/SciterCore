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
	public class SciterArchive
	{
		private static Sciter.SciterApi _api = Sciter.Api;
		private IntPtr _handle;
		private GCHandle _pinnedArray;

		private const string DEFAULT_URI = "archive://app/";
		private  Uri DEFAULT_URI_URI = new Uri(DEFAULT_URI);

		public Uri Uri { get; private set; }

		public bool IsOpen => _handle != IntPtr.Zero;

		public SciterArchive(string uri = DEFAULT_URI)
		{
			this.Uri = new Uri($"{uri}", UriKind.Absolute);
		}
		
		public SciterArchive(Uri baseUri)
		{
			this.Uri = baseUri;
		}

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

		public void Close()
		{
			ArchiveNotOpened();
			
			_api.SciterCloseArchive(_handle);
			_handle = IntPtr.Zero;
			_pinnedArray.Free();
		}

		public byte[] Get(string path)
		{
			ArchiveNotOpened();

			bool found = _api.SciterGetArchiveItem(_handle, path, out var dataPtr, out var dataLenth);

			if(!found)
			{
				return null;
			}

			byte[] res = new byte[dataLenth];
			Marshal.Copy(dataPtr, res, 0, (int) dataLenth);
			return res;
		}

#region Private

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
