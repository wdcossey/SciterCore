using System;
using NUnit.Framework;

namespace SciterCore.Windows.Tests.Unit
{
    public class SciterElementTests
    {
        private SciterWindow _sciterWindow;

        [SetUp]
        public void Setup()
        {
            _sciterWindow = new SciterWindow()
                .CreateMainWindow(320, 240)
                .LoadHtml("<html></html>");
        }

        [TearDown]
        public void TearDown()
        {
            _sciterWindow?.Close();
            _sciterWindow?.Destroy();
        }
        
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
            var element = _sciterWindow
                .RootElement
                .AppendElement("body", (elm) => elm)
                .AppendElement(tag, "you should not see me!", elm => elm);

            element.SetText("text");

            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual("text", element.Text);
            Assert.AreEqual(0, element.ChildCount);
        }

        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Create_element_from_tag_and_set_text_from_property(string tag)
        {

            var element = _sciterWindow
                .RootElement
                .AppendElement("body", elm => elm)
                .AppendElement(tag, "you should not see me!", elm => elm);

            element.Text = "text";

            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual("text", element.Text);
            Assert.AreEqual(0, element.ChildCount);
        }
        
        [Test]
        public void Verify_element_parent()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div, elm => elm)
                .AppendElement(text, elm => elm)
                .AppendElement(ul, elm => elm);
            
            Assert.AreEqual(ul.Parent.UniqueId, text.UniqueId);
            Assert.AreEqual(text.Parent.UniqueId, div.UniqueId);
            Assert.AreEqual(div.Parent.UniqueId, _sciterWindow.RootElement.UniqueId);
        }
        
        [Test]
        public void Verify_element_children()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div, elm => elm)
                .AppendElement(text, elm => elm)
                .AppendElement(ul, elm => elm);
            
            Assert.AreEqual(_sciterWindow.RootElement.Children[0].UniqueId, div.UniqueId);
            Assert.AreEqual(div.Children[0].UniqueId, text.UniqueId);
            Assert.AreEqual(text.Children[0].UniqueId, ul.UniqueId);
        }
        
        [Test]
        public void Verify_element_next_sibling()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div)
                .AppendElement(text)
                .AppendElement(ul);

            Assert.AreEqual(_sciterWindow.RootElement.GetChildAtIndex(0).UniqueId, div.UniqueId);
            Assert.AreEqual(div.NextSibling.UniqueId, text.UniqueId);
            Assert.AreEqual(text.NextSibling.UniqueId, ul.UniqueId);
        }
        
        [Test]
        public void Verify_element_prev_sibling()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div)
                .AppendElement(text)
                .AppendElement(ul);
            
            Assert.AreEqual(ul.PreviousSibling.UniqueId, text.UniqueId);
            Assert.AreEqual(text.PreviousSibling.UniqueId, div.UniqueId); ;
        }

        [Test]
        public void Verify_element_first_sibling()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div)
                .AppendElement(text)
                .AppendElement(ul);
            
            Assert.AreEqual(div.FirstSibling.UniqueId, div.UniqueId);
            Assert.AreEqual(text.FirstSibling.UniqueId, div.UniqueId);
            Assert.AreEqual(ul.FirstSibling.UniqueId, div.UniqueId);
        }

        [Test]
        public void Verify_element_last_sibling()
        {
            var div = SciterElement.Create("div");
            var text = SciterElement.Create("text");
            var ul = SciterElement.Create("ul");
            
            _sciterWindow
                .RootElement
                .AppendElement(div)
                .AppendElement(text)
                .AppendElement(ul);
            
            Assert.AreEqual(div.LastSibling.UniqueId, ul.UniqueId);
            Assert.AreEqual(text.LastSibling.UniqueId, ul.UniqueId);
            Assert.AreEqual(ul.LastSibling.UniqueId, ul.UniqueId);
        }

        [TestCase("div", "inner div text")]
        [TestCase("text", "inner text text")]
        [TestCase("label", "inner label text")]
        [TestCase("ul", "inner ul text")]
        public void Create_element_from_tag_and_set_html(string tag, string innerText)
        {
            var element = _sciterWindow
                .RootElement
                .AppendElement("body", elm => elm)
                .AppendElement(tag, "you should not see me!", elm => elm);

            element.SetHtml($"<div>{innerText}</div>");

            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(innerText, element.Text);
            Assert.AreEqual(1, element.ChildCount);
        }

        [TestCase("div", "inner div text")]
        [TestCase("text", "inner text text")]
        [TestCase("label", "inner label text")]
        [TestCase("ul", "inner ul text")]
        public void Create_element_from_tag_and_set_html_from_property(string tag, string innerText)
        {
            var element = _sciterWindow
                .RootElement
                .AppendElement("body", elm => elm)
                .AppendElement(tag, "you should not see me!", elm => elm);

            element.Html = $"<div>{innerText}</div>";

            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(innerText, element.Text);
            Assert.AreEqual(1, element.ChildCount);
        }

        [Test]
        public void Create_with_invalid_handle_throws_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var sciterElement = new SciterElement(IntPtr.Zero);
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
            Assert.AreEqual("value", element["unittest"]);
            Assert.AreEqual("value", element.Attributes["unittest"]);
        }
        
        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_set_and_get_attribute_value_from_string_v2(string tag)
        {
            var element = SciterElement.Create(tag);

            element["unittest"] = "value";
            
            Assert.IsNotNull(element);
            Assert.AreEqual(tag, element.Tag);
            Assert.AreEqual(1, element.AttributeCount);

            Assert.AreEqual("value", element.GetAttributeValue("unittest"));
            Assert.AreEqual("value", element["unittest"]);
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
            Assert.AreEqual(element.State, element.GetState());
        }

        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_first_last_child(string tag)
        {
            var element = _sciterWindow
                .RootElement.AppendElement(SciterElement.Create(tag), elm => elm);

            element
                .AppendElement("first", "first child")
                .AppendElement("middle", "middle child")
                .AppendElement("last", "last child");

            Assert.IsNotNull(element);

            Assert.AreEqual(3, element.ChildCount);

            Assert.AreEqual(0, element.FirstChild().Index);
            Assert.AreEqual(element[element.FirstChild().Index].UniqueId, element.FirstChild().UniqueId);
            
            Assert.AreEqual(2, element.LastChild().Index); 
            Assert.AreEqual(element[element.LastChild().Index].UniqueId, element.LastChild().UniqueId);
            
        }

        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Element_first_last_child_at_index(string tag)
        {
            var element = _sciterWindow
                .RootElement.AppendElement(SciterElement.Create(tag), elm => elm);

            element
                .AppendElement("first", "first child")
                .AppendElement("middle", "middle child")
                .AppendElement("last", "last child");

            Assert.IsNotNull(element);

            Assert.AreEqual(3, element.ChildCount);

            Assert.AreEqual(element[0].UniqueId, element.GetChildAtIndex(0).UniqueId);
            Assert.AreEqual(element[1].UniqueId, element.GetChildAtIndex(1).UniqueId);
            Assert.AreEqual(element[2].UniqueId, element.GetChildAtIndex(2).UniqueId);
        }

        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Clear_element_text(string tag)
        {
            var element = _sciterWindow
                .RootElement.AppendElement(SciterElement.Create(tag), elm => elm);

            element.AppendElement("elm", "this is my text");

            Assert.IsNotNull(element);
            Assert.IsFalse(string.IsNullOrWhiteSpace(element.Text));

            element.ClearText();
            
            Assert.AreEqual(string.Empty, element.Text);
        }

        [TestCase("div")]
        [TestCase("text")]
        [TestCase("label")]
        [TestCase("ul")]
        public void Clone_element(string tag)
        {
            var element = _sciterWindow
                .RootElement.AppendElement(SciterElement.Create(tag), elm => elm);

            var clonedElement = _sciterWindow
                .RootElement.AppendElement(element.Clone(), elm => elm);

            Assert.IsNotNull(element);
            Assert.IsNotNull(clonedElement);
            Assert.AreEqual(2, _sciterWindow.RootElement.ChildCount);
            Assert.AreNotEqual(element.UniqueId, clonedElement.UniqueId);
        }
    }
}