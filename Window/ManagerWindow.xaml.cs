using Amazon.CloudTrail.Model;
using Amazon.DeviceFarm.Model;
using Amazon.SimpleDB.Model;
using Emgu.CV.Ocl;
using FULiveAutoApp.Model;
using FULiveAutoApp.Utility;
using KAutoHelper;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using ProtonFather.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WhatsappAccount.Constants;
using WhatsappAccount.Entity;
using WhatsappAccount.Extension;
using WhatsappAccount.Model;
using WhatsappAccount.Utility;
using Image = SixLabors.ImageSharp.Image;

namespace WhatsappAccount
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </sumary>
    public partial class ManagerWindow : Window
    {
        private string _otpKey;
        private string _typePhone;
        private int _currentBalance = 0;
        private string _speechKey;
        private ObservableCollection<AndroidDevice> _phoneInfo = new ObservableCollection<AndroidDevice>();
        private List<string> _lstWaitingRemove = new List<string>();
        private DispatcherTimer timer;
        private DispatcherTimer timerRemove;
        private string _userName;
        private WhatsappEmployee _emp;
        private string _captCha69Key;
        static object lockObject = new object();
        string PathProfile = "Profile";
        List<string> listDevices = new List<string>();

        private DispatcherTimer timerCheckUser;
        public ManagerWindow()
        {
            InitializeComponent();
            _ = InitData();
        }
        private async Task InitData()
        {
            await LoadAllAndroidDevice();
        }


        private async Task LoadAllAndroidDevice()
        {
            _phoneInfo = new ObservableCollection<AndroidDevice>();
            listDevices = await ADBUtility.GetDevices();
            //listDevices = new List<string>();
            //listDevices.Add("192.168.0.111:5555");
            //xDeviceID.Text = "192.168.0.111:5555";
            string textDeviceExcept = File.ReadAllText("Config/ExceptDeviceID.txt");
            if (listDevices != null && listDevices.Any())
            {
                List<string> filterDevice = new List<string>();
                foreach (var device in listDevices ?? new List<string>())
                {
                    if (textDeviceExcept.Contains(device))
                    {
                        continue;
                    }
                    filterDevice.Add(device);
                    AndroidDevice info = new AndroidDevice();
                    info.DeviceID = device;
                    _phoneInfo.Add(info);
                }
                lbQTY.Content = filterDevice.Count.ToString();
                listDevices = filterDevice.OrderBy(x => x).ToList();

                itemPhone.ItemsSource = _phoneInfo;
            }
        }
        public IWebDriver Driver { get; set; }
        public Process TorProcess { get; set; }
        public WebDriverWait Wait { get; set; }
        private void DeleteFolderName(string PathDetailProfile)
        {
            try
            {
                // Check if the folder exists before attempting to delete
                if (Directory.Exists(PathDetailProfile))
                {
                    Directory.Delete(PathDetailProfile, true); // The second parameter is whether to delete subdirectories and files recursively
                                                               //MessageBox.Show("Folder deleted successfully.");
                }
                else
                {
                    //MessageBox.Show("Folder does not exist.");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void TeardownTest()
        {
            this.Driver.Quit();
            this.TorProcess.Kill();
        }
        public void Open_Tor_Browser()
        {
            this.Driver.Navigate().GoToUrl(@"http://whatismyipaddress.com/");
            var expression = By.XPath("//*[@id='section_left']/div[2]");
            this.Wait.Until(x => x.FindElement(expression));
            var element = this.Driver.FindElement(expression);
            // Assert.AreNotEqual<string>("84.40.65.000", element.Text);
        }
        private void btnStartRun_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Lấy ra random chữ
        /// </summary>
        /// <param name="words"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static string PickRandomWords(List<string> words, int count)
        {
            Random random = new Random();
            List<string> pickedWords = new List<string>();

            // Shuffle the list to ensure randomness
            for (int i = words.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                string temp = words[i];
                words[i] = words[j];
                words[j] = temp;
            }
            // Pick the specified number of random words
            for (int i = 0; i < Math.Min(count, words.Count); i++)
            {
                pickedWords.Add(words[i]);
            }
            return string.Join(" ", pickedWords);
        }
        /// <summary>
        /// Lấy mã màu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetColor_Click(object sender, RoutedEventArgs e)
        {
            tbColorCodeResult.Text = string.Empty;
            if (string.IsNullOrEmpty(xDeviceID.Text))
            {
                return;
            }
            string deviceID = xDeviceID.Text.Trim();
            Task.Run(async () =>
            {
                try
                {
                    _ = this.Dispatcher.Invoke(async () =>
                    {
                        using (var imager = await ADBUtility.ScreenShoot(deviceID))
                        {
                            if (imager != null)
                            {
                                int compareX = int.Parse(xLocation.Text);
                                int compareY = int.Parse(yLocation.Text);
                                //compare 1
                                Rgba32 pixelColorCompare1 = imager[compareX, compareY];
                                string colorCodeCompare1 = pixelColorCompare1.ColorCode();
                                tbColorCodeResult.Text = colorCodeCompare1;
                            }
                            else
                            {
                                tbColorCodeResult.Text = "null";
                            }

                        }
                    });
                }
                catch (Exception e)
                {

                }
            });
        }

        private void btnScreen_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
           {
               _ = this.Dispatcher.Invoke(async () =>
               {
                   if (!string.IsNullOrEmpty(xDeviceID.Text))
                   {
                       await ADBUtility.ScreenShoot(xDeviceID.Text, false, "test.png");
                   }
               });
           });
        }

        private void btnSendVideo_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Click vào device ID để coppy text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDeviceID_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }
        private void SetLoadingStatus(bool isLoading)
        {
        }
        /// <summary>
        /// Cho xoay video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRotateVideo_Click(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// Sự kiện click để ẩn hiện nút
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbHello_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }
        /// <summary>
        /// Buff cuộc gọi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBuffCall_Click(object sender, RoutedEventArgs e)
        {
        first_run:
            int retryTime = 0;
            string item = "thamkhaothoinha";
        isCheckAccount:
            bool isCheckAccount = await ImageUtility.CompareScreen(item, ScreenRegister.FirstScreen());
            if (!isCheckAccount)
            {
                retryTime++;
                if (retryTime > 20)
                {
                    goto first_run;
                }
                await Task.Delay(TimeSpan.FromSeconds(1));
                goto isCheckAccount;
            }
        }
    }
}
