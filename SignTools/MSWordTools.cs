using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// WPS library...
//using Office;
//using Word;

/// OFFICE library...
//using Microsoft.Office.Interop.Word;
//using Word = Microsoft.Office.Interop.Word;
using MSWord = Microsoft.Office.Interop.Word;
using System.IO;
//using System.Reflection;
//using Microsoft.Office.Interop.Word;

using SignPressServer.SignContract;

namespace SignPressServer.SignTools
{
    /*
     * 操作word的类库MSWordTools
     * 参考: http://www.cnblogs.com/eye-like/p/4121219.html
     * 
     * 创建Word 文档所使用的主要方法是通过微软公司提供的Microsoft Word X Object Library，
     * 其中X 为版本号。
     * Word 2010  对应  14.0, 
     * Word 2007  对应  12.0,
     * Word 2003  对应  11.0.
     */
    class MSWordTools
    {

        // 系统默认的员工签名图片的存储路径
        private const String DEFAULT_SIGNATURE_PATH = @".\\signature\\";

        #region  创建一个WORD文档
        public void CreateWord(Object filePath)
        {
            MSWord._Application wordApp;             //  Word应用程序变量
            MSWord._Document wordDoc;                //  Word文档变量

            //MSWord.Application wordApp;             //  Word应用程序变量
            //MSWord.Document wordDoc;                //  Word文档变量

            wordApp = new MSWord.Application( );    //初始化
            
            if (File.Exists((String)filePath))
            {
                File.Delete((String)filePath);
            }
            
            //由于使用的是COM 库，因此有许多变量需要用Missing.Value 代替
            Object Missing = System.Reflection.Missing.Value;
            //新建一个word对象
            wordDoc = wordApp.Documents.Add(ref Missing, ref Missing, ref Missing, ref Missing);
            
            //WdSaveDocument为Word2003文档的保存格式(文档后缀.doc)\wdFormatDocumentDefault为Word2007的保存格式(文档后缀.docx)
            Object format = MSWord.WdSaveFormat.wdFormatDocument;
            
            //将wordDoc 文档对象的内容保存为DOC 文档,并保存到filePath指定的路径
            wordDoc.SaveAs(ref filePath, ref format, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing);
            //关闭wordDoc文档
            
            wordDoc.Close(ref Missing, ref Missing, ref Missing);
            //关闭wordApp组件对象
            wordApp.Quit(ref Missing, ref Missing, ref Missing);
            Console.WriteLine(filePath + ": Word文档创建完毕!')...");
        }
        #endregion


        #region  在WORD中添加图片
        public void AddWordPic(Object filePath, Object picPath)
        {
            MSWord._Application wordApp;            //  Word应用程序变量
            MSWord._Document    wordDoc;            //  Word文档变量
            
            wordApp = new MSWord.Application( );    //初始化
            if(File.Exists((String)filePath))
            {
                File.Delete((String)filePath);
            }
            Object Missing = System.Reflection.Missing.Value;
            
            wordDoc = wordApp.Documents.Add(ref Missing, ref Missing, ref Missing, ref Missing);

            //定义要向文档中插入图片的位置
            Object range = wordDoc.Paragraphs.Last.Range;
            
            //定义该图片是否为外部链接
            Object linkToFile = false;  //默认
            
            //定义插入的图片是否随word一起保存
            Object saveWithDocument = true;
            
            //向word中写入图片
            wordDoc.InlineShapes.AddPicture((String)picPath, ref Missing, ref Missing, ref Missing);

            Object unite = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            wordApp.Selection.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;//居中显示图片
            wordDoc.InlineShapes[1].Height = 130;
            wordDoc.InlineShapes[1].Width = 200;
            wordDoc.Content.InsertAfter("\n");
            wordApp.Selection.EndKey(ref unite, ref Missing);
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            wordApp.Selection.Font.Size = 10;//字体大小
            wordApp.Selection.TypeText("图1 测试图片\n");

            //WdSaveDocument为Word2003文档的保存格式(文档后缀.doc)\wdFormatDocumentDefault为Word2007的保存格式(文档后缀.docx)
            Object format = MSWord.WdSaveFormat.wdFormatDocument;
            wordDoc.SaveAs(ref filePath, ref format, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing);
            wordDoc.Close(ref Missing, ref Missing, ref Missing);
            wordApp.Quit(ref Missing, ref Missing, ref Missing);
            Console.WriteLine(filePath + ": Word文档创建图片完毕!");
        }
        #endregion


