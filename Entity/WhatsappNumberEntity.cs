using System;
using System.Collections.Generic;
using System.Text;
using WhatsappAccount.Extension;

namespace WhatsappAccount
{
    public class WhatsappNumberEntity
    {
        public WhatsappNumberEntity()
        {

        }
        public WhatsappNumberEntity(string phoneNumber, int status, string createdBy)
        {
            PhoneNumber = phoneNumber;
            Status = status;          
            CreatedBy = createdBy;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// ngày cập nhật
        /// </summary>
        public DateTime UpdatedDate { get; set; }
        /// <summary>
        /// người tạo - để đánh dấu chia tiền
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
