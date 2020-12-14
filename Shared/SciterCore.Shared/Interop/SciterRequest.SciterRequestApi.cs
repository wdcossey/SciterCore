using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
    public static partial class SciterRequest
    {
        [StructLayout(LayoutKind.Sequential)]
		internal readonly struct SciterRequestApi
		{
			public readonly SciterRequestApiDelegates.FPTR_RequestUse						RequestUse;
			public readonly SciterRequestApiDelegates.FPTR_RequestUnUse						RequestUnUse;
			public readonly SciterRequestApiDelegates.FPTR_RequestUrl						RequestUrl;
			public readonly SciterRequestApiDelegates.FPTR_RequestContentUrl				RequestContentUrl;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetRequestType			RequestGetRequestType;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetRequestedDataType		RequestGetRequestedDataType;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetReceivedDataType		RequestGetReceivedDataType;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfParameters		RequestGetNumberOfParameters;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthParameterName		RequestGetNthParameterName;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthParameterValue		RequestGetNthParameterValue;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetTimes					RequestGetTimes;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfRqHeaders		RequestGetNumberOfRqHeaders;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthRqHeaderName		RequestGetNthRqHeaderName;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthRqHeaderValue		RequestGetNthRqHeaderValue;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNumberOfRspHeaders		RequestGetNumberOfRspHeaders;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthRspHeaderName		RequestGetNthRspHeaderName;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetNthRspHeaderValue		RequestGetNthRspHeaderValue;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetCompletionStatus		RequestGetCompletionStatus;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetProxyHost				RequestGetProxyHost;
			public readonly SciterRequestApiDelegates.FPTR_RequestGetProxyPort				RequestGetProxyPort;
			public readonly SciterRequestApiDelegates.FPTR_RequestSetSucceeded				RequestSetSucceeded;
			public readonly SciterRequestApiDelegates.FPTR_RequestSetFailed					RequestSetFailed;
			public readonly SciterRequestApiDelegates.FPTR_RequestAppendDataChunk			RequestAppendDataChunk;
			public readonly SciterRequestApiDelegates.FPTR_RequestSetRqHeader				RequestSetRqHeader;
			public readonly SciterRequestApiDelegates.FPTR_RequestSetRspHeader				RequestSetRspHeader;
			
			public readonly SciterRequestApiDelegates.FPTR_RequestSetReceivedDataType		RequestSetReceivedDataType;
			public readonly SciterRequestApiDelegates.FPTR_RequestSetReceivedDataEncoding	RequestSetReceivedDataEncoding;
			
			public readonly SciterRequestApiDelegates.FPTR_RequestGetData					RequestGetData;
		}
    }
}