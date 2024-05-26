using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappAccount.Utility
{
    public class XmlUtility
    {
        /// <summary>
        /// đọc từ file XML
        /// created by: ltluat 05.06.2023
        /// </summary>
        /// <returns></returns>
        public static async Task<string> ReadXmlFromDevice(string deviceID)
        {
            string xmlContent = string.Empty;
            string contentString = await ADBUtility.ExecuteCMD($"adb -s {deviceID} shell cat /data/data/com.whatsapp.w4b/shared_prefs/com.whatsapp.registration.RegisterPhone.xml ");
            if (!string.IsNullOrEmpty(contentString))
            {
                return contentString;
            }
            return xmlContent;
        }
    }
}
