using NUnit.Framework;

namespace SciterCore.UnitTests.Graphics
{
    public class SciterSizeTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterSize_width_and_height_from_ctor(int width, int height)
        {
            var size = new SciterSize(width, height);
            Assert.AreEqual(width, size.Width);
            Assert.AreEqual(height, size.Height);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(int.MinValue, int.MinValue)]
        [TestCase(int.MaxValue, int.MaxValue)]
        public void SciterSize_set_width_and_height_from_property(int width, int height)
        {
            var size = new SciterSize
            {
                Width = width,
                Height = height
            };
            
            Assert.AreEqual(width, size.Width);
            Assert.AreEqual(height, size.Height);
        }
        
        
    }
}