using System;
#if WINDOWS
using System.Drawing;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
#endif

namespace SciterCore
{
    public struct RGBAColor
    {
        private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;
        private readonly uint _value;

        public uint Value => _value;

        public byte R => (byte) (_value & 0xFF);
        public byte G => (byte) ((_value >> 8) & 0xFF);
        public byte B => (byte) ((_value >> 16) & 0xFF);
        public byte A => (byte) ((_value >> 24) & 0xFF);

        public RGBAColor(int r, int g, int b, double a = 1d)
            : this (r, g, b, (int)(Math.Min(Math.Max(a, 0d), 1d) * byte.MaxValue))
        {

        }

        public RGBAColor(int r, int g, int b, int a)
        {
            _value = GraphicsApi.RGBA((uint)GetMinMaxValue(r), (uint)GetMinMaxValue(g), (uint)GetMinMaxValue(b), (uint)GetMinMaxValue(a));
        }

        public RGBAColor(uint value)
        {
            _value = value;
        }

        public static RGBAColor White = new RGBAColor(255, 255, 255);
        public static RGBAColor Black = new RGBAColor(0, 0, 0);
        public static RGBAColor Invalid = new RGBAColor(-1, -1, -1);

#if WINDOWS
        public static RGBAColor FromColor(Color color)
        {
            return new RGBAColor(color.R, color.G, color.B, color.A);
        }

        // ReSharper disable once InconsistentNaming
        public static uint ToRGBAColor(Color color)
        {
            return GraphicsApi.RGBA(color.R, color.G, color.B, color.A);
        }
#endif

        private static int GetMinMaxValue(int value)
        {
            // ReSharper disable once RedundantCast
            return (int)Math.Min(Math.Max(value, -1), byte.MaxValue);
        }
    }
}