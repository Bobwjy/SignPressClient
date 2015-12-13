using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.MyWord.MyWord;

namespace Microsoft.Office.MyWord.MyWordTable
{
    public class WordTable 
    {

        protected Microsoft.Office.Interop.Word.Table _Table;

        public delegate void DesginTableHandler(ref Microsoft.Office.Interop.Word.Table table, List<int> columnWeight);
        public event DesginTableHandler DesginTableEvent;

        public WordTable() { }

        public Microsoft.Office.Interop.Word.Table TableOne
        {
            get { return _Table; }
            set { _Table = value; }
        }

        public void ConstructTable(List<int> columnWeight)
        {
            _SetTableStyle(ref _Table);
            DesginTableEvent(ref this._Table, columnWeight);
        }

   
        protected void _SetTableStyle(ref Microsoft.Office.Interop.Word.Table table)
        {
            object style = "网格型";
            table.set_Style(ref style);

            //11/04/14加入
            //外边框颜色
            table.Rows.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleInset;
            table.Borders.OutsideColor = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
            table.Borders.OutsideLineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth150pt;
            //编写表格格式
            table.LeftPadding = 0;
            table.RightPadding = 0;
            table.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        }
        /// <summary>
        /// 填充表格数据
        /// </summary>
        /// <param name="cellContent"></param>
        public virtual void FillingTableContent(Dictionary<WordTableCell, string> cellContent)
        {
            UpdateTaleContent(cellContent);
        }

        /// <summary>
        /// 向Table中写入数据
        /// </summary>
        /// <param name="cellContent"></param>
        protected void UpdateTaleContent(Dictionary<WordTableCell, string> cellContent)
        {
            foreach (var cell in cellContent.Keys)
                if (!string.IsNullOrEmpty(cellContent[cell]) && (!string.Equals(_Table.Cell(cell.Row, cell.Column).Range.Text, cellContent[cell])))
                    _Table.Cell(cell.Row, cell.Column).Range.Text = cellContent[cell];
        }

        private void WriteTableCell(int row, int column, string content)
        {
            _Table.Cell(row, column).Range.Text = content;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="merge">合并单元格</param>
        /// <param name="merged">被合并的单元格</param>
        public void MergeCell(WordTableCell merge, WordTableCell merged)
        {
            _Table.Cell(merge.Row, merge.Column).Merge(_Table.Cell(merged.Row, merged.Column));
        }

        /// <summary>
        /// 合并标题上的单元格
        /// </summary>
        /// <param name="merge">合并单元格</param>
        /// <param name="merged">被合并的单元格</param>
        public void MergeTitleCell(WordTableCell merge, WordTableCell merged)
        {
            object os = 2;
            object oc = 1;

            _Table.Cell(merge.Row, merge.Column).Split(ref os, ref oc);
            _Table.Cell(merged.Row, merged.Column).Split(ref os, ref oc);
            _Table.Cell(merge.Row, merge.Column).Merge(_Table.Cell(merged.Row, merged.Column));
        }

        /// <summary>
        /// 在row后面添加一行
        /// </summary>
        /// <param name="row">添加行后的前一行</param>
        public void AddTableRow(int row)
        {
            object missing = System.Reflection.Missing.Value;
            Row newRow = _Table.Cell(row, 1).Range.Rows.Add(ref missing);
        }

        /// <summary>
        /// 删除一列
        /// </summary>
        /// <param name="row">列数</param>
        public void DeleteTableRow(int row)
        {
            _Table.Cell(row, 1).Range.Rows.Delete();
        }

        /// <summary>
        /// 添加一列
        /// </summary>
        /// <param name="column">添加列的前一列</param>
        public void AddTableColumn(int column)
        {
            object missing = System.Reflection.Missing.Value;
            Column newRow = _Table.Cell(1, column).Range.Columns.Add(missing);
        }
        /// <summary>
        /// 在表格的最后一列后面添加一列
        /// </summary>
        public void AddColumnEndTable()
        {
            AddTableColumn(_Table.Columns.Count);
        }
        /// <summary>
        /// 在表格的最后添加一行
        /// </summary>
        public void AddRowEndTable()
        {
            AddTableRow(_Table.Rows.Count);
        }

        /// <summary>
        /// 当前表格的总行数
        /// </summary>
        public int CurrentTableRow
        {
            get { return _Table.Rows.Count; }
        }


        /// <summary>
        /// 当前表格的总列数
        /// </summary>
        public int CurrentTableColumn
        {
            get { return _Table.Columns.Count; }
        }
    }
}
