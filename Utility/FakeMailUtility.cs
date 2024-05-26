using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FULiveAutoApp.Utility
{
    public class FakeMailUtility
    {
        public static async Task<string> GetCode(string urlPath)
        {
            string codeReturn = string.Empty;
            try
            {
                // Create an instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        // Send a GET request to the specified URL
                        HttpResponseMessage response = await httpClient.GetAsync(urlPath);

                        // Ensure the response is successful
                        response.EnsureSuccessStatusCode();

                        // Read the content asynchronously
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseBody))
                        {
                            Regex rx = new Regex(@"font-weight: bold"">(.*?)</span>.");
                            string newCode = rx.Match(responseBody).Groups[0].Value.Replace("font-weight: bold\">", "").Replace("</span>.", "").Trim();

                            if (!string.IsNullOrEmpty(newCode))
                            {
                                return newCode;
                            }
                        }
                        // Output the content
                        Console.WriteLine(responseBody);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine($"Error fetching content: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {

            }
            return codeReturn;
        }
    }
}
