using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Entity
{
    public class WhatsappEmployee
    {
        /// <summary>
        /// id của nhân viên/mã thiết bị
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// Key của thuê
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// trạng thái hoạt động hay không
        /// </summary>
        public bool inActive { get; set; }
        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime createdDate { get; set; }
    }
}
