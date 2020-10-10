namespace SciterCore
{
    public readonly struct ArchiveGetItemResult
    {
        public byte[] Data { get; }

        public int Size => Data?.Length ?? 0;
        
        public string Path { get; }
        
        public bool IsSuccessful { get; }
        
        internal ArchiveGetItemResult(byte[] data, string path, bool isSuccessful)
        {
            Data = data;
            Path = path;
            IsSuccessful = isSuccessful;
        }
        
    }
}