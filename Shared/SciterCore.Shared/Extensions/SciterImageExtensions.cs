using System.Drawing;

namespace SciterCore
{
    public static class SciterImageExtensions
    {

        #region Save

        /// <summary>
        /// Save this image to png/jpeg/WebP stream of bytes
        /// </summary>
        /// <param name="encoding">The output image type</param>
        /// <param name="quality">png: 0, jpeg/WebP: 10 - 100</param>
        public static byte[] Save(this SciterImage sciterImage, ImageEncoding encoding, uint quality = 0)
        {
            return sciterImage?.SaveInternal(encoding: encoding, quality: quality);
        }

        /// <summary>
        /// Save this image to png/jpeg/WebP stream of bytes
        /// </summary>
        /// <param name="encoding">The output image type</param>
        /// <param name="quality">png: 0, jpeg/WebP: 10 - 100</param>
        public static bool TrySave(this SciterImage sciterImage, out byte[] buffer, ImageEncoding encoding, uint quality = 0)
        {
            buffer = null;
            return sciterImage?.TrySaveInternal(buffer: out buffer, encoding: encoding, quality: quality) == true;
        }

        #endregion Save
        public static Size GetDimensions(this SciterImage sciterImage)
        {
            return sciterImage?.GetDimensionsInternal() ?? default;
        }

        public static bool TryGetDimensions(this SciterImage sciterImage, out Size size)
        {
            size = default;
            return sciterImage?.TryGetDimensionsInternal(out size) == true;
        }

        public static void Clear(this SciterImage sciterImage, SciterColor color)
        {
            sciterImage?.ClearInternal(color);
        }

        public static bool TryClear(this SciterImage sciterImage, SciterColor color)
        {
            return sciterImage?.TryClearInternal(color) == true;
        }
    }
}