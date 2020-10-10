namespace SciterCore
{
    public enum SciterResourceType : int
    {
        Html = 0,
        Image = 1,
        Style = 2,
        Cursor = 3,
        Script = 4,
        Raw = 5,
        Font,
        Sound, // wav bytes
        ForceDword = unchecked((int)0xffffffff)
    }
}