        #region  在WORD中添加表格
        public void AddWordTable(Object filePath)
        {
            MSWord._Application wordApp;             //Word应用程序变量
            MSWord._Document wordDoc;                //Word文档变量
            wordApp = new MSWord.Application();     //初始化
            
            if(File.Exists((String)filePath))
            {
                File.Delete((String)filePath);
            }
            Object Missing = System.Reflection.Missing.Value;
            wordDoc = wordApp.Documents.Add(ref Missing, ref Missing, ref Missing, ref Missing);

            int tableRow = 6;
            int tableColumn = 6;
            
            //定义一个word中的表格对象
            MSWord.Table table = wordDoc.Tables.Add(wordApp.Selection.Range, tableRow, tableColumn, ref Missing, ref Missing);


            wordDoc.Tables[1].Cell(1, 1).Range.Text = "列\n行"; 
            for (int i = 1; i < tableRow; i++)
            {
                for (int j = 1; j < tableColumn; j++)
                {
                    if (i == 1)
                    {
                        table.Cell(i, j+1).Range.Text = "Column " + j;
                    }
                    if (j == 1)
                    {
                        table.Cell(i+1, j).Range.Text = "Row " + i;
                    }
                    table.Cell(i+1, j+1).Range.Text =  i + "行 " + j + "列";
                }
            }


            //添加行
            table.Rows.Add(ref Missing);
            table.Rows[tableRow + 1].Height = 45;
            //向新添加的行的单元格中添加图片
            String fileName = @"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\测试图片.jpg";   //图片所在路径
            Object LinkToFile = false;
            Object SaveWithDocument = true;
            Object Anchor = table.Cell(tableRow+1, tableColumn).Range;//选中要添加图片的单元格
            
            wordDoc.Application.ActiveDocument.InlineShapes.AddPicture((String)fileName, ref LinkToFile, ref SaveWithDocument, ref Anchor);
            wordDoc.Application.ActiveDocument.InlineShapes[1].Width = 75;//图片宽度
            wordDoc.Application.ActiveDocument.InlineShapes[1].Height = 45;//图片高度
            // 将图片设置为四周环绕型
            MSWord.Shape s = wordDoc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
            s.WrapFormat.Type = MSWord.WdWrapType.wdWrapSquare;


            //设置table样式
            table.Rows.HeightRule = MSWord.WdRowHeightRule.wdRowHeightAtLeast;
            table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));

            table.Range.Font.Size = 10.5F;
            table.Range.Font.Bold = 0;

            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalBottom;
            //设置table边框样式
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;

            table.Rows[1].Range.Font.Bold = 1;
            table.Rows[1].Range.Font.Size = 12F;
            table.Cell(1, 1).Range.Font.Size = 10.5F;
            wordApp.Selection.Cells.Height = 40;//所有单元格的高度
            for (int i = 2; i <= tableRow; i++)
            {
                table.Rows[i].Height = 20;
            }
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            
            table.Columns[1].Width = 50;
            for (int i = 2; i <=tableColumn; i++)
            {
                table.Columns[i].Width = 75;
            }


            //添加表头斜线,并设置表头的样式
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGray60;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;

            //表格边框
            /*//表格内容行边框
            table.SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal, Microsoft.Office.Interop.Word.WdColor.wdColorGray20, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth025pt);
            //表格内容列边框
            table.SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical, Microsoft.Office.Interop.Word.WdColor.wdColorGray20, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth025pt);

            SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

            SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

            SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

            SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);
            */
              //合并单元格
            table.Cell(4, 4).Merge(table.Cell(4, 5));//横向合并

