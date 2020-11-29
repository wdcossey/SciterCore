using NUnit.Framework;

namespace SciterCore.Tests.Unit.Graphics
{
    public class SciterSizeTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(100, 200)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterSize_width_and_height_from_ctor(int width, int height)
        {
            var actual = new SciterSize(width, height);
            Assert.AreEqual(width, actual.Width);
            Assert.AreEqual(height, actual.Height);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(100, 200)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterSize_set_width_and_height_from_property(int width, int height)
        {
            var actual = new SciterSize
            {
                Width = width,
                Height = height
            };
            
            Assert.AreEqual(width, actual.Width);
            Assert.AreEqual(height, actual.Height);
        }
    }
}