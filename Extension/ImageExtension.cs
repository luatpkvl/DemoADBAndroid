using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Extension
{
    public static class ImageExtension
    {
        /// <summary>
        /// Laasy ra ma mau
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorCode(this Rgba32 color)
        {
            string colorCode = string.Empty;

            if (color != null)
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }

            return colorCode;
        }
    }
}