            table.Cell(2, 3).Merge(table.Cell(4, 3));//纵向合并


            Object format = MSWord.WdSaveFormat.wdFormatDocument;
            wordDoc.SaveAs(ref filePath, ref format, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing);
            wordDoc.Close(ref Missing, ref Missing, ref Missing);
            wordApp.Quit(ref Missing, ref Missing, ref Missing);
            Console.Write(filePath + ": Word文档创建表格完毕!");
        }

        #endregion


        #region 将WORD转换为PDF
        public static bool WordConvertToPdf(Object wordFilePath, Object saveFilePath)
        {
            bool result                 = false;

            Object readOnly             = true;
            Object isVisible            = true;
            Object confirmConverisons   = false;
            Object openAndRepair        = false;
            Object Missing              = System.Reflection.Missing.Value;

            MSWord._Application wordApp = new MSWord.Application( );
            MSWord._Document    wordDoc = null;
            
            try
            {
                wordDoc = wordApp.Documents.Open((String)wordFilePath, ref confirmConverisons, ref readOnly,
                            ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing,
                            ref Missing, ref Missing, ref isVisible, ref openAndRepair, ref Missing, ref Missing, ref Missing);

           
/*
            if (wordDoc.ActiveWindow.ActivePane.View.Type != WdViewType.wdPrintView)
            {
                if (wordDoc.ActiveWindow.View.SplitSpecial == WdSpecialPane.wdPaneNone)
                    wordDoc.ActiveWindow.ActivePane.View.Type = WdViewType.wdPrintView;
                else
                    wordDoc.ActiveWindow.View.Type = WdViewType.wdPrintView;
            }
            int FilePageCount = GetPageCount(wordDoc);//得到总页数
            try
            {
                if (!System.IO.File.Exists(saveFilePath))
                    wordDoc.ExportAsFixedFormat(saveFilePath, WdExportFormat.wdExportFormatPDF, false, WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportFromTo,
                        1, FilePageCount, WdExportItem.wdExportDocumentContent, false, true, WdExportCreateBookmarks.wdExportCreateNoBookmarks, true, true, false, Missing);
                result = String.Empty;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(ref Missing, ref Missing, ref Missing);
                    wordDoc = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit(ref Missing, ref Missing, ref Missing);
                    wordApp = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
*/

                //  设置保存的格式   
                Object fileFarmat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
                //   保存为PDF   
                wordDoc.SaveAs(ref saveFilePath, ref fileFarmat, ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing,ref Missing);
                result = true;

                Console.WriteLine("将转换WORD文件" + wordFilePath + "转换为" + saveFilePath);
            }
            catch(Exception ex)
            {
                result = false;

                // 输出异常信息
                Console.WriteLine("Error:" + ex.Message);
                Console.WriteLine("Source:" + ex.Source);
                Console.WriteLine("StackTrace:" + ex.StackTrace);
            }
            finally
            {
                //  关闭文档对象 
                if (wordDoc != null)
                {
                    wordDoc.Close(ref Missing, ref Missing, ref Missing);
                    wordDoc = null;
                }
                
                //  退出组件   
                if (wordApp != null)
                {
                    wordApp.Quit(ref Missing, ref Missing, ref Missing);
                    wordApp = null;
                }
            }
            return result;
        }  
        #endregion


        #region 生成拨款会签单

        public static bool CreateYHJLHXMBOContractWord(YHJLHXMBKContract contract, Object saveFilePath)
        {
            Console.WriteLine("开始生成养护及例会项目拨款会签单");
            
            MSWord._Application wordApp;             // Word应用程序变量
            MSWord._Document wordDoc;                // Word文档变量
            wordApp = new MSWord.Application();      //  初始化
            Object Missing = System.Reflection.Missing.Value;


            if (File.Exists((String)saveFilePath))
            {
                File.Delete((String)saveFilePath);
            }
            wordDoc = wordApp.Documents.Add(ref Missing, ref Missing, ref Missing, ref Missing);

            int tableRow = 10;          //  表格的行数
            int tableColumn = 6;        //  表格的列数

            //  定义一个word中的表格对象
            MSWord.Table table = wordDoc.Tables.Add(wordApp.Selection.Range, tableRow, tableColumn, ref Missing, ref Missing);

            // 添加表头信息，同时合并单元格的信息
            Console.WriteLine("开始生成前5行基本栏目的信息");
            for (int row = 1; row < 5/*contract.conTemp.ColumnCount*/; row++)              // 循环每行
            {
                table.Cell(row, 1).Range.Text = contract.ConTemp.ColumnNames[0];    //  添加表头信息
                table.Cell(row, 2).Merge(table.Cell(row, 4));                       //  横向合并
                table.Cell(row, 2).Range.Text = contract.ColumnDatas[row];
            }

            //  开始添加审核人的签名图片信息和备注信息
            Console.WriteLine("开始生成后8行签字栏目的信息");
            for (int row = 6, cnt = 0; row < 8; row++)    // 填写表格的签字人表头
            {
                for (int col = 1; col <= 3; col += 2, cnt++)
                {
                    Console.WriteLine("签字人信息位置{0}, {1} ==== 签字人序号{2} ==== 签字位置{3},{4}", row, col, cnt, row, col + 1);

                    ///////
                    // 填写第row行第一个签字人的签字信息[表头 + 签字信息 + 备注]
                    ///////
                    /// 填写第row行第一个签字人的表头
                    /// [签字人表头坐标(row, col), 签字人序号cnt]
                    table.Cell(row, col).Range.Text = contract.ConTemp.SignDatas[cnt].SignInfo;

                    // 插入第row行第一个人签字人的签字图片
                    //[签字人签字位置坐标(row, col + 1)] 
                    String signFileName = DEFAULT_SIGNATURE_PATH + contract.ConTemp.SignDatas[cnt].SignEmployee.Id + ".jpg";   //图片所在路径
                    Object LinkToFile = false;
                    Object SaveWithDocument = true;
                    Object Anchor = table.Cell(row, col + 1).Range;//选中要添加图片的单元格

                    wordDoc.Application.ActiveDocument.InlineShapes.AddPicture((String)signFileName, ref LinkToFile, ref SaveWithDocument, ref Anchor);
                    wordDoc.Application.ActiveDocument.InlineShapes[1].Width = 75;//图片宽度
                    wordDoc.Application.ActiveDocument.InlineShapes[1].Height = 45;//图片高度

                    // 将图片设置为四周环绕型
                    MSWord.Shape s = wordDoc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
                    s.WrapFormat.Type = MSWord.WdWrapType.wdWrapSquare;
                }
            }

            //设置table样式
            table.Rows.HeightRule = MSWord.WdRowHeightRule.wdRowHeightAtLeast;
            table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));

            table.Range.Font.Size = 10.5F;
            table.Range.Font.Bold = 0;

            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalBottom;
            //设置table边框样式
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;

            table.Rows[1].Range.Font.Bold = 1;
            table.Rows[1].Range.Font.Size = 12F;
            table.Cell(1, 1).Range.Font.Size = 10.5F;
            wordApp.Selection.Cells.Height = 40;//所有单元格的高度
            for (int i = 2; i <= tableRow; i++)
            {
                table.Rows[i].Height = 20;
            }
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

            table.Columns[1].Width = 50;
            for (int i = 2; i <= tableColumn; i++)
            {
                table.Columns[i].Width = 75;
            }

            Object format = MSWord.WdSaveFormat.wdFormatDocument;
            wordDoc.SaveAs(ref saveFilePath, ref format, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing);
            wordDoc.Close(ref Missing, ref Missing, ref Missing);
            wordApp.Quit(ref Missing, ref Missing, ref Missing);
            Console.Write(saveFilePath + ": Word文档创建表格完毕!");

            return true;
        }
        #endregion
    }       // end of MSWordTools Class

}   // end of NameSpace



