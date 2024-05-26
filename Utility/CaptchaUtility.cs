using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tesseract;
using Tesseract.Interop;

namespace WhatsappAccount.Utility
{
    public class CaptchaUtility
    {
        public static string ImageToBase64(string imagePath)
        {
            try
            {
                // Read the image bytes from the file path
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Convert the image bytes to a base64 string
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file reading or conversion
                Console.WriteLine($"Error converting image to base64: {ex.Message}");
                return null;
            }
        }
        public string ReadCaptchaNumber(string imagePath)
        {
            // Load the image from the file path using SixLabors.ImageSharp
            using (var image = Image.FromFile(imagePath))
            {
                // Remove the captcha number cross
                RemoveCaptchaCross(image);

                // Convert the image to a Pix object using Tesseract.Interop
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageData = memoryStream.ToArray();
                    var pixImage = Pix.LoadFromMemory(imageData);

                    // Set up the Tesseract engine
                    using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
                    {
                        // Set the image for OCR
                        using (var page = engine.Process(pixImage))
                        {
                            // Get the recognized text
                            string result = page.GetText().Trim();
                            return result;
                        }
                    }
                }
            }
        }

        private void RemoveCaptchaCross(Image image)
        {
            // Implement your cross removal logic using SixLabors.ImageSharp

            //// For example, you can use a simple rectangle to cover the cross:
            //var backgroundColor = SixLabors.ImageSharp.Color.White;
            //var rectangle = new Rectangle(100, 100, 200, 50);
            //image.Mutate(x => x.Fill(backgroundColor, rectangle));
        }


        public static void SaveBitmapAsImage(Bitmap bitmap, string filePath)
        {
            //----------------------------------------------------------
            // Create a BitmapSource from the Bitmap
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

            // Create a BitmapEncoder based on the file extension
            string fileExtension = Path.GetExtension(filePath).ToLower();
            BitmapEncoder encoder;
            switch (fileExtension)
            {
                case ".bmp":
                    encoder = new BmpBitmapEncoder();
                    break;
                case ".jpg":
                case ".jpeg":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                case ".gif":
                    encoder = new GifBitmapEncoder();
                    break;
                case ".tiff":
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new NotSupportedException($"File format {fileExtension} is not supported.");
            }

            // Set the BitmapSource as the encoder's frames
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            // Save the encoder's frames to the specified file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
        public static string ReadCaptchaNumberWithCross(string imagePath)
        {
            var ocrtext = string.Empty;

            Bitmap testBitMap = new Bitmap(imagePath);

            PreprocessCaptchaImage(testBitMap);
            SaveBitmapAsImage(testBitMap,"testbit.png");
            //using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            //{
            //    using (var img = Pix.LoadFromFile(imagePath))
            //    {
            //        using (var page = engine.Process(img))
            //        {
            //            ocrtext = page.GetText();
            //        }
            //    }
            //}

            return ocrtext;        
        }



        private static void PreprocessCaptchaImage(Bitmap image)
        {
            // Convert the image to grayscale
            Grayscale(image);

            // Threshold the image to emphasize the digits
            Threshold(image);

            // Dilate the image to fill gaps in the digits
            Dilate(image);
        }
        private static void Grayscale(Bitmap image)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][] {
                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 }
                    });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
                graphics.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
        }
        private static void Threshold(Bitmap image)
        {
            //using (var gr = Graphics.FromImage(image))
            //{
            //    var grayScale = ColorMatrixFilters.CreateColorMatrixForThreshold(150);
            //    gr.ApplyColorMatrix(grayScale);
            //}
        }
        private static void Dilate(Bitmap image)
        {
            int dilationSize = 2;
            var element = new Bitmap(dilationSize, dilationSize);

            using (Graphics graphics = Graphics.FromImage(element))
            {
                using (var brush = new SolidBrush(System.Drawing.Color.White))
                {
                    graphics.FillRectangle(brush, 0, 0, dilationSize, dilationSize);
                }
            }

            var rectangle = new Rectangle(0, 0, image.Width, image.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var attributes = new ImageAttributes())
                {
                    attributes.SetColorKey(System.Drawing.Color.White, System.Drawing.Color.White);
                    graphics.DrawImage(element, rectangle, 0, 0, element.Width, element.Height, GraphicsUnit.Pixel, attributes);
                }
            }
        }
    }
}
