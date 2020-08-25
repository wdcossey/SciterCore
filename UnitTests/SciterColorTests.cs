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
            var rgba = new SciterColor(color);
            Assert.AreEqual(r, rgba.R);
            Assert.AreEqual(g, rgba.G);
            Assert.AreEqual(b, rgba.B);
            Assert.AreEqual(a, rgba.A);
        }

        [TestCase(255, 0, 0, 127, 2130706687u)]
        [TestCase(0, 0, 0, 0, 0u)]
        [TestCase(255, 0, 0, 255, 4278190335u)]
        public void SciterColor_toSciterColor(int r, int g, int b, int a, uint exp)
        {
            var actual = SciterColor.ToSciterColor(Color.FromArgb(a, r, g, b));
            Assert.AreEqual(exp, actual);
        }

        [TestCase(255, 0, 0, 255)]
        [TestCase(0, 0, 0, 127)]
        [TestCase(255, 0, 0, 0)]
        public void RGBAColor_alpha_as_int(int r, int g, int b, int a)
        {
            var rgba = new SciterColor(r, g, b, a);
            Assert.AreEqual(r, rgba.R);
            Assert.AreEqual(g, rgba.G);
            Assert.AreEqual(b, rgba.B);
            Assert.AreEqual(a, rgba.A);
        }

        [TestCase(255, 0, 0, .5d)]
        [TestCase(0, 0, 0, .25d)]
        [TestCase(255, 0, 0, .5d)]
        public void SciterColor_alpha_as_double(int r, int g, int b, double a)
        {
            var rgba = new SciterColor(r, g, b, a);
            Assert.AreEqual(r, rgba.R);
            Assert.AreEqual(g, rgba.G);
            Assert.AreEqual(b, rgba.B);
            Assert.AreEqual((int) (a * byte.MaxValue), rgba.A);
        }

        [TestCase(255, 0, 0, 127)]
        [TestCase(0, 0, 0, 0)]
        [TestCase(255, 0, 0, 255)]
        public void SciterColor_fromColor(int r, int g, int b, int a)
        {
            var rgba = SciterColor.FromColor(Color.FromArgb(a, r, g, b));
            Assert.AreEqual(r, rgba.R);
            Assert.AreEqual(g, rgba.G);
            Assert.AreEqual(b, rgba.B);
            Assert.AreEqual(a, rgba.A);
        }
    }
}