//MSWord._Application wordApp;             //Word应用程序变量
//MSWord._Document wordDoc;                //Word文档变量
//wordApp = new MSWord.Application();     //初始化

//if (File.Exists((String)filePath))
//{
//    File.Delete((String)filePath);
//}
//Object Missing = System.Reflection.Missing.Value;
//wordDoc = wordApp.Documents.Add(ref Missing, ref Missing, ref Missing, ref Missing);

//int tableRow = 6;
//int tableColumn = 6;

////定义一个word中的表格对象
//MSWord.Table table = wordDoc.Tables.Add(wordApp.Selection.Range, tableRow, tableColumn, ref Missing, ref Missing);


//wordDoc.Tables[1].Cell(1, 1).Range.Text = "列\n行";
//for (int i = 1; i < tableRow; i++)
//{
//    for (int j = 1; j < tableColumn; j++)
//    {
//        if (i == 1)
//        {
//            table.Cell(i, j + 1).Range.Text = "Column " + j;
//        }
//        if (j == 1)
//        {
//            table.Cell(i + 1, j).Range.Text = "Row " + i;
//        }
//        table.Cell(i + 1, j + 1).Range.Text = i + "行 " + j + "列";
//    }
//}


////添加行
//table.Rows.Add(ref Missing);
//table.Rows[tableRow + 1].Height = 45;
////向新添加的行的单元格中添加图片
//String fileName = @"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\测试图片.jpg";   //图片所在路径
//Object LinkToFile = false;
//Object SaveWithDocument = true;
//Object Anchor = table.Cell(tableRow + 1, tableColumn).Range;//选中要添加图片的单元格

