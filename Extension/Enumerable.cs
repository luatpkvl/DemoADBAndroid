using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Extension
{
    /// <summary>
    /// trạng thái của số điện thoại
    /// created by: ltluat 14.06.2023
    /// </summary>
    public enum WhatsappPhoneStatus
    {
        /// <summary>
        /// trạng thái mới
        /// </summary>
        New = 0,
        /// <summary>
        /// Trạng thái đã get code
        /// </summary>
        GetCode = 1,
        /// <summary>
        /// trạng thái hoàn thành
        /// </summary>
        Finished = 2
    }
}
