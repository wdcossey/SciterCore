using System;
using System.Drawing;
using System.Globalization;
using NUnit.Framework;

namespace SciterCore.Tests.Unit.Graphics
{
    public class SciterColorTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCase(2130706687u, 255, 0, 0, 127)]
        [TestCase(0u, 0, 0, 0, 0)]
        [TestCase(4278190335u, 255, 0, 0, 255)]
        public void SciterColor_from_uint(uint color, int r, int g, int b, int a)
        {
            var actual = SciterColor.Create(color);
            Assert.AreEqual(r, actual.R);
            Assert.AreEqual(g, actual.G);
            Assert.AreEqual(b, actual.B);
            Assert.AreEqual(a, actual.A);
        }

        [TestCase(255, 0, 0, 127, 2130706687u)]
        [TestCase(0, 0, 0, 0, 0u)]
        [TestCase(255, 0, 0, 255, 4278190335u)]
        public void rgba_to_SciterColor(int r, int g, int b, int a, uint exp)
        {
            var actual = SciterColor.Create(Color.FromArgb(a, r, g, b));
            Assert.AreEqual(exp, actual.Value);
        }

        [TestCase(255, 0, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(255, 0, 0)]
        public void Basic(byte r, byte g, byte b)
        {
            var actual = SciterColor.Create(r, g, b);
            Assert.AreEqual(r, actual.R);
            Assert.AreEqual(g, actual.G);
            Assert.AreEqual(b, actual.B);
            Assert.AreEqual(byte.MaxValue, actual.A);
        }

        [TestCase(255, 0, 0, 255)]
        [TestCase(0, 0, 0, 127)]
        [TestCase(255, 0, 0, 0)]
        [TestCase(127, 127, 127, 255)]
        public void Alpha_as_int(byte r, byte g, byte b, byte a)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(r, actual.R);
            Assert.AreEqual(g, actual.G);
            Assert.AreEqual(b, actual.B);
            Assert.AreEqual(a, actual.A);
        }

        [Test]
        public void Test_max_value()
        {
            var actual = SciterColor.Create(byte.MaxValue, byte.MaxValue, byte.MaxValue, float.MaxValue);
            Assert.AreEqual(255, actual.R);
            Assert.AreEqual(255, actual.G);
            Assert.AreEqual(255, actual.B);
            Assert.AreEqual(255, actual.A);
        }

        [Test]
        public void Test_min_value()
        {
            var actual = SciterColor.Create(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue);
            Assert.AreEqual(0, actual.R);
            Assert.AreEqual(0, actual.G);
            Assert.AreEqual(0, actual.B);
            Assert.AreEqual(0, actual.A);
        }

        [TestCase(255, 0, 0, .5f)]
        [TestCase(0, 0, 0, .25f)]
        [TestCase(255, 0, 0, .5f)]
        public void Alpha_as_float(byte r, byte g, byte b, float a)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(r, actual.R);
            Assert.AreEqual(g, actual.G);
            Assert.AreEqual(b, actual.B);
            Assert.AreEqual((int) (a * byte.MaxValue), actual.A);
        }

        [TestCase(255, 0, 0, 127)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(255, 0, 0, 255)]
        public void From_System_Drawing_Color(int r, int g, int b, int a)
        {
            var color = Color.FromArgb(a, r, g, b);
            var actual = SciterColor.Create(color);
            Assert.AreEqual(r, actual.R);
            Assert.AreEqual(g, actual.G);
            Assert.AreEqual(b, actual.B);
            Assert.AreEqual(a, actual.A);
        }

        [Test]
        public void SciterColor_GetTypeCode()
        {
            var actual = SciterColor.Transparent;
            Assert.AreEqual(TypeCode.UInt32, actual.GetTypeCode());
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(255, 255, 255, 255, -1)]
        [TestCase(255, 0, 0, 127, 2130706687)]
        public void SciterColor_ToInt32(byte r, byte g, byte b, byte a, int expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToInt32());
        }

        [TestCase(0, 0, 0, 0, uint.MinValue)]
        [TestCase(255, 255, 255, 255, uint.MaxValue)]
        [TestCase(255, 0, 0, 127, 2130706687U)]
        public void SciterColor_ToUInt32(byte r, byte g, byte b, byte a, uint expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToUInt32());
        }

        [TestCase(0, 0, 0, 0, (long)uint.MinValue)]
        [TestCase(255, 255, 255, 255, (long)uint.MaxValue)]
        [TestCase(255, 0, 0, 127, 2130706687L)]
        public void SciterColor_ToInt64(byte r, byte g, byte b, byte a, long expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToInt64());
        }

        [TestCase(0, 0, 0, 0, (ulong)uint.MinValue)]
        [TestCase(255, 255, 255, 255, (ulong)uint.MaxValue)]
        [TestCase(255, 0, 0, 127, 2130706687UL)]
        public void SciterColor_ToInt64(byte r, byte g, byte b, byte a, ulong expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToUInt64());
        }

        [TestCase(0, 0, 0, 0, (float)uint.MinValue)]
        [TestCase(255, 255, 255, 255, (float)uint.MaxValue)]
        [TestCase(255, 0, 0, 127, 2130706687f)]
        public void SciterColor_ToSingle(byte r, byte g, byte b, byte a, float expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToSingle());
        }

        [TestCase(0, 0, 0, 0, (double)uint.MinValue)]
        [TestCase(255, 255, 255, 255, (double)uint.MaxValue)]
        [TestCase( 255, 0, 0, 127, 2130706687d)]
        public void SciterColor_ToDouble(byte r, byte g, byte b, byte a, double expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToDouble());
        }

        [TestCase(0, 0, 0, 0, (double)uint.MinValue)]
        [TestCase(255, 255, 255, 255, (double)uint.MaxValue)]
        [TestCase( 255, 0, 0, 127, 2130706687d)]
        public void SciterColor_ToDecimal(byte r, byte g, byte b, byte a, decimal expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(expected, actual.ToDecimal());
        }

        [TestCase(0, 0, 0, 0, "00000000")]
        [TestCase(255, 255, 255, 255, "FFFFFFFF")]
        [TestCase( 127, 127, 127, 127, "7F7F7F7F")]
        [TestCase( 255, 0, 0, 127, "7FFF0000")]
        public void SciterColor_ToString(byte r, byte g, byte b, byte a, string expected)
        {
            var actual = SciterColor.Create(r, g, b, a);
            Assert.AreEqual(actual.ToString(CultureInfo.InvariantCulture), actual.ToString(null));
            Assert.AreEqual(expected, actual.ToString(null));
        }
        
        [Test]
        public void SciterColor_are_equal()
        {
            var one = SciterColor.Transparent;
            var two = SciterColor.Create(0,0,0,0);
            Assert.IsTrue(one.Equals(two));
        }
        
        [Test]
        public void SciterColor_are_not_equal()
        {
            var one = SciterColor.Transparent;
            Assert.IsFalse(one.Equals(null));
        }
        
        #region KnownColors

        [Test]
        public void SciterColor_Transparent()
        {
            var actual = SciterColor.Transparent;
            Assert.AreEqual(0, actual.R);
            Assert.AreEqual(0, actual.G);
            Assert.AreEqual(0, actual.B);
            Assert.AreEqual(0, actual.A);
        }

        #endregion
    }
}