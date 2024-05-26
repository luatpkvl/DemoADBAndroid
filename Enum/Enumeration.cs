using System;
using System.Collections.Generic;
using System.Text;

namespace FULiveAutoApp.Enum
{
    public class Enumeration
    {
    }
    /// <summary>
    /// Trạng thái của thiết bị
    /// </summary>
    public enum EnumDeviceStatus
    {
        /// <summary>
        /// Trạng thái chưa khởi chạy
        /// </summary>
        NotRunning = 0,
        /// <summary>
        /// Trạng thái đang cho chạy auto
        /// </summary>
        Running = 1,
        /// <summary>
        /// Trạng thái đang upload
        /// </summary>
        Uploading = 2
    }
}
