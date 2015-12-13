using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// 引入会签单模版类
using SignPressServer.SignData;

namespace SignPressServer.SignContract
{
    /****
     * 
     *  养护及例会项目拨款会签审批单
     *  
     *  此类继承自签字模版类ContractTemplate
     *  签字模版中只包含了该有的签字的基本数据项，
     *  现实中就对应了一张签字的空白表格
     *  
     *  而YHLLHXMBKContract（养护及例会项目拨款会签审批单）
     *  则对应了客户申请时，填写的数据项
     *  
     *  但是应该注意
     *  签字单子在确认为一个模版后，那么他的签字顺序就已经确定了
     *  提交申请的人就只能按照提交签字的顺序走，
     *  因此签字人的的顺序是在模版中的
     * 
     *  [问题2015/6/14 10:19]
     *  我们将签字也设计进模版了
     *  一个单子的申请人应该也需要签字，很有可能是第一个人或者前两个人
     *  那么如果这两个人也设计进入模版中的话，那么每个申请人都会需要一个模版
     *  这样子后面的几个人在模版中就显得非常冗余
     *  比如张三和李四分别提交的会签单中，可能只是前两个人不一样，后面的人都是一个样式
     *  这样级要考虑是否需要将前面两个人设计进入签字模版
     *  
     *  如果真的前两个人即是申请人又是签字人，有一下设计方案
     *  [1] 把这类人员设计进签字模版，那么同样一张表可能不同的申请人就得设计不同的模版
     *  这样同样一份一个单子模版就有好几类，对应每一个提交人就有一个模版，模版很冗余
     *  而且只能在提交人提交单子的时候选择模版。
     *  这样一来的话， 让一个提交人员可以看到或者提交别的提交人的模版是很不好的
     *  一种解决方案是提交单子的时候，判断用户，只提交自己的单子，
     *  
     *  [2] 把后面的6个人设计进签字模版，前面两个人由提交人员指定，
     *  这里面有个优化方案，就是提交的时候用户后面的签字顺序是定死的，
     *  前面两个应该有一个是自己，有一个是自己的直接领导，
     *  自己已经无需指定，直接同意即可
     * 
     *  目前暂时采用第一种方案，因为麻烦点，麻烦点，但是可以排除提交人员不是签字人的情况出现的大BUG

     ****/
    class YHJLHXMBKContract /*  :  ContractTemplate  */
    {

        /// <summary>
        ///  构造函数
        /// </summary>
        private ContractTemplate m_conTemp;       // 会签单的模版信息
        public ContractTemplate ConTemp
        {
            get { return this.m_conTemp; }
            set { this.m_conTemp = value; }
        }
        
        private String m_id;            //  审批会签单编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private List<String> m_columnDatas;     //  存储会签单
        public List<String> ColumnDatas
        {
            get { return this.ColumnDatas; }
            set { this.m_columnDatas = value; }
        }
        
        
        private String m_proName;      //  工程名称
        public String ProName
        {
            get { return this.m_proName; }
            set { this.m_proName = value; }
        }

        private String m_termName;      //  项目名称
        public String TermName
        {
            get { return this.m_termName; }
            set { this.m_termName = value; }
        }

        private String m_termSize;      //  主要项目以及工作量(养护及例会项目拨款会签审批单)
        public String TermSize
        {
            get { return this.m_termName; }
            set { this.m_termSize = value; }
        }

        private int m_reqCaptial;      //  本次申请资金额度（元）(养护及例会项目拨款会签审批单--)
        public int ReqCapial
        {
            get { return this.m_reqCaptial; }
            set { this.m_reqCaptial = value; }
        }

        private int m_totalCaptial;       //  累计申请资金额度（元）
        public int TotalCaptial
        {
            get { return this.m_totalCaptial; }
            set { this.m_totalCaptial = value; }
        }


        private int m_submitId;                 //  提交会签单的人名单
        public int SubmitId
        {
            get { return this.m_submitId; }
            set { this.m_submitId = value; }
        }


        /*  以下字段考虑是用员工数类表示还是用员工表的主键来表示
        private int m_reqDepartProId;           // 申请单位项目负责人的员工编号
        public int ReqDepartProId
        {
            get { return this.m_reqDepartProId; }
            set { this.m_reqDepartProId = value; }
        }

        private int m_reqDepartId;              //  申请单位负责人的员工编号
        public int ReqDepartId
        {
            get { return this.m_reqDepartId; }
            set { this.m_reqDepartId = value; }
        }
        
        
        private int m_conDepartProId;           //  养护主管部门项目负责人（需要签字）
        public int ConDepartProId
        {
            get { return this.m_conDepartProId; }
            set { this.m_conDepartProId = value; }
        }
        
        private int m_conDepartId;              //  养护主管部门负责人（需要签字）
        public int ConDepartId
        {
            get { return this.m_conDepartProId; }
            set { this.m_conDepartProId = value; }
        }
        
        private int m_planDepartId;             //  计划科负责人（需要签字）
        public int PlanDepartI
        {
            get { return this.m_planDepartId; }
            set { this.m_planDepartId = value; }
        }
        
        private int m_finaDepartId;             //  财务科负责人（需要签字）
        public int FinalDepartId
        {
            get { return this.m_finaDepartId; }
            set { this.m_finaDepartId = value; }
        }

        private int m_                      ·//  副局长
        private int m_director               //  局长
        */

        /// <summary>
        ///  构造函数
        /// </summary>
        /*public YHJLHXMBKContract()
        :this()
        { 
        }*/

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conTemp"></param>
        public YHJLHXMBKContract(ContractTemplate conTemp)
        {
            this.m_conTemp = conTemp;       //  模版类信息
        }
        

        /// <summary>
        /// 依照当前的会签单信息生成PDF文件
        /// </summary>
        /// <returns>返回生成的PDF文件的路劲信息</returns>
        public String CreateContractFile()
        {
            return null;
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

        }
    }
}
