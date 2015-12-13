using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Office.MyWord.MyWordTable
{
    /// <summary>
    /// 表格中的单元格
    /// </summary>
    public class WordTableCell
    {

        public WordTableCell()
        {
            Row = 1;
            Column = 1;
        }
        public WordTableCell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; }
    }
}
