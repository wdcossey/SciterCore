using System;

namespace SciterCore.Interop
{
    internal static class InteropExtensions
    {
        internal static LoadData Convert(this SciterXDef.SCN_LOAD_DATA loadData)
        {
            return new LoadData
            {
                Code = loadData.code,
                Data = loadData.outData,
                Handle = loadData.hwnd,
                Initiator = loadData.initiator,
                Principal = loadData.principal,
                Uri = new Uri(loadData.uri, UriKind.RelativeOrAbsolute),
                DataSize = loadData.outDataSize,
                DataType = (SciterResourceType)(long)loadData.dataType,
                RequestId = loadData.requestId
            };
        }
        
        internal static DataLoaded Convert(this SciterXDef.SCN_DATA_LOADED loadData)
        {
            return new DataLoaded
            {
                Code = loadData.code,
                Data = loadData.data,
                Handle = loadData.hwnd,
                Uri = loadData.uri,
                DataSize = loadData.dataSize,
                DataType = (SciterResourceType)(long)loadData.dataType,
                Status = loadData.status,
            };
        }
        
        internal static SciterXDef.SCN_DATA_LOADED Convert(this DataLoaded loadData)
        {
            return new SciterXDef.SCN_DATA_LOADED
            {
                code = loadData.Code,
                data = loadData.Data,
                hwnd = loadData.Handle,
                uri = loadData.Uri,
                dataSize = loadData.DataSize,
                dataType = (SciterRequest.SciterResourceType)(uint)loadData.DataType,
                status = loadData.Status,
            };
        }
    }
}