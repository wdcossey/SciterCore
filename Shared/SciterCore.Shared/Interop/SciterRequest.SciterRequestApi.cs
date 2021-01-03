using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
    public static partial class SciterRequest
    {
        [StructLayout(LayoutKind.Sequential)]
		internal readonly struct SciterRequestApi
		{
			public readonly SciterRequestApiDelegates.RequestUse						RequestUse;
			public readonly SciterRequestApiDelegates.RequestUnUse						RequestUnUse;
			public readonly SciterRequestApiDelegates.RequestUrl						RequestUrl;
			public readonly SciterRequestApiDelegates.RequestContentUrl				RequestContentUrl;
			public readonly SciterRequestApiDelegates.RequestGetRequestType			RequestGetRequestType;
			public readonly SciterRequestApiDelegates.RequestGetRequestedDataType		RequestGetRequestedDataType;
			public readonly SciterRequestApiDelegates.RequestGetReceivedDataType		RequestGetReceivedDataType;
			public readonly SciterRequestApiDelegates.RequestGetNumberOfParameters		RequestGetNumberOfParameters;
			public readonly SciterRequestApiDelegates.RequestGetNthParameterName		RequestGetNthParameterName;
			public readonly SciterRequestApiDelegates.RequestGetNthParameterValue		RequestGetNthParameterValue;
			public readonly SciterRequestApiDelegates.RequestGetTimes					RequestGetTimes;
			public readonly SciterRequestApiDelegates.RequestGetNumberOfRqHeaders		RequestGetNumberOfRqHeaders;
			public readonly SciterRequestApiDelegates.RequestGetNthRqHeaderName		RequestGetNthRqHeaderName;
			public readonly SciterRequestApiDelegates.RequestGetNthRqHeaderValue		RequestGetNthRqHeaderValue;
			public readonly SciterRequestApiDelegates.RequestGetNumberOfRspHeaders		RequestGetNumberOfRspHeaders;
			public readonly SciterRequestApiDelegates.RequestGetNthRspHeaderName		RequestGetNthRspHeaderName;
			public readonly SciterRequestApiDelegates.RequestGetNthRspHeaderValue		RequestGetNthRspHeaderValue;
			public readonly SciterRequestApiDelegates.RequestGetCompletionStatus		RequestGetCompletionStatus;
			public readonly SciterRequestApiDelegates.RequestGetProxyHost				RequestGetProxyHost;
			public readonly SciterRequestApiDelegates.RequestGetProxyPort				RequestGetProxyPort;
			public readonly SciterRequestApiDelegates.RequestSetSucceeded				RequestSetSucceeded;
			public readonly SciterRequestApiDelegates.RequestSetFailed					RequestSetFailed;
			public readonly SciterRequestApiDelegates.RequestAppendDataChunk			RequestAppendDataChunk;
			public readonly SciterRequestApiDelegates.RequestSetRqHeader				RequestSetRqHeader;
			public readonly SciterRequestApiDelegates.RequestSetRspHeader				RequestSetRspHeader;
			
			public readonly SciterRequestApiDelegates.RequestSetReceivedDataType		RequestSetReceivedDataType;
			public readonly SciterRequestApiDelegates.RequestSetReceivedDataEncoding	RequestSetReceivedDataEncoding;
			
			public readonly SciterRequestApiDelegates.RequestGetData					RequestGetData;
		}
    }
}