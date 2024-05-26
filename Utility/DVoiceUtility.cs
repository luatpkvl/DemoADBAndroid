using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhatsappAccount.Model;

namespace WhatsappAccount.Utility
{
    public class DVoiceUtility
    {
        /// <summary>
        /// Lấy ra order id hiện tại
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static async Task<string> GetCurrentOrderID(string privateKey)
        {
            string orderID = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"https://ggcode.site/request_number/?token={privateKey}&type=voice_mail");
                    // Read the response from the server as a string.
                    string content = await result.Content.ReadAsStringAsync();

                    orderID = content.Replace("\r\n", string.Empty);
                }

            }
            catch (Exception ex)
            {

            }
            return orderID;
        }
        /// <summary>
        /// Lấy ra số hiện tại
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static async Task<string> GetCurrentPhoneNumber(string privateKey,string orderID)
        {
            string phoneNumber = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"https://ggcode.site/check_order/?token={privateKey}&orderid={orderID}&action=get_phone");
                    // Read the response from the server as a string.
                    string content = await result.Content.ReadAsStringAsync();
                    phoneNumber = content.Replace("\r\n", string.Empty);
                }

            }
            catch (Exception ex)
            {

            }
            return phoneNumber;
        }
        /// <summary>
        /// Lấy ra thông tin message 
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static async Task<string> ReadMessageCode(string privateKey, string orderID)
        {
            string messageCode = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"https://ggcode.site/check_order/?token={privateKey}&orderid={orderID}&action=get_sms");
                    // Read the response from the server as a string.
                    string content = await result.Content.ReadAsStringAsync();
                    messageCode = content.Replace("\r\n", string.Empty);
                }

            }
            catch (Exception ex)
            {

            }
            return messageCode;
        }
    }
}
