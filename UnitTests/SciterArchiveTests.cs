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

        [TestCase(new byte[] { 
            83 , 65 , 114, 0  , 11 , 0  , 0  , 0  , 105, 0  , 255, 255, 1  , 0  , 255, 255, 110, 0  , 255, 255, 
            2  , 0  , 255, 255, 100, 0  , 255, 255, 3  , 0  , 255, 255, 101, 0  , 255, 255, 4  , 0  , 255, 255, 
            120, 0  , 255, 255, 5  , 0  , 255, 255, 46 , 0  , 255, 255, 6  , 0  , 255, 255, 104, 0  , 255, 255, 
            7  , 0  , 255, 255, 116, 0  , 255, 255, 8  , 0  , 255, 255, 109, 0  , 255, 255, 9  , 0  , 255, 255, 
            108, 0  , 255, 255, 10 , 0  , 255, 255, 0  , 0  , 255, 255, 1  , 0  , 255, 255, 1  , 0  , 0  , 0  , 
            112, 0  , 0  , 0  , 121, 0  , 0  , 0  , 153, 0  , 0  , 0  , 31 , 239, 187, 191, 60 , 104, 116, 109, 
            108, 32 , 108, 97 , 110, 103, 61 , 34 , 101, 110, 34 , 62 , 13 , 10 , 60 , 115, 99 , 114, 105, 112, 
            116, 32 , 116, 121, 112, 9  , 101, 61 , 39 , 116, 101, 120, 116, 47 , 116, 105, 128, 19 , 3  , 39 , 
            62 , 60 , 47 , 128, 9  , 64 , 39 , 3  , 104, 101, 97 , 100, 32 , 7  , 0  , 32 , 32 , 0  , 7  , 60 , 
            116, 105, 116, 108, 101, 62 , 84 , 64 , 5  , 0  , 60 , 32 , 46 , 32 , 6  , 192, 25 , 2  , 115, 116, 
            121, 32 , 12 , 32 , 55 , 96 , 7  , 32 , 54 , 0  , 47 , 160, 55 , 32 , 10 , 3  , 98 , 111, 100, 121, 
            128, 9  , 0  , 47 , 160, 10 , 32 , 29 , 3  , 116, 109, 108, 62 })]
        public void Open_archive_from_byte_array(byte[] buffer)
        {
            Assert.DoesNotThrow(() => _archive.Open(buffer));
            Assert.IsTrue(_archive.IsOpen);
            Assert.AreEqual(SciterArchive.DEFAULT_ARCHIVE_URI, _archive.Uri.AbsoluteUri);
        }

        [Test]
        public void Open_archive_with_invalid_resourceName_throws_InvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _archive.OpenAsync(assembly: _assembly, resourceName: "<not found>"));
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
            _archive.GetItem(new Uri(_archive.Uri, "index.html"), (res) =>
            {
                buffer = res.Data;
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
            _archive.GetItem("index.html", (res) =>
            {
                buffer = res.Data;
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