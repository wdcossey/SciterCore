using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
    public static partial class SciterRequest
    {
        internal static class SciterRequestApiDelegates
        {
            // a.k.a AddRef()
			// REQUEST_RESULT SCFN(RequestUse)( HREQUEST rq );
			[SciterStructMap(nameof(SciterRequestApi.RequestUse))]
			public delegate REQUEST_RESULT RequestUse(IntPtr rq);

			// a.k.a Release()
			// REQUEST_RESULT SCFN(RequestUnUse)( HREQUEST rq );
			[SciterStructMap(nameof(SciterRequestApi.RequestUnUse))]
			public delegate REQUEST_RESULT RequestUnUse(IntPtr rq);

			// get requested URL
			// REQUEST_RESULT SCFN(RequestUrl)( HREQUEST rq, LPCSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestUrl))]
			public delegate REQUEST_RESULT RequestUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcv_param);

			// get real, content URL (after possible redirection)
			// REQUEST_RESULT SCFN(RequestContentUrl)( HREQUEST rq, LPCSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestContentUrl))]
			public delegate REQUEST_RESULT RequestContentUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcv_param);

			// get requested data type
			// REQUEST_RESULT SCFN(RequestGetRequestType)( HREQUEST rq, REQUEST_RQ_TYPE* pType );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetRequestType))]
			public delegate REQUEST_RESULT RequestGetRequestType(IntPtr rq, out REQUEST_RQ_TYPE pType);

			// get requested data type
			// REQUEST_RESULT SCFN(RequestGetRequestedDataType)( HREQUEST rq, SciterResourceType* pData );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetRequestedDataType))]
			public delegate REQUEST_RESULT RequestGetRequestedDataType(IntPtr rq, out SciterResourceType pData);

			// get received data type, string, mime type
			// REQUEST_RESULT SCFN(RequestGetReceivedDataType)( HREQUEST rq, LPCSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetReceivedDataType))]
			public delegate REQUEST_RESULT RequestGetReceivedDataType(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcv_param);

			// get number of request parameters passed
			// REQUEST_RESULT SCFN(RequestGetNumberOfParameters)( HREQUEST rq, UINT* pNumber );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNumberOfParameters))]
			public delegate REQUEST_RESULT RequestGetNumberOfParameters(IntPtr rq, out uint pNumber);

			// get nth request parameter name
			// REQUEST_RESULT SCFN(RequestGetNthParameterName)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param  );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthParameterName))]
			public delegate REQUEST_RESULT RequestGetNthParameterName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get nth request parameter value
			// REQUEST_RESULT SCFN(RequestGetNthParameterValue)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param  );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthParameterValue))]
			public delegate REQUEST_RESULT RequestGetNthParameterValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get request times , ended - started = milliseconds to get the request
			// REQUEST_RESULT SCFN(RequestGetTimes)( HREQUEST rq, UINT* pStarted, UINT* pEnded );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetTimes))]
			public delegate REQUEST_RESULT RequestGetTimes(IntPtr rq, out uint pStarted, out uint pEnded);

			// get number of request headers
			// REQUEST_RESULT SCFN(RequestGetNumberOfRqHeaders)( HREQUEST rq, UINT* pNumber );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNumberOfRqHeaders))]
			public delegate REQUEST_RESULT RequestGetNumberOfRqHeaders(IntPtr rq, out uint pNumber);

			// get nth request header name 
			// REQUEST_RESULT SCFN(RequestGetNthRqHeaderName)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthRqHeaderName))]
			public delegate REQUEST_RESULT RequestGetNthRqHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get nth request header value 
			// REQUEST_RESULT SCFN(RequestGetNthRqHeaderValue)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthRqHeaderValue))]
			public delegate REQUEST_RESULT RequestGetNthRqHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get number of response headers
			// REQUEST_RESULT SCFN(RequestGetNumberOfRspHeaders)( HREQUEST rq, UINT* pNumber );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNumberOfRspHeaders))]
			public delegate REQUEST_RESULT RequestGetNumberOfRspHeaders(IntPtr rq, out uint pNumber);

			// get nth response header name 
			// REQUEST_RESULT SCFN(RequestGetNthRspHeaderName)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthRspHeaderName))]
			public delegate REQUEST_RESULT RequestGetNthRspHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get nth response header value 
			// REQUEST_RESULT SCFN(RequestGetNthRspHeaderValue)( HREQUEST rq, UINT n, LPCWSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetNthRspHeaderValue))]
			public delegate REQUEST_RESULT RequestGetNthRspHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcv_param);

			// get completion status (CompletionStatus - http response code : 200, 404, etc.)
			// REQUEST_RESULT SCFN(RequestGetCompletionStatus)( HREQUEST rq, REQUEST_STATE* pState, UINT* pCompletionStatus );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetCompletionStatus))]
			public delegate REQUEST_RESULT RequestGetCompletionStatus(IntPtr rq, out REQUEST_STATE pState, out uint pCompletionStatus);

			// get proxy host
			// REQUEST_RESULT SCFN(RequestGetProxyHost)( HREQUEST rq, LPCSTR_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetProxyHost))]
			public delegate REQUEST_RESULT RequestGetProxyHost(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcv_param);

			// get proxy port
			// REQUEST_RESULT SCFN(RequestGetProxyPort)( HREQUEST rq, UINT* pPort );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetProxyPort))]
			public delegate REQUEST_RESULT RequestGetProxyPort(IntPtr rq, out uint pPort);

			// mark request as complete with status and data 
			// REQUEST_RESULT SCFN(RequestSetSucceeded)( HREQUEST rq, UINT status, LPCBYTE dataOrNull, UINT dataLength);
			[SciterStructMap(nameof(SciterRequestApi.RequestSetSucceeded))]
			public delegate REQUEST_RESULT RequestSetSucceeded(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength);

			// mark request as complete with failure and optional data 
			// REQUEST_RESULT SCFN(RequestSetFailed)( HREQUEST rq, UINT status, LPCBYTE dataOrNull, UINT dataLength );
			[SciterStructMap(nameof(SciterRequestApi.RequestSetFailed))]
			public delegate REQUEST_RESULT RequestSetFailed(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength);

			// append received data chunk 
			// REQUEST_RESULT SCFN(RequestAppendDataChunk)( HREQUEST rq, LPCBYTE data, UINT dataLength );
			[SciterStructMap(nameof(SciterRequestApi.RequestAppendDataChunk))]
			public delegate REQUEST_RESULT RequestAppendDataChunk(IntPtr rq, byte[] data, uint dataLength);

			// set request header (single item)
			// REQUEST_RESULT SCFN(RequestSetRqHeader)( HREQUEST rq, LPCWSTR name, LPCWSTR value );
			[SciterStructMap(nameof(SciterRequestApi.RequestSetRqHeader))]
			public delegate REQUEST_RESULT RequestSetRqHeader(IntPtr rq, [MarshalAs(UnmanagedType.LPWStr)]string name, [MarshalAs(UnmanagedType.LPWStr)]string value);

			// set response header (single item)
			// REQUEST_RESULT SCFN(RequestSetRspHeader)( HREQUEST rq, LPCWSTR name, LPCWSTR value );
			[SciterStructMap(nameof(SciterRequestApi.RequestSetRspHeader))]
			public delegate REQUEST_RESULT RequestSetRspHeader(IntPtr rq, [MarshalAs(UnmanagedType.LPWStr)]string name, [MarshalAs(UnmanagedType.LPWStr)]string value);

			// set received data type, string, mime type
			// REQUEST_RESULT SCFN(RequestSetReceivedDataType)( HREQUEST rq, LPCSTR type );
			[SciterStructMap(nameof(SciterRequestApi.RequestSetReceivedDataType))]
			public delegate REQUEST_RESULT RequestSetReceivedDataType(IntPtr rq, [MarshalAs(UnmanagedType.LPStr)]string type);

			// set received data encoding, string
			// REQUEST_RESULT SCFN(RequestSetReceivedDataEncoding)( HREQUEST rq, LPCSTR encoding );
			[SciterStructMap(nameof(SciterRequestApi.RequestSetReceivedDataEncoding))]
			public delegate REQUEST_RESULT RequestSetReceivedDataEncoding(IntPtr rq, [MarshalAs(UnmanagedType.LPStr)]string encoding);

			// get received (so far) data
			// REQUEST_RESULT SCFN(RequestGetData)( HREQUEST rq, LPCBYTE_RECEIVER* rcv, LPVOID rcv_param );
			[SciterStructMap(nameof(SciterRequestApi.RequestGetData))]
			public delegate REQUEST_RESULT RequestGetData(IntPtr rq, SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcv_param);
        }
    }
}