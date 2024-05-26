using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WhatsappAccount.Model
{
    public class PhoneInfo : INotifyPropertyChanged
    {              
        public string device_id { get; set; }
        public string code { get; set; }
        public long created_at { get; set; }
        public long done_at { get; set; }
        public string formated_phone { get; set; }
        /// <summary>
        /// id của số phục vụ cho query
        /// </summary>
        public string id { get; set; }
        public string full_text { get; set; }
        /// <summary>
        /// lấy ra order id
        /// </summary>
        public string order_id { get; set; }
        public string phone { get; set; }
        public DateTime created_date { get; set; }
        public string created_date_str { get { return created_date.ToString("dd/MM/yyyy HH:mm:ss"); } }
        /// <summary>
        /// Trạng thái của điện thoại
        /// </summary>
        public string state { get; set; } = "Chờ lấy số";

        public string background_color { get; set; } = "#cd451d";
        /// <summary>
        /// Trangj thái có phải lấy số mới hay không?
        /// </summary>
        public bool IsGetNewNumber { get; set; }
        /// <summary>
        /// trang thai
        /// </summary>
        /// 
        public string status { get; set; }

        /// <summary>
        /// Trạng thái đã được fill code hay là chưa
        /// </summary>
        public bool isFilledCode { get; set; } = false;
        /// <summary>
        /// Trạng thái code hiện tại
        /// </summary>
        public PhoneState CurrentState { get; set; } = PhoneState.None;

        /// <summary>
        /// trạng thái hiển thị
        /// </summary>
        public string phone_state
        {
            get
            {
                string typeString = "";
                switch (CurrentState)
                {
                    case PhoneState.None:
                        typeString = "Đang chờ";
                        break;
                    case PhoneState.GettingPhone:
                        typeString = "Đang lấy số";
                        break;
                    case PhoneState.FillingPhone:
                        typeString = "Đang điền số điện thoại";
                        break;
                    case PhoneState.GettingCode:
                        typeString = "Đang kiểm tra code";
                        break;
                    case PhoneState.FillingCode:
                        typeString = "Đang điền code";
                        break;
                    case PhoneState.FillingInfo:
                        typeString = "Đang điền thông tin";
                        break;
                    case PhoneState.CheckingSuccess:
                        typeString = "Kiểm tra thành công không";
                        break;
                    case PhoneState.Success:
                        typeString = "Tạo thành công";
                        break;
                    default:
                        typeString = "Đang chờ";
                        break;
                }
                return typeString;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
