using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace SciterCore.UnitTests.Graphics
{
    public class SciterImageTests
    {
        private SciterImage _sciterImage;

        [SetUp]
        public void Setup()
        {
            
        }
        
        [TearDown]
        public void TearDown()
        {
            _sciterImage?.Dispose();
        }
        
        [TestCase(10, 10, true)]
        [TestCase(10, 10, false)]
        public void Create_width_height_withAlpha(int width, int height, bool withAlpha)
        {
            _sciterImage = SciterImage.Create(width, height, withAlpha);

            Assert.NotNull(_sciterImage);
        }
        
        [Test]
        public void Dimensions()
        {
            _sciterImage = SciterImage.Create(1, 1, true);

            var dimensions = _sciterImage.Dimensions;
            
            Assert.AreEqual(1, dimensions.Width);
            Assert.AreEqual(1, dimensions.Height);
        }
        
        [Test]
        public void To_SciterValue()
        {
            _sciterImage = SciterImage.Create(1, 1, false);

            var value = _sciterImage.ToSciterValue();
            
            Assert.NotNull(value);
            Assert.IsTrue(value.IsResource);
        }

        [TestCase(new byte[]
        {
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 1, 0, 0, 0, 1, 8, 6, 0, 0, 0, 31, 21,
            196, 137, 0, 0, 0, 13, 73, 68, 65, 84, 8, 153, 99, 96, 96, 96, 248, 15, 0, 1, 4, 1, 0, 125, 178, 200, 223,
            0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
        }, Description = ".png data")]
        [TestCase(new byte[]
        {
            255, 216, 255, 224, 0, 16, 74, 70, 73, 70, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 255, 219, 0, 67, 0, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 219, 0,
            67, 1, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 192, 0, 17, 8, 0, 1, 0, 1, 3, 1, 34, 0, 2, 17, 1, 3, 17, 1, 255, 196, 0, 31, 0, 0, 1, 5, 1,
            1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 255, 196, 0, 181, 16, 0, 2, 1, 3,
            3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125, 1, 2, 3, 0, 4, 17, 5, 18, 33, 49, 65, 6, 19, 81, 97, 7, 34, 113, 20,
            50, 129, 145, 161, 8, 35, 66, 177, 193, 21, 82, 209, 240, 36, 51, 98, 114, 130, 9, 10, 22, 23, 24, 25, 26,
            37, 38, 39, 40, 41, 42, 52, 53, 54, 55, 56, 57, 58, 67, 68, 69, 70, 71, 72, 73, 74, 83, 84, 85, 86, 87, 88,
            89, 90, 99, 100, 101, 102, 103, 104, 105, 106, 115, 116, 117, 118, 119, 120, 121, 122, 131, 132, 133, 134,
            135, 136, 137, 138, 146, 147, 148, 149, 150, 151, 152, 153, 154, 162, 163, 164, 165, 166, 167, 168, 169,
            170, 178, 179, 180, 181, 182, 183, 184, 185, 186, 194, 195, 196, 197, 198, 199, 200, 201, 202, 210, 211,
            212, 213, 214, 215, 216, 217, 218, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 241, 242, 243, 244,
            245, 246, 247, 248, 249, 250, 255, 196, 0, 31, 1, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2,
            3, 4, 5, 6, 7, 8, 9, 10, 11, 255, 196, 0, 181, 17, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119, 0, 1,
            2, 3, 17, 4, 5, 33, 49, 6, 18, 65, 81, 7, 97, 113, 19, 34, 50, 129, 8, 20, 66, 145, 161, 177, 193, 9, 35,
            51, 82, 240, 21, 98, 114, 209, 10, 22, 36, 52, 225, 37, 241, 23, 24, 25, 26, 38, 39, 40, 41, 42, 53, 54, 55,
            56, 57, 58, 67, 68, 69, 70, 71, 72, 73, 74, 83, 84, 85, 86, 87, 88, 89, 90, 99, 100, 101, 102, 103, 104,
            105, 106, 115, 116, 117, 118, 119, 120, 121, 122, 130, 131, 132, 133, 134, 135, 136, 137, 138, 146, 147,
            148, 149, 150, 151, 152, 153, 154, 162, 163, 164, 165, 166, 167, 168, 169, 170, 178, 179, 180, 181, 182,
            183, 184, 185, 186, 194, 195, 196, 197, 198, 199, 200, 201, 202, 210, 211, 212, 213, 214, 215, 216, 217,
            218, 226, 227, 228, 229, 230, 231, 232, 233, 234, 242, 243, 244, 245, 246, 247, 248, 249, 250, 255, 218, 0,
            12, 3, 1, 0, 2, 17, 3, 17, 0, 63, 0, 142, 138, 40, 160, 15, 255, 217
        }, Description = ".jpg data")]
        public void From_byte_array(byte[] buffer)
        {
            _sciterImage = SciterImage.Create(buffer);
            var dimensions = _sciterImage.Dimensions;

            Assert.NotNull(_sciterImage);
            Assert.AreEqual(1, dimensions.Width);
            Assert.AreEqual(1, dimensions.Height);
        }

        [Test]
        public void From_Bitmap()
        {
            using (var bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
            {
                _sciterImage = SciterImage.Create(bitmap);

                var dimensions = _sciterImage.Dimensions;

                Assert.NotNull(_sciterImage);
                Assert.AreEqual(1, dimensions.Width);
                Assert.AreEqual(1, dimensions.Height);
            }
        }
        
        [TestCase(new byte[] { 63, 63, 63, 127})]
        [TestCase(new byte[] { 255, 255, 255, 255})]
        public void From_IntPtr(byte[] buffer)
        {
            var data = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, data, buffer.Length);

            try
            {
                _sciterImage = SciterImage.Create(data, 1, 1, true);

                var dimensions = _sciterImage.Dimensions;

                Assert.NotNull(_sciterImage);
                Assert.AreEqual(1, dimensions.Width);
                Assert.AreEqual(1, dimensions.Height);
            }
            finally
            {
                Marshal.FreeHGlobal(data);
            }
        }

        [Test]
        public void From_SciterValue()
        {
            var sciterImage = SciterImage.Create(1, 1, false);
            var sciterValue = sciterImage.ToSciterValue();
            
            _sciterImage = SciterImage.Create(sciterValue);
            var dimensions = _sciterImage.Dimensions;

            Assert.NotNull(_sciterImage);
            Assert.AreEqual(sciterImage.Handle, _sciterImage.Handle);
            Assert.AreEqual(1, dimensions.Width);
            Assert.AreEqual(1, dimensions.Height);
        }
        
        [TestCase(0, 0, true)]
        [TestCase(-1, -1, false)]
        [TestCase(int.MinValue, int.MinValue, false)]
        public void Invalid_size_returns_null(int width, int height, bool withAlpha)
        {
            _sciterImage = SciterImage.Create(width, height, withAlpha);
            Assert.IsNull(_sciterImage);
        }

        #region Extension Tests
        
        [TestCase(1, 1, 1, 1)]
        [TestCase(127, 127, 127, 127)]
        [TestCase(255, 255, 255, 255)]
        public void Create_and_clear_image(byte r, byte g, byte b, byte alpha)
        {
            _sciterImage = SciterImage.Create(1, 1, true);
            _sciterImage.Clear(SciterColor.Create(r: r, g: g, b: b, a: alpha));
            
            var buffer = _sciterImage.Save(ImageEncoding.Raw);
            
            Assert.NotNull(_sciterImage);
            Assert.AreEqual(1 * 1 * 4, buffer.Length);
            Assert.AreEqual(new byte[] { (byte)(r / (byte.MaxValue / alpha)), (byte)(g / (byte.MaxValue / alpha)), (byte)(b / (byte.MaxValue / alpha)), alpha }, buffer);
        }
        
        [TestCase(1, 1, 1, 1)]
        [TestCase(127, 127, 127, 127)]
        [TestCase(255, 255, 255, 255)]
        public void Create_and_try_clear_image(byte r, byte g, byte b, byte alpha)
        {
            _sciterImage = SciterImage.Create(1, 1, true);
            var clearResult = _sciterImage.TryClear(SciterColor.Create(r: r, g: g, b: b, a: alpha));
            
            var buffer = _sciterImage.Save(ImageEncoding.Raw);
            
            Assert.NotNull(_sciterImage);
            Assert.IsTrue(clearResult);
            Assert.AreEqual(1 * 1 * 4, buffer.Length);
            Assert.AreEqual(new byte[] { (byte)(r / (byte.MaxValue / alpha)), (byte)(g / (byte.MaxValue / alpha)), (byte)(b / (byte.MaxValue / alpha)), alpha }, buffer);
        }
        
        [Test]
        public void Save_a_valid_png_image()
        {
            _sciterImage = SciterImage.Create(10, 10, false);
            var buffer = _sciterImage.Save(ImageEncoding.Png);

            var headerBuffer = new byte[8];
            Array.Copy(buffer, headerBuffer, Math.Min(headerBuffer.Length, buffer.Length));
            
            Assert.NotNull(_sciterImage);
            Assert.GreaterOrEqual(buffer.Length, headerBuffer.Length);
            Assert.AreEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10}, headerBuffer);
        }

        [Test]
        public void Save_a_valid_jpg_image_in_jfif_format()
        {
            _sciterImage = SciterImage.Create(10, 10, false);
            var buffer = _sciterImage.Save(ImageEncoding.Jpg);

            var headerBuffer = new byte[4];
            Array.Copy(buffer, headerBuffer, Math.Min(headerBuffer.Length, buffer.Length));
            
            Assert.NotNull(_sciterImage);
            Assert.GreaterOrEqual(buffer.Length, headerBuffer.Length);
            Assert.AreEqual(new byte[] { 255, 216, 255, 224}, headerBuffer);
        }

        [TestCase(10, 10, true)]
        [TestCase(5, 7, false)]
        public void Save_a_valid_raw_image(int width, int height, bool withAlpha)
        {
            _sciterImage = SciterImage.Create(width, height, withAlpha);
            var buffer = _sciterImage.Save(ImageEncoding.Raw);
            
            Assert.NotNull(_sciterImage);
            Assert.AreEqual(width * height * 4, buffer.Length);
            Assert.AreEqual(withAlpha ? 0 : 255, buffer[3]);
        }
        
        [TestCase(10, 10, true)]
        [TestCase(5, 7, false)]
        public void Try_save_a_valid_raw_image(int width, int height, bool withAlpha)
        {
            _sciterImage = SciterImage.Create(width, height, withAlpha);
            var result = _sciterImage.TrySave(out var buffer, ImageEncoding.Raw);
            
            Assert.NotNull(_sciterImage);
            Assert.IsTrue(result);
            Assert.AreEqual(width * height * 4, buffer.Length);
            Assert.AreEqual(withAlpha ? 0 : 255, buffer[3]);
        }
        
        [Test]
        public void Saves_a_valid_webp_image_in_riff_format()
        {
            _sciterImage = SciterImage.Create(10, 10, false);
            var buffer = _sciterImage.Save(ImageEncoding.WebP);

            var headerBuffer = new byte[4];
            Array.Copy(buffer, headerBuffer, Math.Min(headerBuffer.Length, buffer.Length));
            
            Assert.NotNull(_sciterImage);
            Assert.GreaterOrEqual(buffer.Length, headerBuffer.Length);
            Assert.AreEqual(System.Text.Encoding.UTF8.GetBytes("RIFF"), headerBuffer);
        }
        
        [Test]
        public void Get_dimensions()
        {
            _sciterImage = SciterImage.Create(1, 1, true);

            var dimensions = _sciterImage.GetDimensions();
            
            Assert.AreEqual(1, dimensions.Width);
            Assert.AreEqual(1, dimensions.Height);
        }

        [Test]
        public void Try_get_dimensions()
        {
            _sciterImage = SciterImage.Create(1, 1, true);

            var result = _sciterImage.TryGetDimensions(out var dimensions);
            
            Assert.NotNull(_sciterImage);
            Assert.IsTrue(result);
            Assert.AreEqual(1, dimensions.Width);
            Assert.AreEqual(1, dimensions.Height);
        }

        #endregion
    }
}