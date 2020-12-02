using NUnit.Framework;

namespace SciterCore.Tests.Unit.Graphics
{
    public class SciterPointTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(100, 200)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterPoint_width_and_height_from_ctor(int x, int y)
        {
            var actual = new SciterPoint(x, y);
            Assert.AreEqual(x, actual.X);
            Assert.AreEqual(y, actual.Y);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(100, 200)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterPoint_set_width_and_height_from_property(int x, int y)
        {
            var actual = new SciterPoint
            {
                X = x,
                Y = y
            };
            
            Assert.AreEqual(x, actual.X);
            Assert.AreEqual(y, actual.Y);
        }
    }
}