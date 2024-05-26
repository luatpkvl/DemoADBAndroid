using KAutoHelper;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Point = SixLabors.ImageSharp.Point;
using System.Linq;
using Image = SixLabors.ImageSharp.Image;
using Amazon.SimpleDB.Model;

namespace WhatsappAccount.Utility
{
    public static class ADBUtility
    {
        private static string LIST_DEVICES = "adb devices";

        private static string TAP_DEVICES = "adb -s {0} shell input tap {1} {2}";

        private static string SWIPE_DEVICES = "adb -s {0} shell input swipe {1} {2} {3} {4} {5}";

        private static string KEY_DEVICES = "adb -s {0} shell input keyevent {1}";

        private static string INPUT_TEXT_DEVICES = "adb -s {0} shell input text \"{1}\"";

        private static string CAPTURE_SCREEN_TO_DEVICES = "adb -s {0} shell screencap -p \"{1}\"";

        private static string PULL_SCREEN_FROM_DEVICES = "adb -s {0} pull \"{1}\"";

        private static string REMOVE_SCREEN_FROM_DEVICES = "adb -s {0} shell rm -f \"{1}\"";

        private static string GET_SCREEN_RESOLUTION = "adb -s {0} shell dumpsys display | Find \"mCurrentDisplayRect\"";

        private const int DEFAULT_SWIPE_DURATION = 100;

        private static string ADB_FOLDER_PATH = "";

        private static string ADB_PATH = "";
        /// <summary>
        /// Load ra device
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <returns></returns>
        public static async Task<List<string>> GetDevices()
        {
            List<string> list = new List<string>();
            //ExecuteCMD("adb kill-server");
            //ExecuteCMD("adb start-server");

            //ExecuteCMD("adb devices");
            string input = await ExecuteCMD("adb devices");
            string pattern = "(?<=List of devices attached)([^\\n]*\\n+)+";
            MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
            if (matchCollection.Count > 0)
            {
                string value = matchCollection[0].Groups[0].Value;
                string[] array = Regex.Split(value, "\r\n");
                string[] array2 = array;
                foreach (string text in array2)
                {
                    if (string.IsNullOrEmpty(text) || !(text != " "))
                    {
                        continue;
                    }

                    string[] array3 = text.Trim().Split('\t');
                    string text2 = array3[0];
                    string text3 = "";
                    try
                    {
                        text3 = array3[1];
                        if (text3 != "device")
                        {
                            continue;
                        }
                    }
                    catch
                    {
                    }

                    list.Add(text2.Trim());
                }
            }

            return list;
        }
        /// <summary>
        /// Input text slow
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task InputTextSlow(string deviceID, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var arrText = input.ToCharArray();

                foreach (var itemChar in arrText)
                {
                    await Task.Delay(new Random().Next(400, 800));
                    await InputText(deviceID, itemChar.ToString());
                }
            }
        }

        public static async Task<Image<Rgba32>> ScreenShoot(string deviceID = null, bool isDeleteImageAfterCapture = true, string fileName = "screenShoot.png")
        {
            if (string.IsNullOrEmpty(deviceID))
            {
                List<string> devices = await GetDevices();
                if (devices == null || devices.Count <= 0)
                {
                    return null;
                }

                deviceID = devices.First();
            }

            string text = deviceID.Replace(".", "").Replace(":", "");
            try
            {
                text = deviceID.Replace(".", "").Replace(":", ""); ;
            }
            catch
            {
            }
            if (!isDeleteImageAfterCapture)
            {
                text = Guid.NewGuid().ToString();
            }

            string text2 = Path.GetFileNameWithoutExtension(fileName) + text + Path.GetExtension(fileName);
            if (File.Exists(text2))
            {
                try
                {
                    File.Delete(text2);
                }
                catch (Exception)
                {
                }
            }

            string filename = Directory.GetCurrentDirectory() + "\\" + text2;
            string text3 = Directory.GetCurrentDirectory().Replace("\\\\", "\\");
            text3 = "\"" + text3 + "\"";
            string cmdCommand = string.Format("adb -s {0} shell screencap -p \"{1}\"", deviceID, "/sdcard/" + text2);
            string cmdCommand2 = string.Format("adb -s " + deviceID + " pull /sdcard/" + text2 + " " + text3);
            //Delete all image from mobile
            string cmdDeleteAllImage = string.Format("adb -s {0} shell rm \"{1}\"", deviceID, "/sdcard/*.png");
            string text4 = await ExecuteCMD(cmdCommand);
            //string textCMD = await ExecuteCMD(cmdDeleteAllImage);
            string text5 = await ExecuteCMD(cmdCommand2);
            Image<Rgba32> result = null;
            try
            {
                Image<Rgba32> image = await Image.LoadAsync<Rgba32>(filename);
                result = image;
            }
            catch
            {
            }

            if (isDeleteImageAfterCapture)
            {
                try
                {
                    File.Delete(text2);
                }
                catch
                {
                }

                try
                {
                    string cmdCommand3 = string.Format("adb -s " + deviceID + " shell \"rm /sdcard/" + text2 + "\"");
                    string text6 = await ExecuteCMD(cmdCommand3);
                }
                catch
                {
                }
            }

            return result;
        }

        private static byte[] ConvertOutputToByteArray(string output)
        {
            // Remove the line breaks in the output
            output = output.Replace("\r\n", "\n");

            // Remove the header from the output (the first line)
            int startIndex = output.IndexOf('\n') + 1;
            output = output.Substring(startIndex);

            // Convert the output from Base64 to a byte array
            byte[] byteArray = Convert.FromBase64String(output);

            return byteArray;
        }
        /// <summary>
        /// Truyền key
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task Key(string deviceID, ADBKey key)
        {
            string cmdCommand = string.Format(KEY_DEVICES, deviceID, key);
            string text = await ExecuteCMD(cmdCommand);
        }

