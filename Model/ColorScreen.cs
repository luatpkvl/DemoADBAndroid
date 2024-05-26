using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsappAccount.Model
{
    public class ColorScreen
    {
        /// <summary>
        /// tọa độ X1
        /// </summary>
        public int PosX1 { get; set; }
        /// <summary>
        /// Tọa độ Y1
        /// </summary>
        public int PosY1 { get; set; }
        /// <summary>
        /// Mã màu compare 1
        /// </summary>
        public string Color1 { get; set; }

        /// <summary>
        /// Mã màu compare 2
        /// </summary>
        public string Color2 { get; set; }
        /// <summary>
        /// Vị trí X2
        /// </summary>
        public int PosX2 { get; set; }
        /// <summary>
        /// Vị trí Y2
        /// </summary>
        public int PosY2 { get; set; }
    }
}
