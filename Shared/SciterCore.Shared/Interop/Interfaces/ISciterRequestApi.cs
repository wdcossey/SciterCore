using System;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
    public interface ISciterRequestApi
    {
        /// <summary>
        /// a.k.a AddRef()
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestUse(IntPtr rq);
        
        /// <summary>
        /// a.k.a Release()
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT RequestUnUse(IntPtr rq);
        
        /// <summary>
        /// Get requested URL
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get real, content URL (after possible redirection)
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestContentUrl(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get requested data type
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetRequestType(IntPtr rq, out SciterRequest.REQUEST_RQ_TYPE pType);
        
        /// <summary>
        /// Get requested data type
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pData"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT RequestGetRequestedDataType(IntPtr rq, out SciterRequest.SciterResourceType pData);
        
        /// <summary>
        /// Get received data type, string, mime type
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetReceivedDataType(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get number of request parameters passed
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pNumber"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT RequestGetNumberOfParameters(IntPtr rq, out uint pNumber);
        
        /// <summary>
        /// Get nth request parameter name
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthParameterName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get nth request parameter name
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthParameterValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get nth request parameter value
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pStarted"></param>
        /// <param name="pEnded"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetTimes(IntPtr rq, out uint pStarted, out uint pEnded);
        
        /// <summary>
        /// Get number of request headers
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pNumber"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNumberOfRqHeaders(IntPtr rq, out uint pNumber);
        
        /// <summary>
        /// Get nth request header name 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthRqHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get nth request header value 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthRqHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
         
        /// <summary>
        /// Get nth response header name 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthRspHeaderName(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get nth response header value  
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="n"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetNthRspHeaderValue(IntPtr rq, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get number of response headers
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pNumber"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT RequestGetNumberOfRspHeaders(IntPtr rq, out uint pNumber);

        /// <summary>
        /// Get completion status (CompletionStatus - http response code : 200, 404, etc.)
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pState"></param>
        /// <param name="pCompletionStatus"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetCompletionStatus(IntPtr rq, out SciterRequest.REQUEST_STATE pState, out uint pCompletionStatus);
        
        /// <summary>
        /// Get proxy host
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetProxyHost(IntPtr rq, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
        
        /// <summary>
        /// Get proxy port
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="pPort"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetProxyPort(IntPtr rq, out uint pPort);
        
        /// <summary>
        /// Mark request as complete with status and data 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="status"></param>
        /// <param name="dataOrNull"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestSetSucceeded(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength);
        
        /// <summary>
        /// Mark request as complete with failure and optional data 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="status"></param>
        /// <param name="dataOrNull"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT RequestSetFailed(IntPtr rq, uint status, byte[] dataOrNull, uint dataLength);
        
        /// <summary>
        /// Append received data chunk 
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestAppendDataChunk(IntPtr rq, byte[] data, uint dataLength);
        
        /// <summary>
        /// Set request header (single item)
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestSetRqHeader(IntPtr rq, string name, string value);
        
        /// <summary>
        /// Set response header (single item)
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestSetRspHeader(IntPtr rq, string name, string value);
        
        /// <summary>
        /// Set received data type, string, mime type
        /// </summary>
        SciterRequest.REQUEST_RESULT RequestSetReceivedDataType(IntPtr rq, string type);
        
        /// <summary>
        /// Set received data encoding, string
        /// </summary>
        SciterRequest.REQUEST_RESULT RequestSetReceivedDataEncoding(IntPtr rq, string encoding);
        
        /// <summary>
        /// Get received (so far) data
        /// </summary>
        /// <param name="rq"></param>
        /// <param name="rcv"></param>
        /// <param name="rcvParam"></param>
        /// <returns></returns>
        public SciterRequest.REQUEST_RESULT	RequestGetData(IntPtr rq, SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam);
    }
}