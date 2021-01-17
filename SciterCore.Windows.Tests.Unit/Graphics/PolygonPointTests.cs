using NUnit.Framework;

namespace SciterCore.Windows.Tests.Unit.Graphics
{
    public class PolygonPointTests
    {
        [TestCase(0f, 0f)]
        [TestCase(1f, 1f)]
        public void PolygonPoint_create(float x, float y)
        {
            var point = PolygonPoint.Create(x, y);
            Assert.AreEqual(x, point.X);
            Assert.AreEqual(y, point.Y);
        }
    }
}