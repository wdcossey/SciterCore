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
		private IntPtr _har;
		private GCHandle _pinnedArray;

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
			if (_har != IntPtr.Zero)
				throw new Exception("Archive already open.");

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
			if(_har != IntPtr.Zero)
				throw new Exception("Archive already open.");

			_pinnedArray = GCHandle.Alloc(res_array, GCHandleType.Pinned);
			_har = _api.SciterOpenArchive(_pinnedArray.AddrOfPinnedObject(), (uint) res_array.Length);
			Debug.Assert(_har != IntPtr.Zero);

			return this;
		}

		public void Close()
		{
			if(_har == IntPtr.Zero)
				throw new Exception("You haven't yet opened this archive.");
			
			_api.SciterCloseArchive(_har);
			_har = IntPtr.Zero;
			_pinnedArray.Free();
		}

		public byte[] Get(string path)
		{
			if(_har == IntPtr.Zero)
				throw new Exception("You haven't yet opened this archive.");

			IntPtr data_ptr;
			uint data_count;

			bool found = _api.SciterGetArchiveItem(_har, path, out data_ptr, out data_count);
			if(found == false)
				return null;

			byte[] res = new byte[data_count];
			Marshal.Copy(data_ptr, res, 0, (int) data_count);
			return res;
		}
	}
}
