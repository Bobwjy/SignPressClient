using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SignPressClient.SignLogging
{
    class Logging
    {
        private static string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "log";
        private static string logfilename = filepath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

        /// <summary>
        /// 创建日志文件夹和文件
        /// </summary>
        private static void CreateLogFile()
        {
            if (!Directory.Exists(filepath))
            {
                try
                {
                    Directory.CreateDirectory(filepath);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex + "创建目录失败");
                }
            }
            if (!File.Exists(logfilename))
            {
                FileStream filestream = null;
                try
                {
                    filestream = File.Create(logfilename);
                    /*创建日志头部*/
                    filestream.Dispose();
                    filestream.Close();
                    CreateLogHead();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex + "创建日志文件失败");
                }
            }  
        }

        /// <summary>
        /// 创建日志头部
        /// </summary>
        private static void CreateLogHead()
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(logfilename, true, Encoding.UTF8);
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("【日志创建时间***********】【日志内容******************************************************PDF】");
                sw.WriteLine();
                sw.WriteLine();
            }
            catch { }
            finally
            {
                sw.Flush();
                sw.Dispose();
                sw.Close();
            }  
        }

        public static void AddLog(string message)
        {
            CreateLogFile();
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(logfilename, true, Encoding.UTF8);
                sw.WriteLine();
                sw.WriteLine("【  " + DateTime.Now.ToString() + "  】" + "【  " + message + "  】");
                sw.WriteLine();
            }
            catch (Exception ex)
            {
                throw new Exception(ex + "写入日志失败，检查！");
            }
            finally
            {
                sw.Flush();
                sw.Dispose();
                sw.Close();  
            }
        }
    }
}
