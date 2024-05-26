using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount
{
    public enum PhoneState
    {
        None,
        /// <summary>
        /// Đang lấy số
        /// </summary>
        GettingPhone,
        /// <summary>
        /// Đang điền số
        /// </summary>
        FillingPhone,
        /// <summary>
        /// Đang lấy code
        /// </summary>
        GettingCode,
        /// <summary>
        /// Đang điền code
        /// </summary>
        FillingCode,
        /// <summary>
        /// Điền các thông tin cần thiết
        /// </summary>
        FillingInfo,
        /// <summary>
        /// Kiểm tra thành công
        /// </summary>
        CheckingSuccess,
        //Thành công
        Success,

    }
}
