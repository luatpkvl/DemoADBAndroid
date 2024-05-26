using FULiveAutoApp.Model;
using Newtonsoft.Json;
using ProtonFather.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using static Amazon.EC2.Util.VPCUtilities;

namespace FULiveAutoApp.Utility
{
    public class LDUtility
    {
        public static async Task<string> ExecuteLD(string cmd)
        {
            try
            {
                string pathLD = File.ReadAllText("Config/ld_path.txt");
                Process p = new Process();

                p.StartInfo = new ProcessStartInfo()
                {
                    FileName = pathLD,
                    Arguments = cmd,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    //WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };
                p.Start();
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.Flush();
                p.StandardInput.Close();
                return await p.StandardOutput.ReadToEndAsync();
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Mở app ldplayer
        /// </summary>
        /// <param name="ldName"></param>
        /// <returns></returns>
        public static async Task<string> Open(string ldName)
        {
            return await LDUtility.ExecuteLD($"launch --name {ldName}");
        }
        /// <summary>
        /// Đóng app ldplayer
        /// </summary>
        /// <param name="ldName"></param>
        /// <returns></returns>
        public static async Task<string> Close(string ldName)
        {
            return await LDUtility.ExecuteLD($"quit --name {ldName}");
        }
        public static async Task<bool> CheckLDStartDone(string param, string NameOrIndex)
        {
            string str = string.Format("adb --{0} {1} --command \"{2}\"", param, NameOrIndex, "shell input tap 0 0");
            string pathLD = File.ReadAllText("Config/ld_path.txt");
            int num = 0;
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = pathLD,
                Arguments = str,
                CreateNoWindow = true,
                UseShellExecute = false,
                //WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            do
            {
                process.Start();
                process.StandardInput.Flush();
                process.StandardInput.Close();
                string res = await process.StandardOutput.ReadToEndAsync();
                if (!string.IsNullOrEmpty(res))
                {
                    Thread.Sleep(1000);
                    ++num;
                }
                else
                    goto label_1;
            }
            while (num != 40);
            goto label_3;
        label_1:
            return true;
        label_3:
            return false;
        }
        /// <summary>
        /// Thay đổi thông tin của thiết bị
        /// </summary>
        /// <param name="ldName"></param>
        /// <returns></returns>
        public static async Task ChangeInfo(string ldName)
        {
            try
            {
                string deviceInfoStr = File.ReadAllText("Config/DeviceInfo.json");
                List<LDDeviceInfo> lstInfo = JsonConvert.DeserializeObject<List<LDDeviceInfo>>(deviceInfoStr);
                string manufactured = "Huawei";
                string modelName = "Huawei Ascend G510";
                if (lstInfo != null && lstInfo.Any())
                {
                    LDDeviceInfo info = lstInfo[new Random().Next(lstInfo.Count)];
                    manufactured = info.manufacturer;
                    modelName = info.models[new Random().Next(info.models.Count)];
                }
                string imei = "86516602" + new TextUtility().RandomNumber(7);
                string imsi = "46000" + new TextUtility().RandomNumber(10);
                string simSerial = "898600" + new TextUtility().RandomNumber(14);
                await ExecuteLD($"modify --name {ldName} --manufacturer \"{manufactured}\"");
                await ExecuteLD($"modify --name {ldName} --model  \"{modelName}\"");
                await ExecuteLD($"modify --name {ldName} --imei {imei} --imsi {imsi} --simserial {simSerial}");
            }
            catch (Exception ex)
            {
                LogUtility.Error(ex.ToString());
            }
        }
    }
}
