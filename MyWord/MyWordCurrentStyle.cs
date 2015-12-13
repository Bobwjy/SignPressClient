using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Office.MyWord.MyWord
{
    public class MyWordCurrentStyle
    {
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 字体是否加粗
        /// </summary>
        public int FontBlod { get; set; }

        public MyWordCurrentStyle()
        {
            FontSize = 10;
            FontBlod = 0;
        }
    }
}
