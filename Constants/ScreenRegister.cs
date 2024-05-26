using System;
using System.Collections.Generic;
using System.Text;
using WhatsappAccount.Model;

namespace WhatsappAccount.Constants
{
    public class ScreenRegister
    {
        /// <summary>
        /// Màn hình đầu tiên
        /// </summary>
        /// <returns></returns>
        public static ColorScreen FirstScreen()
        {
            return new ColorScreen()
            {
                PosX1 = 160,
                PosY1 = 515,
                Color1 = ColorScreenConstants.Color160515,
                PosX2 = 391,
                PosY2 = 518,
                Color2 = ColorScreenConstants.Color391518,
            };
        }             
    }
}
