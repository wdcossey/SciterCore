namespace SciterCore
{
    public readonly struct SciterRectangle
    {
        public SciterRectangle(int left, int top, int right, int bottom)
            : this(right: right, bottom: bottom)
        {
            Left = left;
            Top = top;
        }

        public SciterRectangle(int right, int bottom)
        {
            Left = 0;
            Top = 0;
            Right = right;
            Bottom = bottom;
        }

        public int Left { get; }
        
        public int Top { get; }
        
        public int Right { get; }
        
        public int Bottom { get; }

        public int Width => Right - Left;

        public int Height => Bottom - Top;
    }
}