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
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Create_element_from_tag_and_text(string tag)
        {
            var element = SciterElement.Create(tag, "text");
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual("text", element.Text);
            Assert.AreEqual($"<{tag}>text</{tag}>", element.Html);
            Assert.AreEqual("text", element.InnerHtml);
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Create_element_from_tag_and_set_text(string tag)
        {
            SciterWindow sciterWindow = null;

            try
            {
                sciterWindow = new SciterWindow()
                    .CreateMainWindow(320, 240)
                    .LoadHtml("<html></html>");

                var element = sciterWindow
                    .RootElement
                    .AppendChildElement("body")
                    .AppendChildElement(tag, "you should not see me!");
                
                element.SetText("text");
                
                Assert.IsNotNull(element);
                Assert.AreEqual(tag, element.Tag);
                Assert.AreEqual("text", element.Text);
                Assert.AreEqual(0, element.ChildCount);
            }
            finally
            {
                sciterWindow?.Close();
                sciterWindow?.Destroy();
            }
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Create_element_from_tag_and_set_text_from_property(string tag)
        {
            SciterWindow sciterWindow = null;

            try
            {
                sciterWindow = new SciterWindow()
                    .CreateMainWindow(320, 240)
                    .LoadHtml("<html></html>");

                var element = sciterWindow
                    .RootElement
                    .AppendChildElement("body")
                    .AppendChildElement(tag, "you should not see me!");
                
                element.Text = "text";
                
                Assert.IsNotNull(element);
                Assert.AreEqual(tag, element.Tag);
                Assert.AreEqual("text", element.Text);
                Assert.AreEqual(0, element.ChildCount);
            }
            finally
            {
                sciterWindow?.Close();
                sciterWindow?.Destroy();
            }
        }

        [TestCase("div", "inner div text")]
        [TestCase("text", "inner text text")]
        [TestCase("label", "inner label text")]
        [TestCase("ul", "inner ul text")]
        public void Create_element_from_tag_and_set_html(string tag, string innerText)
        {
            SciterWindow sciterWindow = null;

            try
            {
                sciterWindow = new SciterWindow()
                    .CreateMainWindow(320, 240)
                    .LoadHtml("<html></html>");

                var element = sciterWindow
                    .RootElement
                    .AppendChildElement("body")
                    .AppendChildElement(tag, "you should not see me!");

                element.SetHtml($"<div>{innerText}</div>");
                
                Assert.IsNotNull(element);
                Assert.AreEqual(tag, element.Tag);
                Assert.AreEqual(innerText, element.Text);
                Assert.AreEqual(1, element.ChildCount);
            }
            finally
            {
                sciterWindow?.Close();
                sciterWindow?.Destroy();
            }
        }

        [TestCase("div", "inner div text")]
        [TestCase("text", "inner text text")]
        [TestCase("label", "inner label text")]
        [TestCase("ul", "inner ul text")]
        public void Create_element_from_tag_and_set_html_from_property(string tag, string innerText)
        {
            SciterWindow sciterWindow = null;

            try
            {
                sciterWindow = new SciterWindow()
                    .CreateMainWindow(320, 240)
                    .LoadHtml("<html></html>");

                var element = sciterWindow
                    .RootElement
                    .AppendChildElement("body")
                    .AppendChildElement(tag, "you should not see me!");

                element.Html = $"<div>{innerText}</div>";
                
                Assert.IsNotNull(element);
                Assert.AreEqual(tag, element.Tag);
                Assert.AreEqual(innerText, element.Text);
                Assert.AreEqual(1, element.ChildCount);
            }
            finally
            {
                sciterWindow?.Close();
                sciterWindow?.Destroy();
            }
        }
        
        [Test]
        public void Create_with_invalid_handle_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var element = new SciterElement(IntPtr.Zero);
            });
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_set_and_get_attribute_value_from_string(string tag)
        {
            var element = SciterElement.Create(tag);

            element.SetAttributeValue("unittest", "value");
            
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(1, element.AttributeCount);

            Assert.AreEqual("value", element.GetAttributeValue("unittest"));
            Assert.AreEqual("value", element.Attributes["unittest"]);
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_set_and_get_attribute_value_from_func(string tag)
        {
            var element = SciterElement.Create(tag);

            element.SetAttributeValue("unittest", () =>  "value");
            
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(1, element.AttributeCount);

            Assert.AreEqual("value", element.GetAttributeValue("unittest"));
            Assert.AreEqual("value", element.Attributes["unittest"]);
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_set_and_get_attribute_value_from_func_with_object(string tag)
        {
            var element = SciterElement.Create(tag);

            element.SetAttributeValue("unittest", sciterElement => sciterElement.Tag, element);
            
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(1, element.AttributeCount);
            
            Assert.AreEqual(tag, element.GetAttributeValue("unittest"));
            Assert.AreEqual(tag, element.Attributes["unittest"]);
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_set_and_get_state(string tag)
        {
            var element = SciterElement.Create(tag);

            element.SetState(ElementState.Checked);
            
            Assert.IsNotNull(element);

            Assert.IsTrue(element.State.HasFlag(ElementState.Checked));

        }
        
        
    }
}