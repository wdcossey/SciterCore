namespace SciterCore
{
    public enum SciterResourceType : long
    {
        Html = 0,
        Image = 1,
        Style = 2,
        Cursor = 3,
        Script = 4,
        Raw = 5,
        Font,
        Sound, // wav bytes
        ForceDword = 0xffffffff
    }
}