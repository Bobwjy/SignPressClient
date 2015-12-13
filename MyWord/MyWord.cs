using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace Microsoft.Office.MyWord.MyWord
{
    public class MyWord
    {
        protected Microsoft.Office.Interop.Word._Application _WordApplicMain;
        protected Microsoft.Office.Interop.Word._Document _MainDoc;
        protected Object Nothing = System.Reflection.Missing.Value;
        protected MyWordTable.WordTable _WordTable;
        protected string _SaveFilePath;

        public delegate void DesginTableHandler(ref Microsoft.Office.Interop.Word.Table table, List<int> columnWeight);
        public event DesginTableHandler DesginTable;
        /// <summary>
        /// Word文件
        /// </summary>
        public MyWord()
        {
            _SaveFilePath = @"C:\temp.doc";
        }

        /// <summary>
        /// word文件
        /// </summary>
        /// <param name="saveFilePath">word的保存路径</param>
        public MyWord(string saveFilePath)
        {
            this._SaveFilePath = saveFilePath;
        }

        /// <summary>
        /// 创建word文档
        /// </summary>
        public void CreateWord()
        {
            _WordApplicMain = new Microsoft.Office.Interop.Word.Application();
            _MainDoc = _WordApplicMain.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            SetPageDefaultStyle(new MyWordPageStyle());
        }

        /// <summary>
        /// 设置页格式
        /// </summary>
        /// <param name="myWordPageStyle">要设置的页格式</param>
        public void SetPageDefaultStyle(MyWordPageStyle myWordPageStyle)
        {
            _WordApplicMain.ActiveDocument.PageSetup.TopMargin = _WordApplicMain.CentimetersToPoints(myWordPageStyle.TopMargin);
            _WordApplicMain.ActiveDocument.PageSetup.BottomMargin = _WordApplicMain.CentimetersToPoints(myWordPageStyle.BottomMargin);
            _WordApplicMain.ActiveDocument.PageSetup.LeftMargin = _WordApplicMain.CentimetersToPoints(myWordPageStyle.LeftMargin);
            _WordApplicMain.ActiveDocument.PageSetup.RightMargin = _WordApplicMain.CentimetersToPoints(myWordPageStyle.RightMargin);
            _WordApplicMain.Selection.ParagraphFormat.LeftIndent = _WordApplicMain.CentimetersToPoints(myWordPageStyle.LeftIndent);
            _WordApplicMain.Selection.ParagraphFormat.RightIndent = _WordApplicMain.CentimetersToPoints(myWordPageStyle.RightIndent);
        }
        /// <summary>
        /// 设置当前页面的字体
        /// </summary>
        /// <param name="currentPageStyle">字体设置</param>
        public void SetCurrentPageStyle(MyWordCurrentStyle currentPageStyle)
        {
            _WordApplicMain.Selection.ParagraphFormat.LineSpacingRule = Microsoft.Office.Interop.Word.WdLineSpacing.wdLineSpaceSingle;
            _WordApplicMain.Selection.ParagraphFormat.FarEastLineBreakControl = 0;
            _WordApplicMain.Selection.ParagraphFormat.WordWrap = 0;
            _WordApplicMain.Selection.Font.Bold = currentPageStyle.FontBlod;
            _WordApplicMain.Selection.Font.Size = currentPageStyle.FontSize;
        }

        /// <summary>
        /// 向Word文档中写入内容
        /// </summary>
        /// <param name="content">写入的内容</param>
        public void WriteContent(string content)
        {
            _WordApplicMain.Selection.TypeText(content);
        }

        /// <summary>
        /// 换页
        /// </summary>
        public void Feed()
        {
            _WordApplicMain.Selection.WholeStory();
            object MoveUnit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            object MoveExtend = Type.Missing;
            _WordApplicMain.Selection.EndKey(ref MoveUnit, ref MoveExtend);
            Object pageObj = WdBreakType.wdPageBreak;
            _WordApplicMain.Selection.InsertBreak(ref pageObj);
        }

        /// <summary>
        /// 设置页眉
        /// </summary>
        /// <param name="pageHeaderContent">页眉内容</param>
        public void SetPageHeader(string pageHeaderContent)
        {
            _WordApplicMain.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
            string sFooter = pageHeaderContent;
            _SetHeaderOrFooter(sFooter);

        }
        /// <summary>
        /// 设置页脚
        /// </summary>
        /// <param name="pageFooterContent">设置页脚内容</param>
        public void SetPageFooter(string pageFooterContent)
        {
            _WordApplicMain.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryFooter;
            string sFooter = pageFooterContent;
            _SetHeaderOrFooter(sFooter);
        }
        private void _SetHeaderOrFooter(string sFooter)
        {
            //添加页眉
            if (_WordApplicMain.ActiveWindow.ActivePane.View.Type == WdViewType.wdNormalView || _WordApplicMain.ActiveWindow.ActivePane.View.Type == WdViewType.wdOutlineView)
            {
                _WordApplicMain.ActiveWindow.ActivePane.View.Type = WdViewType.wdPrintView;
            }
            object objATEntry_Page = "第 X 页 共 Y 页";
            _WordApplicMain.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;//设置居中齐
            object objTrue = true;
            Range rng = _WordApplicMain.Selection.HeaderFooter.Range;
            _WordApplicMain.NormalTemplate.AutoTextEntries.get_Item(ref objATEntry_Page).Insert(rng, ref objTrue);
            _WordApplicMain.Selection.HeaderFooter.Range.InsertBefore(sFooter);
            //退出页眉，返回正文
        }

        #region 表格处理

        /// <summary>
        /// 获得表格对象
        /// </summary>
        public MyWordTable.WordTable WordTable
        {
            get { return _WordTable; }
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="rowNumber">行数</param>
        /// <param name="columnNumber">列数</param>
        /// <param name="columnWeight">每一列的宽度</param>
        /// <returns>第几个表格</returns>
        public void CreateTable(int rowNumber, int columnNumber, List<int> columnWeight)
        {
            _WordTable = new MyWordTable.WordTable();
            _WordTable.TableOne = _MainDoc.Tables.Add(_WordApplicMain.Selection.Range, rowNumber, columnNumber, ref Nothing, ref Nothing);
            _WordTable.DesginTableEvent += _DesginTable;
            _WordTable.ConstructTable(columnWeight);
        }
        /// <summary>
        /// 设计表格样式
        /// </summary>
        /// <param name="table">要设计的表格</param>
        /// <param name="columnWeight">表格的宽度</param>
        protected virtual void _DesginTable(ref Microsoft.Office.Interop.Word.Table table, List<int> columnWeight)
        {
            DesginTable(ref table, columnWeight);
            //for (int i = 0; i < table.Rows.Count; i++)
            //    for (int j = 0; j < table.Columns.Count; j++)
            //        table.Cell(i + 1, j + 1).Width = columnWeight[j];
            _WordTable.TableOne = table;
        }
        #endregion
        /// <summary>
        /// 保存word文档
        /// </summary>
        public void SaveWord()
        {
            object missing = System.Reflection.Missing.Value;
            Object Nothing = System.Reflection.Missing.Value;
            object Visible = false;

            try
            {
                object Save_FileName = _SaveFilePath;

                _MainDoc.SaveAs(ref Save_FileName, ref missing, ref missing, ref missing, ref missing,
               ref missing, ref missing, ref missing, ref Visible,
               ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
               ref missing);
                _MainDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                _MainDoc = null;
                _WordApplicMain.Quit(ref Nothing, ref Nothing, ref Nothing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
