using ImageMagick;

namespace ImageResizerApi.Factories
{
    public class CompressionFactory
    {
        public static void Compress(MagickImage image, string extention) //outputs the path after compression
        {
           switch (extention)
            {
                case "jpg":
                    image.Settings.Compression = CompressionMethod.JPEG;
                    break;
                case "webp":
                    image.Settings.Compression = CompressionMethod.WebP;
                    break;
                case "jpeg":
                    break;
                case "png":
                    break;
                default: throw new ArgumentException("Extention not supported");
            }
        }

        public static string ChangeExtention(string path, string extention)
        {
            return $"{path.Substring(0, path.LastIndexOf('.'))}.{extention}";
        }
    }
}
