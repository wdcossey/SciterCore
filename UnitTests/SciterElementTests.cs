using System;
using NUnit.Framework;

namespace SciterCore.UnitTests
{
    public class SciterElementTests
    {
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Create_element_from_tag(string tag)
        {
            var element = SciterElement.Create(tag);
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
        }
        
        [Test]
        public void Create_element_from_tag_and_text()
        {
            var element = SciterElement.Create("div", "text");
            Assert.IsNotNull(element);
            Assert.AreEqual("div", element.Tag);
            Assert.AreEqual("text", element.Text);
            Assert.AreEqual("<div>text</div>", element.Html);
            Assert.AreEqual("text", element.InnerHtml);
        }
        
        [Test]
        [Ignore("SetText() not working!")]
        public void Create_element_from_tag_and_set_text()
        {
            var element = SciterElement.Create("div", "something");
            element.Text = "text";
            Assert.IsNotNull(element);
            Assert.AreEqual("div", element.Tag);
            Assert.AreEqual("text", element.Text);
        }
        
        [Test]
        public void Create_with_invalid_handle_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var element = new SciterElement(IntPtr.Zero);
            });
        }
        
    }
}