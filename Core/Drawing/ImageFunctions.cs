using System.Drawing;
using System.Drawing.Drawing2D;

namespace Core.Drawing
{
    public static class ImageFunctions
    {
        public static Size CalculateMaxSize(Size currentSize, int maxWidth)
        {
            if (currentSize.Width < maxWidth)
                return currentSize;

            var height = (int)((decimal)maxWidth / currentSize.Width * currentSize.Height);

            return new Size(maxWidth, height);
        }

        public static Bitmap ResizeImage(Image image, Size size)
        {
            var result = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.DrawImage(image, 0, 0, size.Width, size.Height);
            }

            return result;
        }
    }
}