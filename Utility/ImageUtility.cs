using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WhatsappAccount.Extension;
using WhatsappAccount.Model;

namespace WhatsappAccount.Utility
{
    public static class ImageUtility
    {
        /// <summary>
        /// Chuyển dạng ảnh về base64
        /// created by: ltluat 01.07.2023
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static async Task<string> ImageToBase64(Image<Rgba32> image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the image to the memory stream in PNG format
                await image.SaveAsync(memoryStream, new PngEncoder());

                // Convert the image bytes to a base64 string
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }
        /// <summary>
        /// lấy thông tin ảnh theo vị trí
        /// created by: ltluat 01.07.2023
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<string> GetBase64ImageByPosition(Image<Rgba32> image, int x, int y, int width, int height)
        {
            MemoryStream memoryStream = new MemoryStream();
            // Create a PngEncoder
            PngEncoder pngEncoder = new PngEncoder();
            // Convert Image<Rgba32> to byte array
            await image.SaveAsync(memoryStream, pngEncoder);

            // Create a BitmapDecoder from the MemoryStream
            BitmapDecoder bitmapDecoder = BitmapDecoder.Create(memoryStream,
                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            // Create a CroppedBitmap based on the original image and the specified position and dimensions
            Int32Rect cropRect = new Int32Rect(x, y, width, height);
            CroppedBitmap croppedBitmap = new CroppedBitmap(bitmapDecoder.Frames[0], cropRect);

            // Create a PngBitmapEncoder
            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

            return ImageToBase64(bitmapEncoder);
        }
        /// <summary>
        /// Chuyển dạng bitmap encoder về base64 chuôi
        /// created by: ltluat 01.07.2023
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns></returns>
        private static string ImageToBase64(PngBitmapEncoder encoder)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the image to the memory stream
                encoder.Save(memoryStream);

                // Convert the image bytes to a base64 string
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }
        /// <summary>
        /// Lưu ảnh cut theo vị trí
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task SaveImageByPosition(Image<Rgba32> image, int x, int y, int width, int height, string filePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            // Create a PngEncoder
            PngEncoder pngEncoder = new PngEncoder();
            // Convert Image<Rgba32> to byte array
            await image.SaveAsync(memoryStream, pngEncoder);

            // Create a BitmapDecoder from the MemoryStream
            BitmapDecoder bitmapDecoder = BitmapDecoder.Create(memoryStream,
                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

            // Create a CroppedBitmap based on the original image and the specified position and dimensions
            Int32Rect cropRect = new Int32Rect(x, y, width, height);
            CroppedBitmap croppedBitmap = new CroppedBitmap(bitmapDecoder.Frames[0], cropRect);

            // Create a PngBitmapEncoder
            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

            // Save the cropped image to the file
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                bitmapEncoder.Save(fileStream);
            }
            memoryStream.Close();
        }
        /// <summary>
        /// Lưu thành dạng file
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task SaveImageToFile(Image<Rgba32> image, string filePath)
        {
            // Convert Image<Rgba32> to byte array
            await image.SaveAsPngAsync(filePath);

        }
        /// <summary>
        /// so sánh màn hình xuất hiện
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static async Task<bool> CompareScreen(string deviceID, ColorScreen compare)
        {
            bool isMatch = false;
            try
            {
                using (var imager = await ADBUtility.ScreenShoot(deviceID))
                {
                    if(imager != null)
                    {
                        //compare 1
                        Rgba32 pixelColorCompare1 = imager[compare.PosX1, compare.PosY1];
                        string colorCodeCompare1 = pixelColorCompare1.ColorCode();
                        //Compare 2
                        Rgba32 pixelColorCompare2 = imager[compare.PosX2, compare.PosY2];
                        string colorCodeCompare2 = pixelColorCompare2.ColorCode();

                        if (colorCodeCompare1.Trim().ToUpper() == compare.Color1.Trim().ToUpper() && colorCodeCompare2.Trim().ToUpper() == compare.Color2.Trim().ToUpper())
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }
            catch (Exception e)
            {
                isMatch = false;
            }

            return isMatch;
        }
    }
}
