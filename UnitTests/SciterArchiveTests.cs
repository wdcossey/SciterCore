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
        public void Open_archive_async()
        {
            Assert.DoesNotThrowAsync(() => _archive.OpenAsync(_assembly));
            Assert.IsTrue(_archive.IsOpen);
            Assert.AreEqual(SciterArchive.DEFAULT_ARCHIVE_URI, _archive.Uri.AbsoluteUri);
        }

        [Test]
        public void Open_archive()
        {
            Assert.DoesNotThrow(() => _archive.Open(_assembly));
            Assert.IsTrue(_archive.IsOpen);
            Assert.AreEqual(SciterArchive.DEFAULT_ARCHIVE_URI, _archive.Uri.AbsoluteUri);
        }

        [Test]
        public void Open_archive_twice_throws_InvalidOperationException()
        {
            _archive.OpenAsync(_assembly);
            Assert.ThrowsAsync<InvalidOperationException>(() => _archive.OpenAsync(_assembly));
        }

        [Test]
        public void Get_valid_item_from_archive()
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
        public void Get_valid_item_relative_path()
        {
            _archive.OpenAsync(_assembly);
            var buffer = _archive.GetItem("index.html");
            Assert.NotNull(buffer);
        }
        
        [Test]
        public void Get_valid_item_relative_path_with_callback()
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
        public void Get_invalid_item_returns_null()
        {
            _archive.OpenAsync(_assembly);
            var buffer = _archive.GetItem(new Uri(_archive.Uri, "qwerty.html"));
            Assert.IsNull(buffer);
        }

        [Test]
        public void Open_archive_with_a_custom_uri()
        {
            using (var archive = new SciterArchive(new Uri("custom://app/")))
            {
                archive.OpenAsync(_assembly);
                var buffer = archive.GetItem("index.html");
                Assert.NotNull(buffer);
            }
        }

        [Test]
        public void Open_archive_with_a_custom_uri_from_string()
        {
            using (var archive = new SciterArchive("custom://app/"))
            {
                archive.OpenAsync(_assembly);
                var buffer = archive.GetItem("index.html");
                Assert.NotNull(buffer);
            }
        }

        [Test]
        public void Open_archive_with_a_custom_uri_and_incorrect_item_scheme_returns_null()
        {
            using (var archive = new SciterArchive("custom://app/"))
            {
                archive.OpenAsync(_assembly);
                var buffer = archive.GetItem("notme://app/index.html");
                Assert.IsNull(buffer);
            }
        }
        
    }
}