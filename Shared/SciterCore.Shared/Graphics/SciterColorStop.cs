using System;
using System.Drawing;

namespace SciterCore
{
    public readonly struct SciterColorStop
    {
        public SciterColor Color { get; }

        /// <summary>
        /// Offset of the gradient, between 0.0 ... 1.0
        /// </summary>
        public float Offset { get; }

        internal SciterColorStop(float offset, SciterColor color)
        {
            Offset = offset;
            Color = color;
        }
        
        internal SciterColorStop(float offset, byte r, byte g, byte b, byte alpha)
        {
            Offset = offset;
            Color = SciterColor.Create(r: r, g: g, b: b, alpha:alpha);
        }
        
        internal SciterColorStop(float offset, byte r, byte g, byte b, float alpha = 1f)
            : this(offset: offset, r, g, b, alpha: (byte)(Math.Min(Math.Max(alpha, 0f), 1f) * byte.MaxValue))
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Gradient offset, between 0.0f and 1.0f</param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static SciterColorStop Create(float offset, byte r, byte g, byte b)
        {
            return new SciterColorStop(offset: offset, r, g, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Gradient offset, between 0.0f and 1.0f</param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="alpha">Alpha, between 0.0f and 1.0f</param>
        /// <returns></returns>
        public static SciterColorStop Create(float offset, byte r, byte g, byte b, float alpha = 1f)
        {
            return new SciterColorStop(offset: offset, r, g, b, alpha: alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Gradient offset, between 0.0f and 1.0f</param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="alpha">Alpha, between 0 and 255</param>
        /// <returns></returns>
        public static SciterColorStop Create(float offset, byte r, byte g, byte b, byte alpha = byte.MaxValue)
        {
            return new SciterColorStop(offset: offset, r, g, b, alpha: alpha);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Gradient offset, between 0.0f and 1.0f</param>
        /// <param name="color"><see cref="Color"/> value</param>
        /// <returns></returns>
        public static SciterColorStop Create(float offset, Color color)
        {
            return new SciterColorStop(offset, color.R, color.G, color.B, color.A);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">Gradient offset, between 0.0f and 1.0f</param>
        /// <param name="color"><see cref="SciterColor"/> value</param>
        /// <returns></returns>
        public static SciterColorStop Create(float offset, SciterColor color)
        {
            return new SciterColorStop(offset, color);
        }
    }
}