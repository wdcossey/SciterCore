using NUnit.Framework;

namespace SciterCore.UnitTests
{
    public class PolylinePointTests
    {
        [TestCase(0f, 0f)]
        [TestCase(1f, 1f)]
        public void PolylinePoint_create(float x, float y)
        {
            var point = PolylinePoint.Create(x, y);
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }
    }
}