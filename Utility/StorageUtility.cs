using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhatsappAccount.Extension;
using WhatsappAccount.Model;

namespace WhatsappAccount.Utility
{
    public class StorageUtility
    {
        /// <summary>
        /// Upload folder lên server
        /// created by: ltluat 14.06.2023
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="uploadUrl"></param>
        /// <returns></returns>
        public static async Task UploadFile(string filePath, string uploadUrl)
        {          
        }
        /// <summary>
        /// Lưu tài khoản whatsapp
        /// created by: ltluat 14.06.2023
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public async static Task SaveWhatsappAccount(string deviceID,string userName)
        {
            //:v clear all
        }
    }
}
