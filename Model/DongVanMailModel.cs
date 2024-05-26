using System;
using System.Collections.Generic;
using System.Text;

namespace ProtonFather.Model
{
    public class DongVanMailModel
    {
        public string error_code { get; set; }
        public bool status { get; set; }
        public DongVanMailAccount data { get; set; }
    }

    public class DongVanMailAccount
    {
        public string order_code { get; set; }
        public int balance { get; set; }
        public List<string> list_data { get; set; }
    }
}
