using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// MySQL library...
using MySql.Data.MySqlClient;

/// WPS library...
//using Office;
//using Word;

/// OFFICE library...
//using Microsoft.Office.Interop.Word;
//using Word = Microsoft.Office.Interop.Word;
using MSWord = Microsoft.Office.Interop.Word;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Word;

/// 添加JSON支持
///  
//  方法1
//  怎样在C#中使用json字符串 
//  http://jingyan.baidu.com/article/6fb756ecd2b051241858fbef.html
//  在NuGet程序包管理器中在线搜索“json
//
//  方法2
//  NuGet  http://www.nuget.org/packages/Newtonsoft.Json
//  Json.NET is a popular high-performance JSON framework for .NET
//  To install Json.NET, run the following command in the Package Manager Console
//  PM> Install-Package Newtonsoft.Json -Version 6.0.8

//  http://json.codeplex.com/
//  http://www.cnblogs.com/QLJ1314/p/3862583.html
//  http://www.cnblogs.com/ambar/archive/2010/07/13/parse-json-via-csharp.html
/// 使用JsonConvert对象的SerializeObject只是简单地将一个list或集合转换为json字符串。
/// 但是，有的时候我们的前端 框架比如ExtJs对服务端返回的数据格式是有一定要求的，
/// 
/// 这时就需要用到JSON.NET的LINQ to JSON，LINQ to JSON的作用就是根据需要的格式来定制json数据。
/// 使用LINQ to JSON前，需要引用Newtonsoft.Json的dll和using Newtonsoft.Json.Linq的命名空间。
/// 
/// LINQ to JSON主要使用到JObject, JArray, JProperty和JValue这四个对象，
/// JObject用来生成一个JSON对象，简单来说就是生成”{}”，
/// JArray用来生成一个JSON数 组，也就是”[]”，
/// JProperty用来生成一个JSON数据，格式为key/value的值，
/// 而JValue则直接生成一个JSON值。下面我们就 用LINQ to JSON返回上面分页格式的数据。代码如下：
/// http://www.cnblogs.com/QLJ1314/p/3862583.html
//// Java用Json序列化对象方法：
//
// 序列化： 
//　　JsonConvert.SerializeObject（string）； 
//　　反序列化： 
//　　JsonConvert.DeserializeObject（obj）； 
/*
 * Java可以用开源项目google-gson，
 * 在项目中导入这个项目的第三方jar包，
 * 然后添加引用：import com.google.gson.Gson；
 * 就可使用以下方法： 
Java用Json反序列化对象方法：

Gson gson = new Gson();
序列化： 
　　Gson gson=new Gson（）； 
　　String s=gson.toJson（obj）； 
反序列化： 
　　Gson gson=new Gson（）； 
　　Object obj=gson.fromJson（s，Object.class）； 
s是经过Json序列化的对象，字符串类型；TestEntity是目标类型
注意：使用fromJson方法反序列化一个对象时，该对象的类型必须显示的声明一个不带参数的构造方法
TestEntity te = gson.fromJson(s,TestEntity.class);*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;


///  本项目的命名空间引用
using SignPressServer.SignData;     //  数据存储
using SignPressServer.SignTools;    //  处理工具
using SignPressServer.SignDAL;      //  数据库处理

/// 通信方案
/// webservice + json/xml
/// socket + json
using SignPressServer.SignSocket.AsyncSocket;   //  套接字信息
using SignPressServer.SignSocket.AsyncTcpListener;  //  
using SignPressServer.SignSocket.SyncSocket;

///  测试正则表达式
using System.Text.RegularExpressions;
/// 

///  modify by gatieme at 2015-07-09 11:12
///  新增加功能签字人对会签单的权限问题，
///  在SignatureTemplate中增加了canview和candownload权限
/// 
///  新增的功能会签单前缀信息 [比如信内，信界]
///  通常来说, 前缀跟模板相关  模板跟部门相关就可以了
///  即，正常来说，一个部门使用一套签字模版，一个部门有几个固定的前缀
///
///  那么我们最终的设计可以这样，
///  签字模版中增加一个部门标识，用于标识使用此模版的部门信息
///  我们的前缀信息这样设计，前缀，前缀表 == id + 前缀信息串 + 使用部门
///  
///  这样用户在提交会签单的时候，

/*
 * SignPress程序的服务器程序
 * 
 */
