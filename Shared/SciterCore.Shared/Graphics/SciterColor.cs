using System;
#if WINDOWS
using System.Drawing;
#endif

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace SciterCore
{
    public readonly struct SciterColor
    {
        private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;
        private readonly uint _value;

        public uint Value => _value;

        public byte R => (byte) (_value & 0xFF);
        public byte G => (byte) ((_value >> 8) & 0xFF);
        public byte B => (byte) ((_value >> 16) & 0xFF);
        public byte A => (byte) ((_value >> 24) & 0xFF);

        internal SciterColor(uint value)
        {
            _value = value;
        }

        internal SciterColor(int r, int g, int b)
            : this(r: r, g:g, b:b, 1d)
        {
           
        }

        internal SciterColor(int r, int g, int b, int a)
        {
            _value = GraphicsApi.RGBA((uint)GetMinMaxValue(r), (uint)GetMinMaxValue(g), (uint)GetMinMaxValue(b), (uint)GetMinMaxValue(a));
        }

        internal SciterColor(int r, int g, int b, double a = 1d)
            : this(r, g, b, (int)(Math.Min(Math.Max(a, 0d), 1d) * byte.MaxValue))
        {
            
        }
        
        public static SciterColor Create(uint value)
        {
            return new SciterColor(value: value);
        }
        
        public static SciterColor Create(int r, int g, int b)
        {
            return new SciterColor(r, g, b);
        }
        
        public static SciterColor Create(int r, int g, int b, double a = 1d)
        {
            return new SciterColor(r, g, b, a);
        }

        public static SciterColor Create(int r, int g, int b, int a = byte.MaxValue)
        {
            return new SciterColor(r, g, b, a);
        }

#if WINDOWS
        public static SciterColor Create(Color color)
        {
            return new SciterColor(color.R, color.G, color.B, color.A);
        }

        // ReSharper disable once InconsistentNaming
        //public static uint ToSciterColor(Color color)
        //{
        //    return GraphicsApi.RGBA(color.R, color.G, color.B, color.A);
        //}
#endif

        private static int GetMinMaxValue(int value)
        {
            // ReSharper disable once RedundantCast
            return (int)Math.Min(Math.Max(value, -1), byte.MaxValue);
        }
    }
}