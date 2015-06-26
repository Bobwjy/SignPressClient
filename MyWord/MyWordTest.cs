using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.MyWord.MyWordTable;
using Microsoft.Office.MyWord.MyWord;

namespace Microsoft.Office.MyWord.MyWord
{
    public class MyWordTest
    {


        public void WordTestNoTable()
        {
            MyWord myWord = new MyWord(@"D:\User\lufangtao\WordTestNoTable1.doc");
            myWord.CreateWord();
            myWord.SetCurrentPageStyle(new MyWordCurrentStyle { FontBlod = 2, FontSize = 20 });
            myWord.WriteContent("好人一生平安");
            myWord.Feed();
            myWord.SetCurrentPageStyle(new MyWordCurrentStyle { FontBlod = 4, FontSize = 30 });
            myWord.WriteContent("好人一生平安");
            myWord.SetPageFooter("中国海洋大学");
            myWord.SetPageHeader("中华人民共和国");
            myWord.SaveWord();
        }

        public void WordTestWithTable()
        {
            MyWord myWord = new MyWord(@"D:\User\lufangtao\WordTestWithTable6.doc");
            myWord.CreateWord();
            myWord.DesginTable += _DesginTable;
            int row = 10;
            int column = 10;
            List<int> columnWeight = new List<int>();
            for (int i = 1; i < 11; i++)
                columnWeight.Add(40);
            myWord.CreateTable(row, column, columnWeight);
            WordTableCell mergeTableCell = new WordTableCell(1, 3);
            WordTableCell mergeTabledCell = new WordTableCell(1, 4);
            //  myWord.WordTable.MergeTitleCell(mergeTableCell, mergeTabledCell);
            //  myWord.WordTable.MergeCell(mergeTableCell, mergeTabledCell);
            myWord.WordTable.AddRowEndTable();
            myWord.WordTable.AddColumnEndTable();
            Dictionary<WordTableCell, string> tableContent = new Dictionary<WordTableCell, string>();
            tableContent[mergeTableCell] = "1,3";
            tableContent[mergeTabledCell] = "1,4";
            myWord.WordTable.FillingTableContent(tableContent);
            myWord.SaveWord();
        }

        public void WordTestWithTemplateTable()
        {
            MyWordWithTemplate myWordWithTemplate = new MyWordWithTemplate(@"D:\User\lufangtao\G-大洋调查现场使用样品登记表.doc", @"D:\User\lufangtao\WordTestWithTemplateTable1.doc");
            myWordWithTemplate.CreateWord();
            myWordWithTemplate.OpenWordTemplate();
            myWordWithTemplate.CopyAndPasteTemplateTable();
            Dictionary<string, string> replaceLabel = new Dictionary<string, string>();
            replaceLabel["RequistionNum"] = "1001";
            myWordWithTemplate.ReplaceLabel(replaceLabel);
            myWordWithTemplate.SaveTemplateWord();
          
        }

        public  void _DesginTable(ref Microsoft.Office.Interop.Word.Table table, List<int> columnWeight)
        {
            for (int i = 0; i < table.Rows.Count; i++)
                for (int j = 0; j < table.Columns.Count; j++)
                    table.Cell(i + 1, j + 1).Width = columnWeight[j];
        }
    }
}

