using FULiveAutoApp.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FULiveAutoApp.Model
{
    public class AndroidDevice : INotifyPropertyChanged
    {
        public string DeviceID { get; set; }
        public EnumDeviceStatus Status { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
