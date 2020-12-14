using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
	public static partial class SciterRequest
	{
		internal static class UnsafeNativeMethods
		{
			public static ISciterRequestApi GetApiInterface(ISciterApi sciterApi)
			{
				var sciterRequestApi = sciterApi.GetSciterRequestAPI();
				
				return new NativeSciterRequestApiWrapper<SciterRequestApi>(sciterRequestApi);
			}

			private sealed class NativeSciterRequestApiWrapper<TStruct> : ISciterRequestApi
				where TStruct : struct
			{
				private IntPtr _apiPtr;

#pragma warning disable 649
				
				private readonly SciterRequestApiDelegates.FPTR_RequestUse						_requestUse;
				private readonly SciterRequestApiDelegates.FPTR_RequestUnUse					_requestUnUse;
				private readonly SciterRequestApiDelegates.FPTR_RequestUrl						_requestUrl;
				private readonly SciterRequestApiDelegates.FPTR_RequestContentUrl				_requestContentUrl;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetRequestType			_requestGetRequestType;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetRequestedDataType		_requestGetRequestedDataType;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetReceivedDataType		_requestGetReceivedDataType;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfParameters	_requestGetNumberOfParameters;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthParameterName		_requestGetNthParameterName;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthParameterValue		_requestGetNthParameterValue;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetTimes					_requestGetTimes;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfRqHeaders		_requestGetNumberOfRqHeaders;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthRqHeaderName		_requestGetNthRqHeaderName;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthRqHeaderValue		_requestGetNthRqHeaderValue;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfRspHeaders	_requestGetNumberOfRspHeaders;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthRspHeaderName		_requestGetNthRspHeaderName;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetNthRspHeaderValue		_requestGetNthRspHeaderValue;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetCompletionStatus		_requestGetCompletionStatus;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetProxyHost				_requestGetProxyHost;
				private readonly SciterRequestApiDelegates.FPTR_RequestGetProxyPort				_requestGetProxyPort;
				private readonly SciterRequestApiDelegates.FPTR_RequestSetSucceeded				_requestSetSucceeded;
				private readonly SciterRequestApiDelegates.FPTR_RequestSetFailed				_requestSetFailed;
				private readonly SciterRequestApiDelegates.FPTR_RequestAppendDataChunk			_requestAppendDataChunk;
				private readonly SciterRequestApiDelegates.FPTR_RequestSetRqHeader				_requestSetRqHeader;
				private readonly SciterRequestApiDelegates.FPTR_RequestSetRspHeader				_requestSetRspHeader;
				
				private readonly SciterRequestApiDelegates.FPTR_RequestSetReceivedDataType		_requestSetReceivedDataType;
				private readonly SciterRequestApiDelegates.FPTR_RequestSetReceivedDataEncoding	_requestSetReceivedDataEncoding;
				
				private readonly SciterRequestApiDelegates.FPTR_RequestGetData					_requestGetData;

#pragma warning restore 649

				internal NativeSciterRequestApiWrapper(IntPtr apiPtr)
				{
					_apiPtr = apiPtr;
					var @struct = Marshal.PtrToStructure<TStruct>(apiPtr);

					var fieldInfoDictionary = GetType()
						.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
						.Where(w => w.FieldType.GetCustomAttribute<SciterStructMapAttribute>() != null)
						.ToDictionary(key => key.FieldType.GetCustomAttribute<SciterStructMapAttribute>()?.Name,
							value => value);

					var fieldInfos = @struct.GetType().GetFields();
					foreach (var fieldInfo in fieldInfos)
					{
						if (!fieldInfoDictionary.ContainsKey(fieldInfo.Name))
							continue;
						fieldInfoDictionary[fieldInfo.Name].SetValue(this, fieldInfo.GetValue(@struct));
					}
				}

				public REQUEST_RESULT RequestUse(IntPtr rq) =>
					_requestUse(rq);

				public REQUEST_RESULT RequestUnUse(IntPtr rq) =>
					_requestUnUse(rq);

				public REQUEST_RESULT RequestUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) =>
					_requestUrl(rq, rcv, rcvParam);

				public REQUEST_RESULT RequestContentUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) =>
					_requestContentUrl(rq, rcv, rcvParam);

				public REQUEST_RESULT RequestGetRequestType(IntPtr rq, out REQUEST_RQ_TYPE pType) => 
					_requestGetRequestType(rq, out pType);

				public REQUEST_RESULT RequestGetRequestedDataType(IntPtr rq, out SciterResourceType pData) => 
					_requestGetRequestedDataType(rq, out pData);

				public REQUEST_RESULT RequestGetReceivedDataType(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetReceivedDataType(rq, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNumberOfParameters(IntPtr rq, out uint pNumber) => 
					_requestGetNumberOfParameters(rq, out pNumber);

				public REQUEST_RESULT RequestGetNthParameterName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthParameterName(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNthParameterValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthParameterValue(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetTimes(IntPtr rq, out uint pStarted, out uint pEnded) => 
					_requestGetTimes(rq, out pStarted, out pEnded);

				public REQUEST_RESULT RequestGetNumberOfRqHeaders(IntPtr rq, out uint pNumber) => 
					_requestGetNumberOfRqHeaders(rq, out pNumber);

				public REQUEST_RESULT RequestGetNthRqHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthRqHeaderName(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNthRqHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthRqHeaderValue(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNthRspHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthRspHeaderName(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNthRspHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetNthRspHeaderValue(rq, n, rcv, rcvParam);

				public REQUEST_RESULT RequestGetNumberOfRspHeaders(IntPtr rq, out uint pNumber) => 
					_requestGetNumberOfRspHeaders(rq, out pNumber);

				public REQUEST_RESULT RequestGetCompletionStatus(IntPtr rq, out REQUEST_STATE pState, out uint pCompletionStatus) => 
					_requestGetCompletionStatus(rq, out pState, out pCompletionStatus);

				public REQUEST_RESULT RequestGetProxyHost(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetProxyHost(rq, rcv, rcvParam);

				public REQUEST_RESULT RequestGetProxyPort(IntPtr rq, out uint pPort) => 
					_requestGetProxyPort(rq, out pPort);

				public REQUEST_RESULT RequestSetSucceeded(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength) => 
					_requestSetSucceeded(rq, status, dataOrNull, dataLength);

				public REQUEST_RESULT RequestSetFailed(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength) => 
					_requestSetFailed(rq, status, dataOrNull, dataLength);

				public REQUEST_RESULT RequestAppendDataChunk(IntPtr rq, byte[] data, uint dataLength) => 
					_requestAppendDataChunk(rq, data, dataLength);

				public REQUEST_RESULT RequestSetRqHeader(IntPtr rq, string name, string value) => 
					_requestSetRqHeader(rq, name, value);

				public REQUEST_RESULT RequestSetRspHeader(IntPtr rq, string name, string value) => 
					_requestSetRspHeader(rq, name, value);

				public REQUEST_RESULT RequestSetReceivedDataType(IntPtr rq, string type) =>
					_requestSetReceivedDataType(rq, type);

				public REQUEST_RESULT RequestSetReceivedDataEncoding(IntPtr rq, string encoding) =>
					_requestSetReceivedDataEncoding(rq, encoding);

				public REQUEST_RESULT RequestGetData(IntPtr rq, SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam) => 
					_requestGetData(rq, rcv, rcvParam);
			}
		}
	}
}