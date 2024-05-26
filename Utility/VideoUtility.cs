using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FULiveAutoApp.Utility
{
    public class VideoUtility
    {
        /// <summary>
        /// Tiến hành xoay video
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <returns></returns>
        public static async Task RotateVideoAsync(string inputFilePath, string outputFilePath, int currentFileIndex, int totalFiles)
        {
            await Task.Run(() =>
            {
                try
                {
                    string ffmpegArgs = $"-i {inputFilePath} -vf \"transpose=3\" {outputFilePath}";
                    ExecuteFFmpegCommandSilently(ffmpegArgs, currentFileIndex, totalFiles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing {inputFilePath}: {ex.Message}");
                }
            });
        }
        /// <summary>
        /// Excute thong tin từ ffmpeg
        /// </summary>
        /// <param name="args"></param>
        public static void ExecuteFFmpegCommand(string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                Console.WriteLine("Output: " + output);
                Console.WriteLine("Error: " + error);
            }
        }
        /// <summary>
        /// Thực thi lệnh
        /// </summary>
        /// <param name="args"></param>
        public static void ExecuteFFmpegCommandSilently(string args, int currentFileIndex, int totalFiles)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true; // Hide the command window

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        Console.WriteLine($"Processing file {currentFileIndex}/{totalFiles}: {e.Data}");
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        Console.WriteLine($"Error processing file {currentFileIndex}/{totalFiles}: {e.Data}");
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
