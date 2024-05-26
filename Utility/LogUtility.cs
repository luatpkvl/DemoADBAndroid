using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FULiveAutoApp.Utility
{
    public class LogUtility
    {
        public static void Error(string message)
        {
            try
            {
                File.AppendAllText("log.txt", message+Environment.NewLine);
            }
            catch
            {

            }
        }
    }
}
