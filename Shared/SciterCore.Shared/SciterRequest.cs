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
using System.Runtime.InteropServices;

namespace SciterCore
{
	public class SciterRequest
	{
		private static readonly Interop.SciterRequest.SciterRequestApi RequestApi = Interop.Sciter.RequestApi;
		public readonly IntPtr _hrq;

		private SciterRequest() 
		{
			//
		}

		public SciterRequest(IntPtr hrq)
			: this()
		{
			_hrq = hrq;
		}

		public string Url
		{
			get
			{
				string result = null;
				RequestApi.RequestUrl(
					_hrq, 
					(IntPtr str, uint strLength, IntPtr param) =>
						{
							result = Marshal.PtrToStringAnsi(str, (int)strLength);
						}, 
					IntPtr.Zero);
				
				return result;
			}
		}

		public string ContentUrl
		{
			get
			{
				string result = null;

				RequestApi.RequestContentUrl(
					_hrq, 
					(IntPtr str, uint strLength, IntPtr param) =>
						{
							result = Marshal.PtrToStringAnsi(str, (int)strLength);
						},
					IntPtr.Zero);
				
				return result;
			}
		}

		public Interop.SciterRequest.SciterResourceType RequestedType
		{
			get
			{
				Interop.SciterRequest.SciterResourceType rv;
				RequestApi.RequestGetRequestedDataType(_hrq, out rv);
				return rv;
			}
		}

		public void Succeeded(uint status, byte[] dataOrNull = null)
		{
			RequestApi.RequestSetSucceeded(_hrq, status, dataOrNull, dataOrNull == null ? 0 : (uint)dataOrNull.Length);
		}

		public void Failed(uint status, byte[] dataOrNull = null)
		{
			RequestApi.RequestSetFailed(_hrq, status, dataOrNull, dataOrNull == null ? 0 : (uint)dataOrNull.Length);
		}

		public void AppendData(byte[] data)
		{
			RequestApi.RequestAppendDataChunk(_hrq, data, (uint)data.Length);
		}
	}
}