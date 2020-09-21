using System;
using System.Reflection;
using NUnit.Framework;

namespace SciterCore.UnitTests
{
    public class SciterArchiveTests
    {
        private SciterArchive _archive;
        private Assembly _assembly;

        [SetUp]
        public void SetUp()
        {
            _assembly = GetType().Assembly;
            _archive = new SciterArchive();
        }

        [TearDown]
        public void TearDown()
        {
            if (_archive.IsOpen)
                _archive?.Dispose();
        }

        [Test]
        public void Archive_is_not_opened()
        {
            Assert.IsFalse(_archive.IsOpen);
        }

        [Test]
        public void Archive_open_async()
        {
            Assert.DoesNotThrowAsync(() => _archive.OpenAsync(_assembly));
            Assert.IsTrue(_archive.IsOpen);
            Assert.AreEqual(SciterArchive.DEFAULT_ARCHIVE_URI, _archive.Uri.AbsoluteUri);
        }

        [Test]
        public void Archive_open()
        {
            Assert.DoesNotThrow(() => _archive.Open(_assembly));
            Assert.IsTrue(_archive.IsOpen);
            Assert.AreEqual(SciterArchive.DEFAULT_ARCHIVE_URI, _archive.Uri.AbsoluteUri);
        }

        [Test]
        public void Archive_open_twice_throws_InvalidOperationException()
        {
            _archive.OpenAsync(_assembly);
            Assert.ThrowsAsync<InvalidOperationException>(() => _archive.OpenAsync(_assembly));
        }

        [Test]
        public void Archive_get_valid_item()
        {
            byte[] buffer = null;
            
            _archive.OpenAsync(_assembly);
            _archive.GetItem(new Uri(_archive.Uri, "index.html"), (bytes, s) =>
            {
                buffer = bytes;
            });
            
            Assert.NotNull(buffer);
        }

        [Test]
        public void Archive_get_valid_item_relative_path()
        {
            _archive.OpenAsync(_assembly);
            var buffer = _archive.GetItem("index.html");
            Assert.NotNull(buffer);
        }
        
        [Test]
        public void Archive_get_valid_item_relative_path_with_callback()
        {
            byte[] buffer = null;
            
            _archive.OpenAsync(_assembly);
            _archive.GetItem("index.html", (bytes, s) =>
            {
                buffer = bytes;
            });
            
            Assert.NotNull(buffer);
        }

        [Test]
        public void Archive_get_invalid_item()
        {
            _archive.OpenAsync(_assembly);
            var buffer = _archive.GetItem(new Uri(_archive.Uri, "qwerty.html"));
            Assert.IsNull(buffer);
        }
        
    }
}