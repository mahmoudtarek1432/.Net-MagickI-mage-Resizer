using ImageMagick;
using ImageResizerApi.Model;

namespace ImageResizerApi.Factories
{
    public class FillFactory
    {
        public static IMagickColor<ushort> FillSetting(byte[] imagebytes, FillMode mode)
        {
            IMagickColor<ushort> color = null;
            switch (mode)
            {
                case FillMode.DominantColor:
                    using (var dominantColor = new MagickImage(imagebytes))
                    {
                        dominantColor.Resize(1, 1); //to get the most dominant color
                        dominantColor.Alpha(AlphaOption.Off);
                        color = dominantColor.Histogram().OrderBy(e => e.Value).FirstOrDefault().Key;
                    }
                    break;
                case FillMode.Black:
                    color = MagickColor.FromRgb(0, 0, 0);
                    break;
                case FillMode.White:
                    color = MagickColor.FromRgb(255, 255, 255);
                    break;
                case FillMode.None:
                    color = MagickColor.FromRgba(0, 0, 0, 0);
                    break;

            }

            return color;
        }
    }
}
