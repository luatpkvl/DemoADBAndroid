using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WhatsappAccount
{
    public class WhatsappPhoneUtility
    {
        /// <summary>
        /// Thêm số điện thoại mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task InsertNewPhoneNumber(WhatsappNumberEntity entity)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        string jsonData = JsonConvert.SerializeObject(entity);

                        var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("http://example.com/api/Whatsapp/InsertNewNumber", content);

                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// cập nhật lại trạng thái của số điện thoại
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task UpdatePhoneNumber(WhatsappNumberEntity entity)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        string jsonData = JsonConvert.SerializeObject(entity);

                        var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync("http://example.com/api/Whatsapp/UpdateNumber", content);

                        response.EnsureSuccessStatusCode();

                        var responseContent = await response.Content.ReadAsStringAsync();

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