namespace SignPressServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====================");
            Console.WriteLine("SignPressServer 1.0.0");
            Console.WriteLine("=====================");

            Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Console.WriteLine(System.DateTime.Now.ToString("yyyyMMddHHmmss"));


            //获取当前进程的完整路径，包含文件名(进程名)。
            //Console.WriteLine(this.GetType().Assembly.Location);
            //result: X:\xxx\xxx\xxx.exe (.exe文件所在的目录+.exe文件名)

            //获取新的Process 组件并将其与当前活动的进程关联的主模块的完整路径，包含文件名(进程名)。
            //Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //result: X:\xxx\xxx\xxx.exe (.exe文件所在的目录+.exe文件名)

            //获取和设置当前目录（即该进程从中启动的目录）的完全限定路径。
            //Console.WriteLine(System.Environment.CurrentDirectory);
            //result: X:\xxx\xxx (.exe文件所在的目录)

            //获取当前 Thread 的当前应用程序域的基目录，它由程序集冲突解决程序用来探测程序集。
            //Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory);
            //result: X:\xxx\xxx\ (.exe文件所在的目录+"\")

            //获取和设置包含该应用程序的目录的名称。
            //Console.WriteLine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            //result: X:\xxx\xxx\ (.exe文件所在的目录+"\")

            //获取应用程序的当前工作目录(不可靠)。
            //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            //result: X:\xxx\xxx (.exe文件所在的目录)
            /// 测试连接服务器以及查询测试


            //int currDayConCount = DALHDJContract.GetDayHDJContractCount(System.DateTime.Now.Date);
            //Console.WriteLine(System.DateTime.Now.ToString("yyyyMMdd") + (currDayConCount + 1).ToString().PadLeft(6, '0'));


            #region 测试正则表达式
//            String[] text = {   "username/password@//myserver:1521/my.service.com", 
//                                "username/password@myserver//instancename",
//                                "username/password@myserver/myservice:dedicated/instancename"};
//            //Regex regex = new Regex(@"\S*/\S*@/*(\S*):(\S*)/(\S*)", RegexOptions.None);     // text1
//            //Regex regex = new Regex(@"\S*/\S*@/*(\S*):(\d*|/)/(\S*)", RegexOptions.None);     // text1
//            //Regex regex = new Regex(@"\S*/\S*@/*(\S*):(\d*|/*)/(\S*)", RegexOptions.None);     // text1

//            // 在第二个串中@后的`/`可省, myserver:1521中的:port也可以缺省, 
//            //Regex regex = new Regex(@"\S*/\S*@/*(\S*)[:*(\S*) | (/*)]/{1,2}(\S*)", RegexOptions.None);     // text1 + text2
//            //Regex regex = new Regex(@"\S*/\S*@/*(\w*)(:*)(\d*)/*(\S*)", RegexOptions.None);     // text1 + text2

//            // 在第三个串中, 
////            Regex regex = new Regex(@"\S*/\S*(@/{0,2})(\w*)(:*)(\d*)/*(\S*)(:*)(\S*)/(\S*)", RegexOptions.None);     // text1 + text2
//            Regex regex = new Regex(@"\S*/\S*(@/{0,2})(\w*):*(\d*)/*\S*:*\S*/(\S*)", RegexOptions.None);     // text1 + text2

