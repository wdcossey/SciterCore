﻿// Copyright 2016 Ramon F. Mendes
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
		private static Interop.SciterRequest.SciterRequestApi _requestApi = Interop.Sciter.RequestApi;
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
				string strval = null;
				Interop.SciterXDom.LPCSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
				{
					strval = Marshal.PtrToStringAnsi(str, (int)str_length);
				};

				_requestApi.RequestUrl(_hrq, frcv, IntPtr.Zero);
				return strval;
			}
		}

		public string ContentUrl
		{
			get
			{
				string strval = null;
				Interop.SciterXDom.LPCSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
				{
					strval = Marshal.PtrToStringAnsi(str, (int)str_length);
				};

				_requestApi.RequestContentUrl(_hrq, frcv, IntPtr.Zero);
				return strval;
			}
		}

		public Interop.SciterRequest.SciterResourceType RequestedType
		{
			get
			{
				Interop.SciterRequest.SciterResourceType rv;
				_requestApi.RequestGetRequestedDataType(_hrq, out rv);
				return rv;
			}
		}

		public void Suceeded(uint status, byte[] dataOrNull = null)
		{
			_requestApi.RequestSetSucceeded(_hrq, status, dataOrNull, dataOrNull == null ? 0 : (uint)dataOrNull.Length);
		}

		public void Failed(uint status, byte[] dataOrNull = null)
		{
			_requestApi.RequestSetFailed(_hrq, status, dataOrNull, dataOrNull == null ? 0 : (uint)dataOrNull.Length);
		}

		public void AppendData(byte[] data)
		{
			_requestApi.RequestAppendDataChunk(_hrq, data, (uint)data.Length);
		}
	}
}