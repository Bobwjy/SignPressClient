using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Office.MyWord.MyWord
{
    public class MyWordPageStyle
    {


        /// <summary>
        /// 默认设置word风格
        /// </summary>
        public MyWordPageStyle()
        {
            TopMargin = 3;
            BottomMargin = 1;
            LeftMargin = 1.67F;
            RightMargin = 1.67F;
            LeftIndent = 0;
            RightIndent = 0; ;
        }

        /// <summary>
        /// 上边距
        /// </summary>
        public float TopMargin { get; set; }


        /// <summary>
        /// 下边距
        /// </summary>
        public float BottomMargin { get; set; }


        /// <summary>
        /// 左边距
        /// </summary>
        public float LeftMargin { get; set; }

        /// <summary>
        /// 右边距
        /// </summary>
        public float RightMargin { get; set; }

        /// <summary>
        /// 左缩进
        /// </summary>
        public float LeftIndent { get; set; }
        /// <summary>
        /// 右缩进
        /// </summary>
        public float RightIndent { get; set; }


    }
}
