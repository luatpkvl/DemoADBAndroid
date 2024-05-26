using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace WhatsappAccount.Utility
{
    public class ZipUtility
    {
        /// <summary>
        /// Tiến hành zip file C#
        /// created by: ltluat 14.06.2023
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destinationZipPath"></param>
        public static void ZipFolder(string sourceFolderPath, string destinationZipPath)
        {
            ZipFile.CreateFromDirectory(sourceFolderPath, destinationZipPath);
        }
    }
}
