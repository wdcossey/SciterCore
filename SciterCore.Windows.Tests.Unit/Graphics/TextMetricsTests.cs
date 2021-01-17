using NUnit.Framework;

namespace SciterCore.Windows.Tests.Unit.Graphics
{
    public class TextMetricsTests
    {
        [TestCase(1f, 200f, 21f, 2f, 2f, 3)]
        public void SciterPoint_width_and_height_from_ctor(float minWidth, float maxWidth, float height, float ascent, float descent, int noLines)
        {
            var actual = new TextMetrics(minWidth, maxWidth, height, ascent, descent, noLines);
            
            Assert.AreEqual(ascent, actual.Ascent);
            Assert.AreEqual(descent, actual.Descent);
            Assert.AreEqual(height, actual.Height);
            Assert.AreEqual(noLines, actual.LineCount);
            Assert.AreEqual(maxWidth, actual.MaxWidth);
            Assert.AreEqual(minWidth, actual.MinWidth);
        }
        
        [TestCase(1f, 200f, 21f, 2f, 2f, 3)]
        public void SciterPoint_width_and_height_from_create(float minWidth, float maxWidth, float height, float ascent, float descent, int noLines)
        {
            var actual = TextMetrics.Create(minWidth, maxWidth, height, ascent, descent, noLines);

            Assert.AreEqual(ascent, actual.Ascent);
            Assert.AreEqual(descent, actual.Descent);
            Assert.AreEqual(height, actual.Height);
            Assert.AreEqual(noLines, actual.LineCount);
            Assert.AreEqual(maxWidth, actual.MaxWidth);
            Assert.AreEqual(minWidth, actual.MinWidth);
        }
        
        [TestCase(1f, 200f, 21f, 2f, 2f, 3)]
        public void SciterPoint_set_width_and_height_from_property(float minWidth, float maxWidth, float height, float ascent, float descent, int noLines)
        {
            var actual = new TextMetrics
            {
                Ascent = ascent,
                Descent = descent,
                Height = height,
                LineCount = noLines,
                MaxWidth = maxWidth,
                MinWidth = minWidth,
            };
            
            Assert.AreEqual(ascent, actual.Ascent);
            Assert.AreEqual(descent, actual.Descent);
            Assert.AreEqual(height, actual.Height);
            Assert.AreEqual(noLines, actual.LineCount);
            Assert.AreEqual(maxWidth, actual.MaxWidth);
            Assert.AreEqual(minWidth, actual.MinWidth);
        }
    }
}