using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WhatsappAccount.Model;

namespace WhatsappAccount.Utility
{
    public static class XOtpUtility
    {
        /// <summary>
        /// Kiểm tra đã hủy số hay là chưa
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> IsCanceledNumber(string otpKey, string removeID)
        {
            bool isCanceled = false;
            try
            {
                if (!string.IsNullOrEmpty(removeID))
                {
                    //Kiểm tra trạng thái của số
                    PhoneInfo currentInfo = await GetCurrentPhoneInfo(otpKey, removeID);

                    if (currentInfo != null && !string.IsNullOrEmpty(currentInfo.status) && currentInfo.status.ToUpper() != "ACTIVE" && currentInfo.status.ToUpper() != "OK")
                    {
                        isCanceled = true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return isCanceled;
        }
        /// <summary>
        /// Lấy số mới
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <returns></returns>
        public static async Task<PhoneInfo> GetNewPhoneInfo(string otpKey)
        {
            PhoneInfo phoneInfo = new PhoneInfo();

            try
            {
                using (HttpClient client = new HttpClient())
                {

                    var resultNumber = await client.GetAsync($"https://xotp.pro/api/v1/create-request?apikey={otpKey}&service=voicemail");

                    if (resultNumber != null)
                    {
                        string contentBalance = await resultNumber.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(contentBalance))
                        {
                            PhoneInfo info = JsonConvert.DeserializeObject<PhoneInfo>(contentBalance);
                            info.created_date = DateTime.Now;
                            return info;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return phoneInfo;
        }
        /// <summary>
        /// Lấy ra thông tin số hiện tại
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="otpKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<PhoneInfo> GetCurrentPhoneInfo(string otpKey, string id)
        {
            PhoneInfo phoneInfo = new PhoneInfo();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var resultInfo = await client.GetAsync($"https://xotp.pro/api/v1/get-request?apikey={otpKey}&id={id}");
                    if (resultInfo != null)
                    {
                        string currentInfo = await resultInfo.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(currentInfo))
                        {
                            phoneInfo = JsonConvert.DeserializeObject<PhoneInfo>(currentInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return phoneInfo;
        }
        /// <summary>
        /// Xóa số hiện tại
        /// </summary>
        /// <param name="otpKey"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task RemovePhoneInfo(string otpKey, string id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"https://xotp.pro/api/v1/cancel-request?apikey={otpKey}&id={id}");
                    // Read the response from the server as a string.
                    string read = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
