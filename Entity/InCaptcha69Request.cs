using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Entity
{
    public class InCaptcha69Request
    {
        /// <summary>
        /// key của tải khoản
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// phương thức
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// Thân dạng base64 của ảnh gửi lên
        /// </summary>
        public string body { get; set; }
    }
}
