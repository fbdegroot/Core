using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Core.Drawing.Extensions
{
    public static class BitmapExtensions
    {
        public static void SaveJpeg(this Image image, string path, int quality = 100) {
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var codec = ImageCodecInfo.GetImageEncoders()[1];
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            using (var stream = new MemoryStream()) {
                image.Save(stream, codec, encoderParams);
                var bytes = stream.ToArray();

                Directory.CreateDirectory(FileFunctions.GetDirectory(path));
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))                    
                    fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}