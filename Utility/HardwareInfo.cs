using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WhatsappAccount.Utility
{
    public static class HardwareInfo
    {
        // Import the necessary native methods
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetVolumeInformation(
            string rootPathName,
            IntPtr volumeNameBuffer,
            int volumeNameSize,
            out uint volumeSerialNumber,
            out uint maximumComponentLength,
            out uint fileSystemFlags,
            IntPtr fileSystemNameBuffer,
            int nFileSystemNameSize);

        public static string GetMotherboardSerialNumber()
        {
            const string rootPath = "C:\\"; // Can be any valid root path

            uint serialNumber;
            uint _, __, ___; // Unused parameters

            if (GetVolumeInformation(rootPath, IntPtr.Zero, 0, out serialNumber, out _, out __, IntPtr.Zero, 0))
            {
                return serialNumber.ToString("X"); // Convert to hexadecimal format
            }

            return null;
        }
    }
}