        // <summary>
        /// wipe
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="duration"></param>
        public static async Task SwipeByPercent(string deviceID, double x1, double y1, double x2, double y2, int duration = 100)
        {
            Point screenResolution = await GetScreenResolution(deviceID);
            int num = (int)(x1 * ((double)screenResolution.X * 1.0 / 100.0));
            int num2 = (int)(y1 * ((double)screenResolution.Y * 1.0 / 100.0));
            int num3 = (int)(x2 * ((double)screenResolution.X * 1.0 / 100.0));
            int num4 = (int)(y2 * ((double)screenResolution.Y * 1.0 / 100.0));
            string cmdCommand = string.Format(SWIPE_DEVICES, deviceID, num, num2, num3, num4, duration);
            string text = await ExecuteCMD(cmdCommand);
        }
        /// <summary>
        /// Click vào tọa độ
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async static Task Tap(string deviceID, int x, int y, int count = 1)
        {
            if (string.IsNullOrEmpty(deviceID)) return;
            StringBuilder sb = new StringBuilder();
            string text = string.Format(TAP_DEVICES, deviceID, x, y).Truncate(250);
            sb.Append(text);
            for (int i = 1; i < count; i++)
            {
                sb.Append(" && " + string.Format(TAP_DEVICES, deviceID, x, y).Truncate(250));
            }

            string text2 = await ExecuteCMD(sb.ToString());
        }
        public static async Task<Point> GetScreenResolution(string deviceID)
        {
            try
            {
                string cmdCommand = string.Format(GET_SCREEN_RESOLUTION, deviceID);
                string text = await ExecuteCMD(cmdCommand);
                if (text.IndexOf("- ") > 0)
                {
                    text = text.Substring(text.IndexOf("- "));
                }
                else
                {
                    return new Point(1080, 1920);
                }
                if (text.IndexOf(' ') > 0 && text.IndexOf(')') > 0)
                {
                    text = text.Substring(text.IndexOf(' '), text.IndexOf(')') - text.IndexOf(' '));
                }
                else
                {
                    return new Point(1080, 1920);
                }

                string[] array = text.Split(',');
                int x = Convert.ToInt32(array[0].Trim());
                int y = Convert.ToInt32(array[1].Trim());
                return new Point(x, y);
            }
            catch
            {
                return new Point(1080, 1920);
            }

        }
        /// <summary>
        /// Nhập text truyền từ thiết bị
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="text"></param>
        public static async Task InputText(string deviceID, string text)
        {
            string cmdCommand = string.Format(INPUT_TEXT_DEVICES, deviceID, text.Replace(" ", "%s").Replace("&", "\\&").Replace("<", "\\<")
                .Replace(">", "\\>")
                .Replace("?", "\\?")
                .Replace(":", "\\:")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("|", "\\|"));
            string text2 = await ExecuteCMD(cmdCommand);
        }
        /// <summary>
        /// Click vào vị trí trên màn hình
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        public static async Task TapByPercent(string deviceID, double x, double y, int count = 1)
        {
            if (string.IsNullOrEmpty(deviceID)) return;
            Point screenResolution = await GetScreenResolution(deviceID);
            int num = (int)(x * ((double)screenResolution.X * 1.0 / 100.0));
            int num2 = (int)(y * ((double)screenResolution.Y * 1.0 / 100.0));
            StringBuilder sb = new StringBuilder();
            string text = string.Format(TAP_DEVICES, deviceID, num, num2);
            sb.Append(text);
            for (int i = 1; i < count; i++)
            {
                sb.Append(" && " + string.Format(TAP_DEVICES, deviceID, x, y).Truncate(250));
            }

            string text2 = await ExecuteCMD(sb.ToString());
        }
        /// <summary>
        /// Xóa sạch dữ liệu app
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="deviceID"></param>
        public static void ClearAppData(string packageName, string deviceID)
        {
            ExecuteCMD($"adb -s {deviceID} shell pm clear {packageName}");
        }
        /// <summary>
        /// Xoá toàn bộ video trong folder download
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task DeleteAllVideo(string deviceID)
        {
            if (string.IsNullOrEmpty(deviceID)) { return; }
            string command = $"adb -s {deviceID} shell find \"/sdcard/Download/\" -name '*.mp4' -type f -delete";
            await ExecuteCMD(command);
        }
        /// <summary>
        /// Đẩy video
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task PushVideo(string deviceID, string fileName)
        {
            if (string.IsNullOrEmpty(deviceID)) { return; }
            string videoFilePath = AppDomain.CurrentDomain.BaseDirectory + $"/video/{fileName}";
            string destinationPath = $"/sdcard/Download/{fileName}";
            string adbShell = $"adb -s {deviceID}  push \"{videoFilePath}\" \"{destinationPath}\"";
            await ADBUtility.ExecuteCMD(adbShell);
            string showVideo = $"adb -s {deviceID} shell am start -a android.intent.action.VIEW -d file:///sdcard/Download/{fileName}";
            await ADBUtility.ExecuteCMD(showVideo);
        }
        public static async Task<string> ExecuteCMD(string cmdCommand)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WorkingDirectory = "";
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.Verb = "runas";
                process.StartInfo = processStartInfo;
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                return await process.StandardOutput.ReadToEndAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public static void SendCommandToDevice(string deviceName, string command)
        {
            Process process = CreateADBProcess($"-s {deviceName} {command}");
            process.Start();
            process.WaitForExit();
        }

        private static Process CreateADBProcess(string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "adb.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.StartInfo = startInfo;


            return process;
        }
    }
}
