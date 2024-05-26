using Amazon.DeviceFarm.Model;
using Amazon.ECS.Model;
using Newtonsoft.Json;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhatsappAccount.Entity;
using WhatsappAccount.Model;
using static Amazon.S3.Util.S3EventNotification;

namespace WhatsappAccount.Utility
{
    public class Captcha69Utility
    {
        /// <summary>
        /// Đọc captcha
        /// created by: ltluat 01.07.2023
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="captCha69Key"></param>
        /// <returns></returns>
        public static async Task<string> ReadCaptcha(string deviceID, string captCha69Key)
        {
            string codeCaptcha = string.Empty;
            try
            {
                int x = 170;
                int y = 243;
                int width = 203;
                int height = 84;

                Image<Rgba32> image = await ADBUtility.ScreenShoot(deviceID);

                string base64Str = await ImageUtility.GetBase64ImageByPosition(image, x, y, width, height);

                string idReadCaptcha = await Captcha69Utility.GetCaptchaId(captCha69Key, base64Str);

                if (!string.IsNullOrEmpty(idReadCaptcha))
                {
                    codeCaptcha = await Captcha69Utility.ReadCaptChaNumber(captCha69Key, idReadCaptcha);
                }
            }
            catch (Exception ex)
            {
            }
            return codeCaptcha;
        }
        /// <summary>
        /// Tiến hành đọc captcha code 
        /// created by: ltluat 01.07.2023
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static async Task<string> GetCaptchaId(string key, string base64Img)
        {
            string idReadCaptcha = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    InCaptcha69Request request = new InCaptcha69Request() { key = key, method = "base64", body = base64Img };
                    string jsonData = JsonConvert.SerializeObject(request);

                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://captcha69.com/in.php", content);

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent) && responseContent.ToUpper().Contains("OK|"))
                    {
                        return responseContent.Replace("OK|", "");
                    }
                }
            }
            catch (Exception e)
            {

            }
            return idReadCaptcha;
        }
        /// <summary>
        /// Get captchar Res
        /// </summary>
        /// <param name="key"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static async Task<string> ReadCaptChaNumber(string key, string id)
        {
            string captchaCode = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ResCaptcha69Request request = new ResCaptcha69Request() { key = key, action = "get", id = id };
                    string jsonData = JsonConvert.SerializeObject(request);

                    var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://captcha69.com/res.php", content);

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent) && responseContent.ToUpper().Contains("OK|"))
                    {
                        return responseContent.Replace("OK|", "");
                    }
                }
            }
            catch (Exception e)
            {

            }
            return captchaCode;
        }
    }
}
