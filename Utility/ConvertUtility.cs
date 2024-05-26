using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WhatsappAccount.Utility
{
    public class ConvertUtility
    {
        public static async Task<string> GetTypePhone()
        {
            string speechKey = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"http://example.com/api/AppleAccount/GetConfigKey?key=typePhone9999");
                    // Read the response from the server as a string.
                    string content = await result.Content.ReadAsStringAsync();
                    speechKey = content;
                }

            }
            catch (Exception ex)
            {

            }
            return speechKey;
        }
        /// <summary>
        /// đọc system key
        /// </summary>
        /// <param name="systemKey"></param>
        /// <returns></returns>
        public static async Task<string> GetSystemKey(string systemKey)
        {
            string speechKey = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync($"http://example.com/api/AppleAccount/GetConfigKey?key={systemKey}");
                    // Read the response from the server as a string.
                    string content = await result.Content.ReadAsStringAsync();                    
                    speechKey = content;                    
                }

            }
            catch(Exception ex)
            {

            }
            return speechKey;
        }
        public static async Task<string> ConvertAudioVietnam(string subscriptionKey, string filePath)
        {                       
            string region = "southeastasia"; // Replace with your region (e.g., "westus", "eastus", etc.)

            var config = SpeechConfig.FromSubscription(subscriptionKey, region);
            config.SpeechRecognitionLanguage = "vi-VN";
            config.SetProperty(PropertyId.AudioConfig_PlaybackBufferLengthInMs,"20");
            string tempFileName = await SaveFileAudio(filePath);

            string fullTempFilePath = AppDomain.CurrentDomain.BaseDirectory + $"Temp/{tempFileName}.wav";


            StringBuilder sbTextCode = new StringBuilder();
            int limitTimeGetCode = 0;
            using (var audioInput = AudioConfig.FromWavFileInput(fullTempFilePath))
            //using (var audioInput = await CreateAudioConfigFromUrl(filePath))
            using (var recognizer = new SpeechRecognizer(config, audioInput))
            {
            getCode:
                var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    sbTextCode.Append(result.Text).ToString();
                    string textCode = sbTextCode.ToString();

                    if (!string.IsNullOrEmpty(textCode))
                    {
                        textCode = textCode.ConvertToUnSign2();
                        textCode = textCode.ToUpper();
                        textCode = textCode.Replace("MOT", "1");
                        textCode = textCode.Replace("HAI", "2");
                        textCode = textCode.Replace("BA ", "3");
                        textCode = textCode.Replace("BON", "4");
                        textCode = textCode.Replace("NAM", "5");
                        textCode = textCode.Replace("SAU", "6");
                        textCode = textCode.Replace("BAY", "7");
                        textCode = textCode.Replace("TAM", "8");
                        textCode = textCode.Replace("CHIN", "9");
                        textCode = textCode.Replace("KHONG", "0");

                        textCode = textCode.Replace(" ", "");
                        textCode = textCode.Replace(".", "");
                        textCode = textCode.Replace(",", "");
                        textCode = textCode.GetNumberCodeFromText();
                    }
                    if (string.IsNullOrEmpty(textCode))
                    {
                        limitTimeGetCode++;

                        if (limitTimeGetCode > 5)
                        {
                            return string.Empty;
                        }
                        else
                        {
                            goto getCode;
                        }
                    }
                    Console.WriteLine($"Transcription: {result.Text}");
                    try
                    {
                        File.Delete(fullTempFilePath);
                    }
                    catch
                    {

                    }
                    return textCode;
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    Console.WriteLine("No speech recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    Console.WriteLine($"Recognition canceled: {cancellation.Reason}");
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"Cancellation error details: {cancellation.ErrorDetails}");
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Lưu file dữ liệu
        /// created by: ltluat 04.06.2023
        /// </summary>
        /// <param name="fileUri"></param>
        /// <returns></returns>
        public static async Task<string> SaveFileAudio(string fileUri)
        {
            string fileName = String.Empty;
            try
            {
                // Create an HttpClient to download the file
                using (HttpClient client = new HttpClient())
                {
                    // Download the file content
                    byte[] fileContent = await client.GetByteArrayAsync(fileUri);
                    string fileDirectoryPath = AppDomain.CurrentDomain.BaseDirectory + "Temp";
                    if (!Directory.Exists(fileDirectoryPath))
                    {
                        Directory.CreateDirectory(fileDirectoryPath);
                    }
                    string fileNameTemp = Guid.NewGuid().ToString();
                    // Specify the file path to save to
                    string filePath = fileDirectoryPath + $"/{fileNameTemp}.wav";

                    // Save the file to the specified path
                    await File.WriteAllBytesAsync(filePath, fileContent);

                    Console.WriteLine("File saved successfully!");

                    return fileNameTemp;
                }
            }
            catch (Exception ex)
            {

            }
            return fileName;
        }
    }
}
