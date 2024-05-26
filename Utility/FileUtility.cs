using ProtonFather.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FULiveAutoApp.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// Lấy ra danh sách tên video
        /// </summary>
        /// <returns></returns>
        public static List<string> GetListVideoName()
        {
            List<string> lstFile = new List<string>();
            string[] mp4Files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "/Video", "*.mp4");
            if (mp4Files != null && mp4Files.Any())
            {
                foreach (string file in mp4Files)
                {
                    lstFile.Add(Path.GetFileName(file));
                }
            }
            //Tiến hành sort lại file theo tên
            if (lstFile != null && lstFile.Any())
            {
                lstFile.Sort();
            }
            return lstFile;
        }

        /// <summary>
        /// Đổi tên file name
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void ChangeFileName(string directoryPath)
        {
            string[] mp4Files = Directory.GetFiles(directoryPath, "*.mp4");

            foreach (string filePath in mp4Files)
            {
                try
                {
                    // Get the file name without the extension
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                    string newFileName = new TextUtility().GenerateName(3).ToUpper(); ;

                    // Get the directory path of the file
                    string fileDirectory = Path.GetDirectoryName(filePath);

                    // Combine the directory path and the new file name with the .mp4 extension
                    string newFilePath = Path.Combine(fileDirectory, newFileName + ".mp4");

                    // Rename the file
                    File.Move(filePath, newFilePath);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
