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
            Console.WriteLine("SignPress服务器程序");
            
            /// 测试连接服务器以及查询测试

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
            
            /// WPS
            /*object missing = System.Reflection.Missing.Value;
            string filename = @".\拨款会签单--航道局.doc";
            
            Word.Application oword = new Word.Application();
            Word.Document oDoc = new Word.Document();
            oword.set_Visible(true);
            oDoc.Open(filename);*/

            /// OFFICE
            /*object oMissing = System.Reflection.Missing.Value;  
            object oEndOfDoc = "//endofdoc"; //endofdoc is a predefined bookmark 
            object fileName = @"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\拨款会签单--航道局.doc";
            //Start Word and create a new document.  
            MSWord.Application oWord = new MSWord.Application();
            oWord.Visible = true;  
            // MSWord.Document oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);              
            MSWord.Document oDoc = oWord.Documents.Open(ref fileName,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);*/

            //MSWordTools wordTools = new MSWordTools();
            
            //  创建一个WORD文档
            //wordTools.CreateWord(@"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\1111.doc");
            
            //  在一个创建的WORD文档中添加图片
            //wordTools.AddWordPic(@"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\测试文档.doc",
            //    @"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\测试图片.jpg");
            
            //  在一个创建的WORD文档中添加表格信息
            //wordTools.AddWordTable(@"G:\[B]CodeRuntimeLibrary\[E]GitHub\SignPressServer\测试文档.doc");
            
            //  将一个创建的WORD保存为PDF
            //MSWordTools.WordConvertToPdf(@"G:\[B]CodeRuntimeLibrary\[E]GitHub\\测试文档.doc",
            //    @"G:\[B]CodeRuntimeLibrary\[E]GitHub\测试文档.pdf");
            #endregion


            #region 测试JSON数据
            //Action<object> log = o => Console.WriteLine(o);
            
            /*
            var e1 = new Employee
            {
                Id = 1,
                Name = "成坚",
                Position = "科长",
                Department = new Department { Id = 1, Name = "申请科" },
                CanSubmit = true,
                CanSign = true,
                IsAdmin = true,
                User = new User { Username = "chengjian", Password = "chengjian" },

            };
            var e2 = new Employee
            {
                Id = 1,
                Name = "吴佳怡",
                Position = "局长",
                Department = new Department { Id = 5, Name = "行政科" },
                CanSubmit = true,
                CanSign = true,
                IsAdmin = true,
                User = new User{ Username = "wujiayi", Password = "wyujiayi"},
            };
            e1.Show();
            e2.Show();
            //序列化 参数是要序列化的对象;json是对象序列化后的字符串
            String json = JsonConvert.SerializeObject(new Employee[] { e1, e2 });
            Console.WriteLine(json);
            //Employee是目标类型；json是经过Json序列化的对象，字符串形式
            List<Employee> employList = JsonConvert.DeserializeObject<List<Employee>>(json);
            JArray ja = JArray.Parse(json);
            Console.WriteLine(ja);	//注意，格式化过的输出
            foreach (Employee employ in employList)
            {
                employ.Show();
            }
            */
            #endregion

            #region 获取本机的IP
            string s1 = "所属机构名称;教师姓名;课程类型;课程名称";

            string[] split = s1.Split(';');    //返回由'/'分隔的子字符串数组
            foreach (string s in split)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
            string hostName = System.Net.Dns.GetHostName();//本机名   
            System.Net.IPAddress[] addressList = System.Net.Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            foreach (System.Net.IPAddress ip in addressList)  
            {  
                Console.WriteLine(ip.ToString());
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
            
            // System.Net.IPEndPoint ep = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("10.0.209.144"), 6666);
            AsyncSocketServer server = new AsyncSocketServer(6666);
            while (true)
            {
                server.Start( );
            }
            //SocketTCPServer server = new SocketTCPServer(6666);
            //server.Start();


            #endregion
        }
    }
}
