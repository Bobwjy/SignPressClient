using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Office.Core;

using MSExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;

using SignPressServer.SignDAL;
using SignPressServer.SignData;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;


namespace SignPressServer.SignTools
{
    public class MSExcelTools
    {
        public static String DEFAULT_ROOT_PATH = System.AppDomain.CurrentDomain.BaseDirectory;

        public static string DEFAULT_CONTEMP_PATH = DEFAULT_ROOT_PATH + "contemp\\";

        public static string DEFAULT_HDJCONTRACT_PATH = DEFAULT_ROOT_PATH + "hdjcontract\\";

        public static string DEFAULT_SIGNATURE_PATH = DEFAULT_ROOT_PATH + "signature\\";
        
        public static string DEFAULT_STATISTIC_PATH = DEFAULT_ROOT_PATH + "statistic\\";

        public static void Test()
        {
            //  创建一个excel应用 
            MSExcel._Application app = new MSExcel.Application();
            
            //  打开一个excel文档
            MSExcel.Workbooks wbks = app.Workbooks;
            MSExcel._Workbook _wbk = wbks.Add(DEFAULT_CONTEMP_PATH + "statistic.xls");
            
            //  获取到excel中的一个sheet
            MSExcel.Sheets shs = _wbk.Sheets;
            MSExcel._Worksheet _wsh = (MSExcel._Worksheet)shs.get_Item(1);

            //Object Missing = System.Reflection.Missing.Value;


            String strValue = ((MSExcel.Range)_wsh.Cells[2, 1]).Text;
            
            Console.WriteLine(strValue);

            //  屏蔽掉系统跳出的Alert
            app.AlertBeforeOverwriting = false;

            //保存到指定目录
//            _wbk.SaveAs("./1.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            _wbk.SaveAs("./1.xls", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        }


        public static void StatisticYearCategory(int year, int categoryId)
        {
            String[] num = {
                               "零", "一", "二", "三", "四", "五", 
                               "六", "七", "八", "九", "十",
                               "十一", "十二", "十三", "十四", "十五",
                               "十六", "十七", "十八", "十九", "二十",

                           };
            //////////////////////////////////////////////////////////////
            /// excel初始化
            //////////////////////////////////////////////////////////////
            //  创建一个excel应用 
            MSExcel._Application app = new MSExcel.Application();

            //  打开一个excel文档
            MSExcel.Workbooks wbks = app.Workbooks;
            MSExcel._Workbook _wbk = wbks.Add(DEFAULT_CONTEMP_PATH + "statistic.xls");

            //  获取到excel中的一个sheet
            MSExcel.Sheets shs = _wbk.Sheets;
            MSExcel._Worksheet sheet = (MSExcel._Worksheet)shs.get_Item(1);


            //////////////////////////////////////////////////////////////
            /// 数据库统计信息
            //////////////////////////////////////////////////////////////
            //  首先获取到当前的会签单的信息
            ContractCategory category = DALContractIdCategory.GetCategory(categoryId);
            Console.WriteLine("统计能申请当前Id = {0}, Category = {1}信息", category.Id, category.Category);
            

            string sheetName = category.CategoryShortCall + year.ToString() + ".xls";
            Console.WriteLine("待保存的excel名 sheetName = " + sheetName);

            //  首先获取数据库中可以申请本会签单类别的所有部门的列表
            List<Department> departments = DALContractIdCategory.QueryCategorySDepartment(categoryId);
            //Console.WriteLine(JsonConvert.SerializeObject(departments));
       
            //  其次查询当前Category类别的所有Project信息和item信息
            List<ContractProject> projects = DALContractProject.QueryCategoryProject(categoryId);
            //Console.WriteLine("统计出当前Category = {1}的所有Project+Item信息", category.CategoryShortCall);
            //Console.WriteLine(JsonConvert.SerializeObject(projects));

            int startRow = 4, row;
            int startCol = 5, col;
            int projectCount = 1, itemCount = 1;
            foreach (Department department in departments)     ///  循环填写每个部门的信息
            {
                //  每次填写部门的信息时

                Console.WriteLine("[{0}, {1}]]开始填写部门{2}的统计信息", startRow, startCol, department.Name);
                row = startRow;
                col = startCol;
                projectCount = 1;
                
                //  可以填写表头, 填写部门的名称
                sheet.Cells[2, col] = department.Name;
                    
                //Console.WriteLine("当前部门Id = {0}, Name = {1}", department.Id, department.Name);
                foreach (ContractProject project in projects)       //  循环每个项目的信息
                {
                    // 填写每行的表头，此处有BUG，每次循环均重复写了，其实只需要第依次填写即可
                    sheet.Cells[row, 1] = num[projectCount];
                    sheet.Cells[row, 2] = project.Project;

                    ///  统计当前部门Department当年Year项目Project的统计信息
                    //Console.WriteLine(JsonConvert.SerializeObject(project));
                    ContractWorkload projectWorkload = DALContractStatistic.StatisSDepartmentYearProjectWorkLoad(department.ShortCall, year, project.Id);
                    //Console.WriteLine("当前工程Id = {0}, Project = {1}, Works = {2}, Expense = {3}", project.Id, project.Project, projectWorkload.Work, projectWorkload.Expense);
                     
                    Console.WriteLine("====[{0}  Project]--{1}-{2}-{3}====", projectCount, department.ShortCall, year, project.Project);

                    sheet.Cells[row, col] = projectWorkload.Work;              //  存储工作量信息
                    sheet.Cells[row, col + 1] = projectWorkload.Expense;       //  存储花费的信息
                    Console.WriteLine("[{0}, {1}], [{2}, {3}]", row, col, row, col + 1);
                    
                    row++;                          //  填写完Project总的统计信息后，增加一行，开始填写分条Item的信息
                    itemCount = 1;
                    //  获取当前项目的工作量集合
                    List<ContractItem> items = DALContractItem.QueryProjectItem(project.Id);
                    foreach (ContractItem item in items)
                    {
                        // 填写每行的表头，此处有BUG，每次循环均重复写了，其实只需要第依次填写即可
                        sheet.Cells[row, 1] = itemCount;
                        sheet.Cells[row, 2] = item.Item;
                        //Console.WriteLine(JsonConvert.SerializeObject(item));

                        //  统计当前部门Department当年Year工作量为item的统计信息
                        ContractWorkload itemWorkload = DALContractStatistic.StatisSDepartmentYearItemWorkLoad(department.ShortCall, year, item.Id);
                        //Console.WriteLine("当前的工作量Id = {0}, Item = {1}, Works = {2}, Expenses = {3}", item.Id, item.Item, itemWorkload.Work, itemWorkload.Expense);
                        Console.WriteLine("========[{0}.{1}]--{2}-{3}-{4}-{5}", projectCount, itemCount, department.ShortCall, year, project.Project,item.Item);
                        //string str = sheet.Rows[row][col].ToString();
                        //sheet.Cells[row, col] = str;  
                        sheet.Cells[row, col] = itemWorkload.Work;              //  存储工作量信息
                        sheet.Cells[row, col + 1] = itemWorkload.Expense;       //  存储花费的信息
                        Console.WriteLine("[{0}, {1}], [{2}, {3}]", row, col, row, col + 1);

                        row++;              //  每次一个填完一个item后，行增加一行填写下一行
                        itemCount++;
                    }

                    projectCount++;

                }

                //  填完了单个部门的所有信息后
                startRow += 0;  //  回复到开始的那一行
                startCol += 2;  //  初始化的列增加两个单元格，跳到下一个部门的列中
            }

            _wbk.SaveAs(DEFAULT_STATISTIC_PATH + sheetName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            
            //_wbk.Close(null, null, null);
            //wbks.Close();
            app.Quit();

            //释放掉多余的excel进程
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app = null;
    
        }

    }
}
