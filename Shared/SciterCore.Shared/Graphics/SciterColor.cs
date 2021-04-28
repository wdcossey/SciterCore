using System;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SciterCore.Interop;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToAutoPropertyWhenPossible
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
    public readonly struct SciterColor : IConvertible
    {
        private static readonly ISciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;
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
        
        internal SciterColor(byte r, byte g, byte b, byte alpha)
        {
            _value = GraphicsApi.RGBA((uint)GetMinMaxValue(r), (uint)GetMinMaxValue(g), (uint)GetMinMaxValue(b), (uint)GetMinMaxValue(alpha));
        }

        internal SciterColor(byte r, byte g, byte b, float alpha = 1f)
            : this(r, g, b, (byte)(Math.Min(Math.Max(alpha, 0f), 1f) * byte.MaxValue))
        {
            
        }
        
        #region Create
        
        public static SciterColor Create(uint value)
        {
            return new SciterColor(value: value);
        }

        public static SciterColor Create(byte r, byte g, byte b)
        {
            return new SciterColor(r, g, b);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="alpha">Range between 0.0f and 1.0f</param>
        /// <returns></returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static SciterColor Create(byte r, byte g, byte b, float alpha = 1f)
        {
            return new SciterColor(r, g, b, alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="alpha">Range between 0 and 255</param>
        /// <returns></returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static SciterColor Create(byte r, byte g, byte b, byte alpha = byte.MaxValue)
        {
            return new SciterColor(r, g, b, alpha);
        }
        
        public static SciterColor Create(Color color)
        {
            return new SciterColor(color.R, color.G, color.B, color.A);
        }
        
        #endregion

        #region Parse

        public static SciterColor Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                return Transparent;

            var hexValue = value;

            if (hexValue[0] == '#')
                hexValue = hexValue.Substring(1);

            var r = (byte) 0;
            var g = (byte) 0;
            var b = (byte) 0;
            var a = byte.MaxValue;
            
            switch (hexValue.Length)
            {
                case 3:
                case 4:
                    r = byte.Parse($"{hexValue.Substring(0, 1)}{hexValue.Substring(0, 1)}", System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse($"{hexValue.Substring(1, 1)}{hexValue.Substring(1, 1)}", System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse($"{hexValue.Substring(2, 1)}{hexValue.Substring(2, 1)}", System.Globalization.NumberStyles.HexNumber);
                    
                    if (hexValue.Length == 4)
                        a = byte.Parse($"{hexValue.Substring(3, 1)}{hexValue.Substring(3, 1)}", System.Globalization.NumberStyles.HexNumber);
                    break;
                
                case 6:
                case 8:
                    r = byte.Parse(hexValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(hexValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(hexValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    
                    if (hexValue.Length == 8)
                        a = byte.Parse(hexValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    break;
            }

            return new SciterColor(r, g, b, a);
        }

        #endregion
        
        public SciterColor WithAlpha(byte alpha)
        {
            return new SciterColor(this.R, this.G, this.B, alpha);
        }
        
        public SciterColor WithAlpha(float alpha)
        {
            return new SciterColor(this.R, this.G, this.B, alpha);
        }

        // ReSharper disable once InconsistentNaming
        //public static uint ToSciterColor(Color color)
        //{
        //    return GraphicsApi.RGBA(color.R, color.G, color.B, color.A);
        //}

        private static byte GetMinMaxValue(byte value)
        {
            // ReSharper disable once RedundantCast
            return (byte)Math.Min(Math.Max(value, (byte)0), byte.MaxValue);
        }

        public override bool Equals(object obj)
        {
            if (obj is SciterColor color)
                return this.Value.Equals(color.Value);

            return false;
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(SciterColor other)
        {
            return Value == other.Value;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return unchecked((int) Value);
        }
        
        public override string ToString()
        {
            return $"{A:X2}{R:X2}{G:X2}{B:X2}";
        }
        
        public string ToHtmlColor()
        {
            return $"#{ToString()}".ToLowerInvariant();
        }
        
        public string ToShortHtmlColor()
        {
            return $"#{ToString().Substring(2)}".ToLowerInvariant();
        }

        #region Known Colors
        [ExcludeFromCodeCoverage]
        public static SciterColor Transparent => new SciterColor(0, 0, 0, 0f);
        [ExcludeFromCodeCoverage]
        public static SciterColor AliceBlue => new SciterColor(240, 248, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightSalmon => new SciterColor(255, 160, 122);
        [ExcludeFromCodeCoverage]
        public static SciterColor AntiqueWhite => new SciterColor(250, 235, 215);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightSeaGreen => new SciterColor(32, 178, 170);
        [ExcludeFromCodeCoverage]
        public static SciterColor Aqua => new SciterColor(0, 255, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightSkyBlue => new SciterColor(135, 206, 250);
        [ExcludeFromCodeCoverage]
        public static SciterColor Aquamarine => new SciterColor(127, 255, 212);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightSlateGray => new SciterColor(119, 136, 153);
        [ExcludeFromCodeCoverage]
        public static SciterColor Azure => new SciterColor(240, 255, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightSteelBlue => new SciterColor(176, 196, 222);
        [ExcludeFromCodeCoverage]
        public static SciterColor Beige => new SciterColor(245, 245, 220);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightYellow => new SciterColor(255, 255, 224);
        [ExcludeFromCodeCoverage]
        public static SciterColor Bisque => new SciterColor(255, 228, 196);
        [ExcludeFromCodeCoverage]
        public static SciterColor Lime => new SciterColor(0, 255, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Black => new SciterColor(0, 0, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor LimeGreen => new SciterColor(50, 205, 50);
        [ExcludeFromCodeCoverage]
        public static SciterColor BlanchedAlmond => new SciterColor(255, 255, 205);
        [ExcludeFromCodeCoverage]
        public static SciterColor Linen => new SciterColor(250, 240, 230);
        [ExcludeFromCodeCoverage]
        public static SciterColor Blue => new SciterColor(0, 0, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor Magenta => new SciterColor(255, 0, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor BlueViolet => new SciterColor(138, 43, 226);
        [ExcludeFromCodeCoverage]
        public static SciterColor Maroon => new SciterColor(128, 0, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Brown => new SciterColor(165, 42, 42);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumAquamarine => new SciterColor(102, 205, 170);
        [ExcludeFromCodeCoverage]
        public static SciterColor BurlyWood => new SciterColor(222, 184, 135);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumBlue => new SciterColor(0, 0, 205);
        [ExcludeFromCodeCoverage]
        public static SciterColor CadetBlue => new SciterColor(95, 158, 160);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumOrchid => new SciterColor(186, 85, 211);
        [ExcludeFromCodeCoverage]
        public static SciterColor Chartreuse => new SciterColor(127, 255, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumPurple => new SciterColor(147, 112, 219);
        [ExcludeFromCodeCoverage]
        public static SciterColor Chocolate => new SciterColor(210, 105, 30);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumSeaGreen => new SciterColor(60, 179, 113);
        [ExcludeFromCodeCoverage]
        public static SciterColor Coral => new SciterColor(255, 127, 80);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumSlateBlue => new SciterColor(123, 104, 238);
        [ExcludeFromCodeCoverage]
        public static SciterColor CornflowerBlue => new SciterColor(100, 149, 237);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumSpringGreen => new SciterColor(0, 250, 154);
        [ExcludeFromCodeCoverage]
        public static SciterColor Cornsilk => new SciterColor(255, 248, 220);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumTurquoise => new SciterColor(72, 209, 204);
        [ExcludeFromCodeCoverage]
        public static SciterColor Crimson => new SciterColor(220, 20, 60);
        [ExcludeFromCodeCoverage]
        public static SciterColor MediumVioletRed => new SciterColor(199, 21, 112);
        [ExcludeFromCodeCoverage]
        public static SciterColor Cyan => new SciterColor(0, 255, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor MidnightBlue => new SciterColor(25, 25, 112);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkBlue => new SciterColor(0, 0, 139);
        [ExcludeFromCodeCoverage]
        public static SciterColor MintCream => new SciterColor(245, 255, 250);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkCyan => new SciterColor(0, 139, 139);
        [ExcludeFromCodeCoverage]
        public static SciterColor MistyRose => new SciterColor(255, 228, 225);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkGoldenrod => new SciterColor(184, 134, 11);
        [ExcludeFromCodeCoverage]
        public static SciterColor Moccasin => new SciterColor(255, 228, 181);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkGray => new SciterColor(169, 169, 169);
        [ExcludeFromCodeCoverage]
        public static SciterColor NavajoWhite => new SciterColor(255, 222, 173);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkGreen => new SciterColor(0, 100, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Navy => new SciterColor(0, 0, 128);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkKhaki => new SciterColor(189, 183, 107);
        [ExcludeFromCodeCoverage]
        public static SciterColor OldLace => new SciterColor(253, 245, 230);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkMagenta => new SciterColor(139, 0, 139);
        [ExcludeFromCodeCoverage]
        public static SciterColor Olive => new SciterColor(128, 128, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkOliveGreen => new SciterColor(85, 107, 47);
        [ExcludeFromCodeCoverage]
        public static SciterColor OliveDrab => new SciterColor(107, 142, 45);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkOrange => new SciterColor(255, 140, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Orange => new SciterColor(255, 165, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkOrchid => new SciterColor(153, 50, 204);
        [ExcludeFromCodeCoverage]
        public static SciterColor OrangeRed => new SciterColor(255, 69, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkRed => new SciterColor(139, 0, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Orchid => new SciterColor(218, 112, 214);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkSalmon => new SciterColor(233, 150, 122);
        [ExcludeFromCodeCoverage]
        public static SciterColor PaleGoldenrod => new SciterColor(238, 232, 170);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkSeaGreen => new SciterColor(143, 188, 143);
        [ExcludeFromCodeCoverage]
        public static SciterColor PaleGreen => new SciterColor(152, 251, 152);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkSlateBlue => new SciterColor(72, 61, 139);
        [ExcludeFromCodeCoverage]
        public static SciterColor PaleTurquoise => new SciterColor(175, 238, 238);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkSlateGray => new SciterColor(40, 79, 79);
        [ExcludeFromCodeCoverage]
        public static SciterColor PaleVioletRed => new SciterColor(219, 112, 147);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkTurquoise => new SciterColor(0, 206, 209);
        [ExcludeFromCodeCoverage]
        public static SciterColor PapayaWhip => new SciterColor(255, 239, 213);
        [ExcludeFromCodeCoverage]
        public static SciterColor DarkViolet => new SciterColor(148, 0, 211);
        [ExcludeFromCodeCoverage]
        public static SciterColor PeachPuff => new SciterColor(255, 218, 155);
        [ExcludeFromCodeCoverage]
        public static SciterColor DeepPink => new SciterColor(255, 20, 147);
        [ExcludeFromCodeCoverage]
        public static SciterColor Peru => new SciterColor(205, 133, 63);
        [ExcludeFromCodeCoverage]
        public static SciterColor DeepSkyBlue => new SciterColor(0, 191, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor Pink => new SciterColor(255, 192, 203);
        [ExcludeFromCodeCoverage]
        public static SciterColor DimGray => new SciterColor(105, 105, 105);
        [ExcludeFromCodeCoverage]
        public static SciterColor Plum => new SciterColor(221, 160, 221);
        [ExcludeFromCodeCoverage]
        public static SciterColor DodgerBlue => new SciterColor(30, 144, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor PowderBlue => new SciterColor(176, 224, 230);
        [ExcludeFromCodeCoverage]
        public static SciterColor Firebrick => new SciterColor(178, 34, 34);
        [ExcludeFromCodeCoverage]
        public static SciterColor Purple => new SciterColor(128, 0, 128);
        [ExcludeFromCodeCoverage]
        public static SciterColor FloralWhite => new SciterColor(255, 250, 240);
        [ExcludeFromCodeCoverage]
        public static SciterColor Red => new SciterColor(255, 0, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor ForestGreen => new SciterColor(34, 139, 34);
        [ExcludeFromCodeCoverage]
        public static SciterColor RosyBrown => new SciterColor(188, 143, 143);
        [ExcludeFromCodeCoverage]
        public static SciterColor Fuchsia => new SciterColor(255, 0, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor RoyalBlue => new SciterColor(65, 105, 225);
        [ExcludeFromCodeCoverage]
        public static SciterColor Gainsboro => new SciterColor(220, 220, 220);
        [ExcludeFromCodeCoverage]
        public static SciterColor SaddleBrown => new SciterColor(139, 69, 19);
        [ExcludeFromCodeCoverage]
        public static SciterColor GhostWhite => new SciterColor(248, 248, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor Salmon => new SciterColor(250, 128, 114);
        [ExcludeFromCodeCoverage]
        public static SciterColor Gold => new SciterColor(255, 215, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor SandyBrown => new SciterColor(244, 164, 96);
        [ExcludeFromCodeCoverage]
        public static SciterColor Goldenrod => new SciterColor(218, 165, 32);
        [ExcludeFromCodeCoverage]
        public static SciterColor SeaGreen => new SciterColor(46, 139, 87);
        [ExcludeFromCodeCoverage]
        public static SciterColor Gray => new SciterColor(128, 128, 128);
        [ExcludeFromCodeCoverage]
        public static SciterColor Seashell => new SciterColor(255, 245, 238);
        [ExcludeFromCodeCoverage]
        public static SciterColor Green => new SciterColor(0, 128, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Sienna => new SciterColor(160, 82, 45);
        [ExcludeFromCodeCoverage]
        public static SciterColor GreenYellow => new SciterColor(173, 255, 47);
        [ExcludeFromCodeCoverage]
        public static SciterColor Silver => new SciterColor(192, 192, 192);
        [ExcludeFromCodeCoverage]
        public static SciterColor Honeydew => new SciterColor(240, 255, 240);
        [ExcludeFromCodeCoverage]
        public static SciterColor SkyBlue => new SciterColor(135, 206, 235);
        [ExcludeFromCodeCoverage]
        public static SciterColor HotPink => new SciterColor(255, 105, 180);
        [ExcludeFromCodeCoverage]
        public static SciterColor SlateBlue => new SciterColor(106, 90, 205);
        [ExcludeFromCodeCoverage]
        public static SciterColor IndianRed => new SciterColor(205, 92, 92);
        [ExcludeFromCodeCoverage]
        public static SciterColor SlateGray => new SciterColor(112, 128, 144);
        [ExcludeFromCodeCoverage]
        public static SciterColor Indigo => new SciterColor(75, 0, 130);
        [ExcludeFromCodeCoverage]
        public static SciterColor Snow => new SciterColor(255, 250, 250);
        [ExcludeFromCodeCoverage]
        public static SciterColor Ivory => new SciterColor(255, 240, 240);
        [ExcludeFromCodeCoverage]
        public static SciterColor SpringGreen => new SciterColor(0, 255, 127);
        [ExcludeFromCodeCoverage]
        public static SciterColor Khaki => new SciterColor(240, 230, 140);
        [ExcludeFromCodeCoverage]
        public static SciterColor SteelBlue => new SciterColor(70, 130, 180);
        [ExcludeFromCodeCoverage]
        public static SciterColor Lavender => new SciterColor(230, 230, 250);
        [ExcludeFromCodeCoverage]
        public static SciterColor Tan => new SciterColor(210, 180, 140);
        [ExcludeFromCodeCoverage]
        public static SciterColor LavenderBlush => new SciterColor(255, 240, 245);
        [ExcludeFromCodeCoverage]
        public static SciterColor Teal => new SciterColor(0, 128, 128);
        [ExcludeFromCodeCoverage]
        public static SciterColor LawnGreen => new SciterColor(124, 252, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor Thistle => new SciterColor(216, 191, 216);
        [ExcludeFromCodeCoverage]
        public static SciterColor LemonChiffon => new SciterColor(255, 250, 205);
        [ExcludeFromCodeCoverage]
        public static SciterColor Tomato => new SciterColor(253, 99, 71);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightBlue => new SciterColor(173, 216, 230);
        [ExcludeFromCodeCoverage]
        public static SciterColor Turquoise => new SciterColor(64, 224, 208);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightCoral => new SciterColor(240, 128, 128);
        [ExcludeFromCodeCoverage]
        public static SciterColor Violet => new SciterColor(238, 130, 238);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightCyan => new SciterColor(224, 255, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor Wheat => new SciterColor(245, 222, 179);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightGoldenrodYellow => new SciterColor(250, 250, 210);
        [ExcludeFromCodeCoverage]
        public static SciterColor White => new SciterColor(255, 255, 255);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightGreen => new SciterColor(144, 238, 144);
        [ExcludeFromCodeCoverage]
        public static SciterColor WhiteSmoke => new SciterColor(245, 245, 245);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightGray => new SciterColor(211, 211, 211);
        [ExcludeFromCodeCoverage]
        public static SciterColor Yellow => new SciterColor(255, 255, 0);
        [ExcludeFromCodeCoverage]
        public static SciterColor LightPink => new SciterColor(255, 182, 193);
        [ExcludeFromCodeCoverage]
        public static SciterColor YellowGreen => new SciterColor(154, 205, 50);

        #endregion

        #region IConvertible
        
        public TypeCode GetTypeCode()
        {
            return TypeCode.UInt32;
        }

        [ExcludeFromCodeCoverage]
        public bool ToBoolean(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public char ToChar(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public sbyte ToSByte(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public byte ToByte(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public short ToInt16(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public ushort ToUInt16(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider = null)
        {
            return unchecked((int)Value);
        }

        public uint ToUInt32(IFormatProvider provider = null)
        {
            return Value;
        }

        public long ToInt64(IFormatProvider provider = null)
        {
            return (long)Value;
        }

        public ulong ToUInt64(IFormatProvider provider = null)
        {
            return Value;
        }

        public float ToSingle(IFormatProvider provider = null)
        {
            return Value;
        }

        public double ToDouble(IFormatProvider provider = null)
        {
            return Value;
        }

        public decimal ToDecimal(IFormatProvider provider = null)
        {
            return Value;
        }

        [ExcludeFromCodeCoverage]
        public DateTime ToDateTime(IFormatProvider provider = null)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();// $"{Value:X8}";
        }

        [ExcludeFromCodeCoverage]
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}