//            //Console.WriteLine(text1);
//            for(int i = 0; i < 3; i++)
//            {
//                MatchCollection matchCollection = regex.Matches(text[i]);
//                Console.WriteLine("Count = " + matchCollection.Count);
//                for (int j = 0; j < matchCollection.Count; j++)
//                {
//                    Match match = matchCollection[j];
//                    Console.WriteLine("Match[{0}]========================", j);
//                    for (int k = 0; k < match.Groups.Count; k++)
//                    {
//                        Console.WriteLine("Groups[{0}]={1}", k, match.Groups[k].Value);
//                    }
//                }
//            }
            #endregion



            #region 测试数据库连接

            /*DBTools dbtools = new DBTools();
            MySqlDataReader mysqlread = dbtools.getmysqlread("SELECT * FROM department;");
            while (mysqlread.Read( ))   // 一次读一条记录
            {
                Console.WriteLine(mysqlread["id"].ToString( ) + "  " + mysqlread["name"].ToString( ));
            }*/

            // 测试添加员工
            /*
            Employee em = new Employee
            {
                Id = 9,
                Name = "王盼盼",
                Position = "局长",
                Department = new Department { Id = 5, Name = "行政科" },
                Username = "wangpanpan",
                Password = "wangpanpan"
            };
            
            DALEmployee.InsertEmployee(em);
            */

            //  测试删除用户
            //Console.WriteLine(DALEmployee.DeleteEmployee(25));

            //  测试用户登录

            //DALEmployee.LoginEmployee("chengjian", "chengjian");
            //DALEmployee.ModifyEmployeePassword(8, "wujiayi");
            /*Department depart = new Department { Id = 7, Name = "赵强科" };
            DALDepartment.InsertDepartment(depart);
            DALDepartment.ModifyDepartmentName(depart.Id, "强哥科");
            DALDepartment.DeleteDepartment(depart.Id);
            Employee employee = DALEmployee.GetEmployee(1);
            employee.Show();*/

            // 向数据库中查询部门的信息

            /*List<Department> departments = DALDepartment.QueryDepartment();
            foreach (Department department in departments)
            {
                Console.WriteLine(department);
            }*/
            /*
            Employee em1 = DALEmployee.GetEmployee(1);
            Console.WriteLine(em1);
            Employee em2 = DALEmployee.GetEmployee("che1ngjian");
            Console.WriteLine(em2);
            */
            //User user = new User { Username = "gatieme", Password = "gatieme" };
            //DALEmployee.IsEmployeePasswordRight(user);
            //Console.WriteLine(DALEmployee.QueryDepartmentEmployeeCount(2));

            SDepartment department = new SDepartment { Name = "很多", ShortCall = "很", CanBoundary = "否", CanRegular = "否", CanInland = "否", CanEmergency = "否" };
            DALSDepartment.InsertSDepartment(department);
            #endregion



            #region Word操作
            //Start Word and create a new document.  
            /*
             * 首先添加COM组件Office，
             * 然后添加.Net组件引用Microsoft.Office.Interop.Word;  
             * 接着添加如下的代码
             * using Office；
             * using Microsoft.Office.Interop.Word;
             * using Word = Microsoft.Office.Interop.Word;  
             */
            /// 对word的操作信息
            //  将一个创建的WORD保存为PDF
            //MSWordTools.WordConvertToPdf(@"G:\[B]CodeRuntimeLibrary\[E]GitHub\\测试文档.doc",
            //    @"G:\[B]CodeRuntimeLibrary\[E]GitHub\测试文档.pdf");              将一个创建的WORD保存为PDF

            //String filePath = MSWordTools.DEFAULT_HDJCONTRACT_PATH + "20150712000005.pdf";
            //if (!(File.Exists((String)filePath)))     // 首先检测文件是否存在
            //{
            //    MSWordTools.KillWordProcess();
            //    String wordPath = MSWordTools.DEFAULT_HDJCONTRACT_PATH + "20150712000005.doc";
            //    HDJContract contract = DALHDJContract.GetHDJContactAgree("20150712000005");       // 获取待生成的会签单信息
            //    MSWordTools.CreateHDJContractWordWithReplace(contract, wordPath);
            //    MSWordTools.WordConvertToPdf(wordPath, filePath);

            //    File.Delete((String)wordPath);
            //    MSWordTools.KillWordProcess();
            //}
            //else
            //{
            //    Console.WriteLine("文件存在无需在生成");
            //}
            //MSWordTools.WordConvertToPdf();

            

            #endregion


            #region 测试JSON数据
            //Action<object> log = o => Console.WriteLine(o);


            //            var e1 = new Employee
            //            {
            //                Id = 1,
            //                Name = "成坚",
            //                Position = "科长",
            //                Department = new Department { Id = 1, Name = "申请科" },
            //                CanSubmit = true,
            //                CanSign = true,
            //                IsAdmin = true,
            //                User = new User { Username = "chengjian", Password = "chengjian" },

            //            };
            //            var e2 = new Employee
            //            {
            //                Id = 1,
            //                Name = "吴佳怡",
            //                Position = "局长",
            //                Department = new Department { Id = 5, Name = "行政科" },
            //                CanSubmit = true,
            //                CanSign = true,
            //                IsAdmin = true,
            //                User = new User{ Username = "wujiayi", Password = "wyujiayi"},
            //            };
            //            e1.Show();
            //            e2.Show();
            //            //序列化 参数是要序列化的对象;json是对象序列化后的字符串
            //            String json = JsonConvert.SerializeObject(new Employee[] { e1, e2 });
            //            Console.WriteLine(json);
            //            //Employee是目标类型；json是经过Json序列化的对象，字符串形式
            //            List<Employee> employList = JsonConvert.DeserializeObject<List<Employee>>(json);
            //            JArray ja = JArray.Parse(json);
            //            Console.WriteLine(ja);	//注意，格式化过的输出
            //            foreach (Employee employ in employList)
            //            {
            //                employ.Show();
            //            }


            #endregion

            #region 获取本机的IP
            //QUERY_UNSIGN_CONTRACT_REQUEST; 1; 1QUERY_SIGN_REFUSE_REQUEST; 1; 1
            //QUERY_REQUEST;0; QUERY_SIGN_REFUSE_REQUEST; 1; 1
            //QUERY_SIGN_REFUSE_REQUEST;1; 1; QUERY_REQUEST;0
            //QUERY_REQUEST;0; QUERY_REQUEST;0 

            ////string s1 = "QUERY_REQUEST; QUERY_SIGN_REFUSE_REQUEST;1;1;";
