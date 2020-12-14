// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo
namespace SciterCore.Interop
{
    public static partial class SciterRequest
    {
	    public enum REQUEST_RESULT
		{
			/// <summary>
			/// e.g. not enough memory
			/// </summary>
			REQUEST_PANIC = -1,
			
			/// <summary>
			/// 
			/// </summary>
			REQUEST_OK = 0,
			
			/// <summary>
			/// bad parameter
			/// </summary>
			REQUEST_BAD_PARAM = 1,
			
			/// <summary>
			/// Operation failed. <br/>
			/// e.g. index out of bounds
			/// </summary>
			REQUEST_FAILURE = 2,
			
			/// <summary>
			/// The platform does not support requested feature
			/// </summary>
			REQUEST_NOTSUPPORTED = 3
		}

		public enum REQUEST_RQ_TYPE : uint
		{
			/// <summary>
			/// 
			/// </summary>
			RRT_GET = 1,
			
			/// <summary>
			/// 
			/// </summary>
			RRT_POST = 2,
			
			/// <summary>
			/// 
			/// </summary>
			RRT_PUT = 3,
			
			/// <summary>
			/// 
			/// </summary>
			RRT_DELETE = 4,
			
			/// <summary>
			/// 
			/// </summary>
			RRT_FORCE_DWORD = 0xffffffff
		}

		public enum SciterResourceType : uint
		{
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_HTML = 0,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_IMAGE = 1,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_STYLE = 2,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_CURSOR = 3,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_SCRIPT = 4,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_RAW = 5,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_FONT,
			
			/// <summary>
			/// wav bytes
			/// </summary>
			RT_DATA_SOUND,
			
			/// <summary>
			/// 
			/// </summary>
			RT_DATA_FORCE_DWORD = 0xffffffff
		}

		public enum REQUEST_STATE : uint
		{
			/// <summary>
			/// 
			/// </summary>
			RS_PENDING = 0,
			
			/// <summary>
			/// Completed successfully
			/// </summary>
			RS_SUCCESS = 1,
			
			/// <summary>
			/// Completed with failure
			/// </summary>
			RS_FAILURE = 2,

			/// <summary>
			/// 
			/// </summary>
			RS_FORCE_DWORD = 0xffffffff
		}
    }
}