//wordDoc.Application.ActiveDocument.InlineShapes.AddPicture((String)fileName, ref LinkToFile, ref SaveWithDocument, ref Anchor);
//wordDoc.Application.ActiveDocument.InlineShapes[1].Width = 75;//图片宽度
//wordDoc.Application.ActiveDocument.InlineShapes[1].Height = 45;//图片高度
//// 将图片设置为四周环绕型
//MSWord.Shape s = wordDoc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
//s.WrapFormat.Type = MSWord.WdWrapType.wdWrapSquare;


////设置table样式
//table.Rows.HeightRule = MSWord.WdRowHeightRule.wdRowHeightAtLeast;
//table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));

//table.Range.Font.Size = 10.5F;
//table.Range.Font.Bold = 0;

//table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
//table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalBottom;
////设置table边框样式
//table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
//table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;

//table.Rows[1].Range.Font.Bold = 1;
//table.Rows[1].Range.Font.Size = 12F;
//table.Cell(1, 1).Range.Font.Size = 10.5F;
//wordApp.Selection.Cells.Height = 40;//所有单元格的高度
//for (int i = 2; i <= tableRow; i++)
//{
//    table.Rows[i].Height = 20;
//}
//table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
//table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

//table.Columns[1].Width = 50;
//for (int i = 2; i <= tableColumn; i++)
//{
//    table.Columns[i].Width = 75;
//}


////添加表头斜线,并设置表头的样式
//table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
//table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorGray60;
//table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt;

////表格边框
///*//表格内容行边框
//table.SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal, Microsoft.Office.Interop.Word.WdColor.wdColorGray20, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth025pt);
////表格内容列边框
//table.SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical, Microsoft.Office.Interop.Word.WdColor.wdColorGray20, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth025pt);

//SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

//SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

//SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);

//SetTableBorderStyle(table, Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom, Microsoft.Office.Interop.Word.WdColor.wdColorGray50, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth050pt);
//*/
////合并单元格
//table.Cell(4, 4).Merge(table.Cell(4, 5));//横向合并

//table.Cell(2, 3).Merge(table.Cell(4, 3));//纵向合并


//Object format = MSWord.WdSaveFormat.wdFormatDocument;
//wordDoc.SaveAs(ref filePath, ref format, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing, ref Missing);
//wordDoc.Close(ref Missing, ref Missing, ref Missing);
//wordApp.Quit(ref Missing, ref Missing, ref Missing);
//Console.Write(filePath + ": Word文档创建表格完毕!");