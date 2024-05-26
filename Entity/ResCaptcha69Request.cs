using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Entity
{
    public class ResCaptcha69Request
    {
        /// <summary>
        /// Key của tài khoản người dùng
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// phương thức sử dụng
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// id của gói tin
        /// </summary>
        public string id { get; set; }
    }
}
