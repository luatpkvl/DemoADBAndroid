using FULiveAutoApp.Model;
using Newtonsoft.Json;
using ProtonFather.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WhatsappAccount.Model;
using WhatsappAccount.Utility;

namespace FULiveAutoApp.Utility
{
    public class ChangeInfoUtility
    {
        public static async Task<DeviceInfo> NewDevice(string deviceID, string fileName)
        {
            //load fileJson
            string deviceStr = File.ReadAllText("Config/ListDevice.json");
            DeviceGenerate deviceGenerate = new DeviceGenerate();
            if (!string.IsNullOrEmpty(deviceStr))
            {
                var lstDevice = JsonConvert.DeserializeObject<List<DeviceGenerate>>(deviceStr);
                if (lstDevice != null && deviceStr.Any())
                {
                    deviceGenerate = lstDevice[new Random().Next(lstDevice.Count)];
                }
                else
                {
                    deviceGenerate.brand = "HTC";
                    deviceGenerate.name = "One remix";
                    deviceGenerate.device = "htc_memwl";
                    deviceGenerate.model = "HTC6515LVW";
                }
            }
            DeviceInfo info = new DeviceInfo();
            info.SimState = "5";
            info.googlead_id = Guid.NewGuid().ToString();
            info.IP = string.Empty;
            info.CODENAME = string.Empty;
            info.WifiBssid = NewMacAdress();
            info.TYPE = string.Empty;
            info.USBDebugMode = "0";
            info.Manufacturer = deviceGenerate.brand;
            info.Wifissid = "TP-LINK_GKzoAa";
            info.TIME = string.Empty;
            info.IMEI = new TextUtility().RandomNumber(15);//15
            info.Product = deviceGenerate.model;
            info.Incremental = string.Empty;
            info.SerialNo = new TextUtility().RandomSerial(13);//13
            info.NetType = "WIFI";
            info.CPU_ABI = "armeabi-v7a";
            info.SDK = "25";
            info.NetworkOperatorName = "19";
            info.BluetoothMac = NewMacAdress();
            info.Host = $"{deviceGenerate.model}-{new TextUtility().RandomText(4)}";
            info.PhoneNumber = $"+86{new TextUtility().RandomNumber(11)}";
            info.Release = "12.0.0";
            info.BuildID = "KTU84Q";
            info.TAGS = string.Empty;
            info.CPU_ABI2 = "armeabi";
            info.Brand = deviceGenerate.brand;
            info.BootLoader = string.Empty;
            info.Model = deviceGenerate.model;
            info.USER = "compiler";
            info.Resolution = string.Empty;
            info.Hardware = "MT6592M";
            info.WifiMac = NewMacAdress();
            info.Board = deviceGenerate.model;
            info.Display = $"{deviceGenerate.model}.{new TextUtility().RandomNumber(6)}.85HX1";
            info.FingerPrint = string.Empty;
            info.SimOperator = "46000";
            info.RadioVersion = string.Empty;
            info.Device = deviceGenerate.device;
            info.Width = string.Empty;
            info.SimCountryIso = "us";
            info.AndroidId = new TextUtility().RandomSerial(16);
            info.SimSerialNo = new TextUtility().RandomSerial(10);
            info.Height = string.Empty;
            info.SimOperatorName = "CMCC";
            info.SubscriberId = new TextUtility().RandomNumber(15);
            // Load the XML document
            string xmlFileContent = File.ReadAllText("Config/TemplateDevice.xml");
            GetDeviceInfoXml(ref xmlFileContent, info);
            string fileNamePath = AppDomain.CurrentDomain.BaseDirectory + "Export/" + fileName;
            if (!string.IsNullOrEmpty(xmlFileContent))
            {
                //Tiến hành ghi ra file để đẩy vào device
                File.WriteAllText(fileNamePath, xmlFileContent);
                //Tiến hành replace trong 
                string localXmlPath = fileNamePath;        
                string deviceTargetPath = "/data/data/com.bigsing.changer/shared_prefs/phoneinfo.xml";
                string textRun = await ADBUtility.ExecuteCMD($"adb -s {deviceID} push \"{localXmlPath}\" \"{deviceTargetPath}\"");             
                //Delete file
                File.Delete(fileNamePath);
            }
            return info;
        }
        public static string GetDeviceInfoXml(ref string xmlDoc, DeviceInfo info)
        {
            SetValueXMLNode(ref xmlDoc, "SimState", info.SimState);
            SetValueXMLNode(ref xmlDoc, "googlead_id", info.googlead_id);
            SetValueXMLNode(ref xmlDoc, "IP", info.IP);
            SetValueXMLNode(ref xmlDoc, "CODENAME", info.CODENAME);
            SetValueXMLNode(ref xmlDoc, "WifiBssid", info.WifiBssid);
            SetValueXMLNode(ref xmlDoc, "TYPE", info.TYPE);
            SetValueXMLNode(ref xmlDoc, "USBDebugMode", info.USBDebugMode);
            SetValueXMLNode(ref xmlDoc, "Manufacturer", info.Manufacturer);
            SetValueXMLNode(ref xmlDoc, "Wifissid", info.Wifissid);
            SetValueXMLNode(ref xmlDoc, "TIME", info.TIME);
            SetValueXMLNode(ref xmlDoc, "IMEI", info.IMEI);
            SetValueXMLNode(ref xmlDoc, "Product", info.Product);
            SetValueXMLNode(ref xmlDoc, "Incremental", info.Incremental);
            SetValueXMLNode(ref xmlDoc, "SerialNo", info.SerialNo);
            SetValueXMLNode(ref xmlDoc, "NetType", info.NetType);
            SetValueXMLNode(ref xmlDoc, "CPU_ABI", info.CPU_ABI);
            SetValueXMLNode(ref xmlDoc, "SDK", info.SDK);
            SetValueXMLNode(ref xmlDoc, "NetworkOperatorName", info.NetworkOperatorName);
            SetValueXMLNode(ref xmlDoc, "BluetoothMac", info.BluetoothMac);
            SetValueXMLNode(ref xmlDoc, "Host", info.Host);
            SetValueXMLNode(ref xmlDoc, "PhoneNumber", info.PhoneNumber);
            SetValueXMLNode(ref xmlDoc, "Release", info.Release);
            SetValueXMLNode(ref xmlDoc, "BuildID", info.BuildID);
            SetValueXMLNode(ref xmlDoc, "TAGS", info.TAGS);
            SetValueXMLNode(ref xmlDoc, "CPU_ABI2", info.CPU_ABI2);
            SetValueXMLNode(ref xmlDoc, "Brand", info.Brand);
            SetValueXMLNode(ref xmlDoc, "BootLoader", info.BootLoader);
            SetValueXMLNode(ref xmlDoc, "Model", info.Model);
            SetValueXMLNode(ref xmlDoc, "USER", info.USER);
            SetValueXMLNode(ref xmlDoc, "Resolution", info.Resolution);
            SetValueXMLNode(ref xmlDoc, "Hardware", info.Hardware);
            SetValueXMLNode(ref xmlDoc, "WifiMac", info.WifiMac);
            SetValueXMLNode(ref xmlDoc, "Board", info.Board);
            SetValueXMLNode(ref xmlDoc, "Display", info.Display);
            SetValueXMLNode(ref xmlDoc, "FingerPrint", info.FingerPrint);
            SetValueXMLNode(ref xmlDoc, "SimOperator", info.SimOperator);
            SetValueXMLNode(ref xmlDoc, "RadioVersion", info.RadioVersion);
            SetValueXMLNode(ref xmlDoc, "Device", info.Device);
            SetValueXMLNode(ref xmlDoc, "Width", info.Width);
            SetValueXMLNode(ref xmlDoc, "SimCountryIso", info.SimCountryIso);
            SetValueXMLNode(ref xmlDoc, "AndroidId", info.AndroidId);
            SetValueXMLNode(ref xmlDoc, "SimSerialNo", info.SimSerialNo);
            SetValueXMLNode(ref xmlDoc, "Height", info.Height);
            SetValueXMLNode(ref xmlDoc, "SimOperatorName", info.SimOperatorName);
            SetValueXMLNode(ref xmlDoc, "SubscriberId", info.SubscriberId);

            return xmlDoc;
        }
        /// <summary>
        /// Update lai gia tri xml node
        /// </summary>
        /// <param name="nodeName"></param>
        public static void SetValueXMLNode(ref string xmlDoc, string nodeName, string value)
        {
            xmlDoc = xmlDoc.Replace($">{nodeName}<", $">{value}<");
        }
        /// <summary>
        /// Tao moi dia chi mac
        /// </summary>
        /// <returns></returns>
        public static string NewMacAdress()
        {
            Random random = new Random();
            byte[] macAddr = new byte[6];
            random.NextBytes(macAddr);
            // Set the local bit for MAC address (bit 1 of first byte)
            macAddr[0] = (byte)(macAddr[0] | 0x02);
            // Format MAC address as hexadecimal string
            string macAddress = string.Join(":", macAddr.Select(b => b.ToString("X2"))).ToLower();
            return macAddress;
        }
    }
}
