using Amazon.Lambda;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using WhatsappAccount.Entity;
using WhatsappAccount.Model;

namespace WhatsappAccount.Utility
{
    public static class WhatsappAccountUtility
    {
        public static string GetSerialNumber()
        {
            // Create a new process to run the command
            Process process = new Process();

            // Set the process start info
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;

            // Start the process
            process.StartInfo = startInfo;
            process.Start();

            // Send the command to the command prompt
            process.StandardInput.WriteLine("wmic diskdrive get serialnumber");
            process.StandardInput.WriteLine("exit");

            // Read the output of the command
            string output = process.StandardOutput.ReadToEnd();

            // Parse the serial number from the output
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string serialNumber = null;

            foreach (string line in lines)
            {
                if (line.Trim() != "SerialNumber")
                {
                    serialNumber = line.Trim();
                    break;
                }
            }

            // Display the serial number
            Console.WriteLine("Serial Number: " + serialNumber);

            // Wait for the user to press Enter before exiting
            Console.ReadLine();
            return serialNumber;
        }
        /// <summary>
        /// Đọc serial number
        /// created by: ltluat 22.06.2023
        /// </summary>
        /// <returns></returns>
        public static async Task<string> Getserial()
        {
            string value = "vol C:";
            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            process.StartInfo = processStartInfo;
            process.Start();
            process.StandardInput.WriteLine(value);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
            string input = await process.StandardOutput.ReadToEndAsync();
            string text = Regex.Match(input, "(Number is.*)").Groups[1].Value.Trim();
            text = text.Substring(9);
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Lấy ra nhân viên hiện tại đang dùng máy
        /// created by: ltluat 18.06.2023
        /// </summary>
        /// <returns></returns>
        public static async Task<WhatsappEmployee> GetCurrentEmployee()
        {
            WhatsappEmployee emp = new WhatsappEmployee();
            try
            {
                // Get the serial number
                string serialNumber = await Getserial();
                if (string.IsNullOrEmpty(serialNumber)) return emp;

                using (HttpClient client = new HttpClient())
                {

                    var resultNumber = await client.GetAsync($"http://example.com/api/Whatsapp/GetWhatsappEmployee?uId={serialNumber}");

                    if (resultNumber != null)
                    {
                        string contentEmp = await resultNumber.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(contentEmp))
                        {
                            WhatsappEmployee info = JsonConvert.DeserializeObject<WhatsappEmployee>(contentEmp);

                            return info;
                        }
                    }
                }
                // Display the serial number
                Console.WriteLine("Serial Number: " + serialNumber);
            }
            catch (Exception ex)
            {

            }
            return emp;
        }
        static string GetDriveSerialNumber()
        {
            Drive drive = Drive.GetDrive("C");
            return drive.SerialNumber.ToString();
        }
    }
    public class Drive
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetVolumeInformation(
            string rootPathName,
            StringBuilder volumeNameBuffer,
            int volumeNameSize,
            ref uint volumeSerialNumber,
            ref uint maximumComponentLength,
            ref uint fileSystemFlags,
            StringBuilder fileSystemNameBuffer,
            int nFileSystemNameSize);

        public string VolumeName { get; private set; }

        public string FileSystemName { get; private set; }

        public uint SerialNumber { get; private set; }

        public string DriveLetter { get; private set; }

        public static Drive GetDrive(string driveLetter)
        {
            const int VolumeNameSize = 255;
            const int FileSystemNameBufferSize = 255;
            StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize);
            uint volumeSerialNumber = 0;
            uint maximumComponentLength = 0;
            uint fileSystemFeatures = 0;
            StringBuilder fileSystemNameBuffer = new StringBuilder(FileSystemNameBufferSize);

            if (GetVolumeInformation(
                string.Format("{0}:\\", driveLetter),
                volumeNameBuffer,
                VolumeNameSize,
                ref volumeSerialNumber,
                ref maximumComponentLength,
                ref fileSystemFeatures,
                fileSystemNameBuffer,
                FileSystemNameBufferSize))
            {
                return new Drive
                {
                    DriveLetter = driveLetter,
                    FileSystemName = fileSystemNameBuffer.ToString(),
                    VolumeName = volumeNameBuffer.ToString(),
                    SerialNumber = volumeSerialNumber
                };
            }

            // Something failed, returns null
            return null;
        }
    }
}
