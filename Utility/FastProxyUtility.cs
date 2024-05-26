using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProtonFather.Utility
{
    public class FastProxyUtility
    {
        public static async Task<string> ChangeIP(string port,string adminKeyport,string apiKey)
        {
            try
            {
                var client = new HttpClient();

                var result = await client.GetAsync($"https://api-socks.fastproxy.vip/api/v1/proxies/{adminKeyport}/ip?port={port}&api_key={apiKey}");

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
    }
}
