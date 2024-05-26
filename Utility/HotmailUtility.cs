using Newtonsoft.Json;
using ProtonFather.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtonFather.Utility
{
    public static class HotmailUtility
    {
        /// <summary>
        /// Lấy về proton code        
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<string> GetProtonCode(string username, string password)
        {
            try
            {
                var client = new HttpClient();

                var result = await client.GetAsync($"http://example.com/api/AppleAccount/ReadProtonMail?username={username}&password={password}");

                string content = await result.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    return content;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Thêm tài khoản
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        public static async Task<string> AddAccount(string accountInfo,string agent)
        {
            try
            {
                var client = new HttpClient();

                var result = await client.GetAsync($"http://example.com/api/AppleAccount/AddProton?account={accountInfo}&agent={agent}");

                string content = await result.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    return content;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Tao mail moi'
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public static async Task<string> GetNewMail(string apiKey, string typeMail = "1")
        {
            string newMail = string.Empty;
            try
            {
                DongVanMailModel model = new DongVanMailModel();

                // Finally, create the HTTP client object
                var client = new HttpClient();

                var result = await client.GetAsync($"https://api.dongvanfb.net/user/buy?apikey={apiKey}&account_type={typeMail}&quality=1");

                string content = await result.Content.ReadAsStringAsync();

                if (content != null)
                {
                    model = JsonConvert.DeserializeObject<DongVanMailModel>(content);

                    if (model != null && model.data != null)
                    {
                        if (model.data.list_data != null && model.data.list_data.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.data.list_data[0]))
                            {
                                newMail = model.data.list_data[0];
                            }
                            else
                            {
                                return await GetOutlook(apiKey, "2");
                            }
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
                return newMail;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Lấy mail outlook
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="typeMail"></param>
        /// <returns></returns>
        public static async Task<string> GetOutlook(string apiKey, string typeMail = "1")
        {
            string newMail = string.Empty;
            try
            {
                DongVanMailModel model = new DongVanMailModel();

                // Finally, create the HTTP client object
                var client = new HttpClient();

                var result = await client.GetAsync($"https://api.dongvanfb.net/user/buy?apikey={apiKey}&account_type={typeMail}&quality=1");

                string content = await result.Content.ReadAsStringAsync();

                if (content != null)
                {
                    model = JsonConvert.DeserializeObject<DongVanMailModel>(content);

                    if (model != null && model.data != null)
                    {
                        if (model.data.list_data != null && model.data.list_data.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(model.data.list_data[0]))
                            {
                                newMail = model.data.list_data[0];
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
                return newMail;
            }
            catch
            {
                return null;
            }
        }
    }
}
