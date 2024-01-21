using ImageMagick;
using ImageResizerApi.Factories;
using ImageResizerApi.Model;
using System.Drawing;
using System.Formats.Tar;
using static System.Net.Mime.MediaTypeNames;

namespace ImageResizerApi.Service
{
    public class ImageResizerService
    {
        public static void resize(byte[] imagebytes, int width, int height, string targetPath)
        {
            using (var imageMagick = new MagickImage(imagebytes))
            {
                MagickGeometry geometry = new MagickGeometry(width, height);
                geometry.IgnoreAspectRatio = false;

                imageMagick.Resize(width, height);
                //add customizations
                imageMagick.Write(targetPath);
            }
        }

        public static void resize(byte[] imagebytes, int? width, int? height, string targetPath, FillMode fill, string extention)
        {

            using (var imageMagick = new MagickImage(imagebytes))
            {  

                MagickGeometry geometry = new MagickGeometry(imageMagick.BaseWidth, imageMagick.BaseHeight);
                if (height != 0)
                    geometry.Height = height.Value;

                if (width != 0)
                    geometry.Width = width.Value;


                if (height.HasValue && ( !width.HasValue || width.Value == 0)) // if only the height was supplied, calculate the width by finding the ration
                {
                    float ratio = 0f;
                    ratio =  (float) height.Value / (float)imageMagick.BaseHeight;
                    geometry.Width = (int)(imageMagick.BaseWidth * ratio);

                }

                geometry.IgnoreAspectRatio = false;

                imageMagick.Resize(geometry);

                if (geometry.Width >= geometry.Height && ((int)(geometry.Width / geometry.Height) == 1)) // aspect ration is 1:1
                    imageMagick.Settings.Compression = CompressionMethod.DWAB; //losseless 

                if (height.HasValue && height != 0 && width.HasValue && width != 0) // fill the image to preserve the new aspect ration
                {
                    //fills with a white background
                    imageMagick.BackgroundColor = MagickColor.FromRgb(255, 255, 255); //FillFactory.FillSetting(imagebytes, fill);

                    imageMagick.Extent(geometry, Gravity.Center);
                }

                imageMagick.Quality = 80;

                //add customizations
                imageMagick.Write(targetPath);
            }
        }

        public static string RenameResizedFile(string fileName, int width, int height) //format ***.***
        {
            var extention = fileName.Substring(fileName.IndexOf('.')+1);
            var fileNameWithoutExtention = fileName.Substring(0, fileName.IndexOf("."));
            return $"{fileNameWithoutExtention}_{width}_{height}.{extention}";
        }

        public static IMagickColor<ushort> GetDominantColor(byte[] image)
        {
            using (var dominantColor = new MagickImage(image))
            {
                dominantColor.Resize(1, 1); //to get the most dominant color
                dominantColor.Alpha(AlphaOption.Off);
                return dominantColor.Histogram().OrderBy(e => e.Value).FirstOrDefault().Key;
            }
        }
    }
}
