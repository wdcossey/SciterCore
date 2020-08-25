using System;
using System.Drawing;
using NUnit.Framework;

namespace SciterCore.UnitTests
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
        public void SciterColor_toSciterColor(int r, int g, int b, int a, uint exp)
        {
            var actual = SciterColor.Create(Color.FromArgb(a, r, g, b));
            Assert.AreEqual(exp, actual.Value);
        }

        [TestCase(255, 0, 0)]
        [TestCase(0, 0, 0)]
        [TestCase(255, 0, 0)]
        public void Basic(int r, int g, int b)
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
        public void Alpha_as_int(int r, int g, int b, int a)
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
            var actual = SciterColor.Create(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
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

        [TestCase(255, 0, 0, .5d)]
        [TestCase(0, 0, 0, .25d)]
        [TestCase(255, 0, 0, .5d)]
        public void Alpha_as_double(int r, int g, int b, double a)
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
    }
}