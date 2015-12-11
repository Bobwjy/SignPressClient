using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Office.Core;

using MSExcel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;


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


    }
}
