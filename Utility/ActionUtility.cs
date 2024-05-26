using KAutoHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhatsappAccount.Constants;

namespace WhatsappAccount.Utility
{
    public class ActionUtility
    {
        /// <summary>
        /// Sự kiện restart clear dữ liệu
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task<bool> RestartFirstTime(string deviceID)
        {           
            return true;
        }
        /// <summary>
        /// Điền số điện thoại vào
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task<bool> FillPhoneNumber(string deviceID,string phoneNumber)
        {                       
            return true;
        }
        /// <summary>
        /// Tieeps tuc danh ba va tep
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static async Task<bool> ContinueFile(string deviceID)
        {          
            return await FillName(deviceID);      
        }
        /// <summary>
        /// Điền tên doanh nghiệp
        /// created by: ltluat 07.06.2023
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task<bool> FillName(string deviceID)
        {           
            return await SemiFinal(deviceID);
        }
        /// <summary>
        /// Check gần cuối
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task<bool> SemiFinal(string deviceID)
        {         
           return await CheckSuccess(deviceID);       
        }

        /// <summary>
        /// Check gần cuối
        /// </summary>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static async Task<bool> CheckSuccess(string deviceID)
        {          
            return true;
        }
    }
}