//            AsyncSocketMessage message = new AsyncSocketMessage(ClientRequest.LOGIN_REQUEST);
//            message.Package = @"QUERY_UNSIGN_CONTRACT_RE
//            QUEST11QUERY_SIGN_REFUSE_REQU
//            EST1QUERY_SIGN_REFUSE_REQUES1";
//            // 3 5 7 9 11
//            do
//            {
//                if (message.Split() == AsyncSocketMessageFlag.MESSAGE_UNKOWN)   // 将数据包分割
//                {
//                    break;
//                }
//            }while(message.Flag != AsyncSocketMessageFlag.MESSAGE_RIGHT);
            string hostName = System.Net.Dns.GetHostName();//本机名   
            System.Net.IPAddress[] addressList = System.Net.Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            Console.WriteLine("HOST Name = " + hostName);
            foreach (System.Net.IPAddress ip in addressList)
            {
                Console.WriteLine("IP Address = " + ip.ToString());
            }
            #endregion


            //for (int row = 6, cnt = 0; row <= 8; row++)    // 填写表格的签字人表头
            //{

            //    for (int col = 1; col <= 3; col += 2, cnt++)
            //    {

            //        Console.WriteLine("签字人信息位置{0}, {1} ==== 签字人序号{2} ==== 签字位置{3},{4}", row, col, cnt, row, col + 1);

            //    }
            //}



            #region 服务器的处理程序AsyncSocketServer

            Console.WriteLine("服务器准备中...");
            const int PORT = 6666;
            //System.Net.IPEndPoint ep = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("10.0.209.144"), 6666);
            AsyncSocketServer server = new AsyncSocketServer(PORT);
            while (true)
            {
                server.Start();
            }
            //SocketTCPServer server = new SocketTCPServer(6666);
            //server.Start();


            #endregion
        }
    }
}
