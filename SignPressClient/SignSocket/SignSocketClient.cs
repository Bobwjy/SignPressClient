using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using SignPressClient.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using SignPressClient.SignLogging;
using System.IO;


namespace SignPressClient.SignSocket
{
     public class SignSocketClient
    {
        IPAddress IP = IPAddress.Parse(ConfigurationManager.AppSettings["ipAddress"]);
        int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        Socket ClientSocket;
        Byte[] recivebuffer = new byte[1024 * 1024 * 8];
        int recLength;

        public SignSocketClient()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ClientSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            

            //  心跳包http://www.cnblogs.com/csMapx/archive/2011/09/04/2166515.html
            uint dummy = 0;
            byte[] inOptionValues = new byte[System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
            BitConverter.GetBytes((uint)120000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));//多长时间开始第一次探测
            BitConverter.GetBytes((uint)120000).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);//探测时间间隔

            ClientSocket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            //  相当于如下代码 
            //byte[] buffer = new byte[12];
            //BitConverter.GetBytes(1).CopyTo(buffer, 0);     //  是否启用Keep-Alive
            //BitConverter.GetBytes(1000).CopyTo(buffer, 4);  //  多长时间开始第一次探测
            //BitConverter.GetBytes(1000).CopyTo(buffer, 8);  //  探测时间间隔

            ///  ClientSocket.IOControl(IOControlCode.KeepAliveValues, buffer, null); 
            ///  因此心跳包的设置我们长采用如下代码
            ///  http://blog.csdn.net/educast/article/details/7597829
            ///  http://www.cnblogs.com/csMapx/archive/2011/09/04/2166515.html
            ///  http://www.baidu.com

            
            try
            {
                ClientSocket.Connect(new IPEndPoint(IP, port));
                ClientSocket.ReceiveTimeout = 1000 * 10;            ///  设置连接超时
                Logging.AddLog("服务器连接成功!");

                try
                {
                    byte[] tmp = new byte[1];
                    ClientSocket.Send(tmp, 0, 0);
                }
                catch (SocketException e)
                {
                    Logging.AddLog(e.ToString());
                }
            }
            catch
            {
                Logging.AddLog("服务器未响应!");
            }
        }
        
        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public Task<Employee> Login(string UserName, string PassWord)
        {
            return Task.Factory.StartNew(() =>
            {
                    User user = new User { Username = UserName, Password = PassWord };
                    SocketMessage sm = new SocketMessage(Request.LOGIN_REQUEST, user);
                    try
                    {
                        //  scoket发送请求信息
                        ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                        //socket接收信息
                        recLength = ClientSocket.Receive(recivebuffer);
                        string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                        string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);
                        
                        if (Msg[0] == Response.LOGIN_SUCCESS.ToString().Trim())
                        {
                            //将接收的信息（json格式）转化为类
                            Employee emp = new Employee();
                            emp = JsonConvert.DeserializeObject<Employee>(Msg[2]);

                            return emp;
                        }
                        else
                        {
                            Logging.AddLog("登录失败!");
                            return null;
                        }
                    }
                    catch
                    {
                        if (ClientSocket.Connected)
                        {
                            return null;
                        }
                        else
                        {
                            Logging.AddLog("连接服务器失败!");
                            return null;
                        }
                    }
                });
        }


        /// <summary>
        /// 退出验证
        /// </summary>
        /// <returns></returns>
        public async Task Quit( )
        {
            //return Task.Factory.StartNew(() =>
            await Task.Run(() =>
            {
                SocketMessage sm = new SocketMessage(Request.QUIT_REQUEST);
                try
                {
                    //  scoket发送请求信息
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));
                }
                catch( Exception ex )
                {
                    Logging.AddLog("退出时异常" + ex.ToString());
                }
            });
        }  


        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        ///  modify by gatieme at 2015-08-08 16:09
        ///  实现功能--为部门添加部门简称
        ///  本函数修改，修改参数string departmentName为Department department
        public string InsertDepartment(Department department)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.INSERT_DEPARTMENT_REQUEST, department);
                    //scoket发送请求信息
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    //scoket接收请求信息
                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.INSERT_DEPARTMENT_SUCCESS.ToString())
                    {
                        Logging.AddLog("添加部门成功!");
                    }
                    else
                    {
                        Logging.AddLog("添加部门失败！");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("添加部门失败（服务器连接中断）!");
                    return "添加失败";
                }
                //});
            }
        }

        /// <summary>
        /// 添加部门权限
        /// </summary>
        /// <param name="sdepartment"></param>
        /// <returns></returns>
        public string InsertSDepartment(SDepartment sdepartment)
        {
            try
            {
                SocketMessage sm = new SocketMessage(Request.INSERT_SDEPARTMENT_REQUEST, sdepartment);
                //scoket发送请求信息
                ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                //scoket接收请求信息
                recLength = ClientSocket.Receive(recivebuffer);
                string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                if (Msg[0] == Response.INSERT_SDEPARTMENT_SUCCESS.ToString())
                {
                    Logging.AddLog("添加部门权限成功!");
                }
                else
                {
                    Logging.AddLog("添加部门权限失败！");
                }
                return Msg[0];
            }
            catch
            {
                Logging.AddLog("添加部门权限失败（服务器连接中断）!");
                return "添加失败";
            }
        }

        /// <summary>
        /// 查询部门列表
        /// </summary>
        /// <returns></returns>
        public List<Department> QueryDepartment()
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_DEPARTMENT_REQUEST);
                    //string request = JsonConvert.SerializeObject(Request.QUERY_DEPARTMENT_REQUEST.ToString());
                    //scoket发送请求信息
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    //scoket接收请求信息
                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_DEPARTMENT_SUCCESS.ToString())
                    {
                        List<Department> list = new List<Department>();

                        list = JsonConvert.DeserializeObject<List<Department>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询部门列表失败!" + recMsg);
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询部门列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

        /// <summary>
        /// 查询部门列表
        /// </summary>
        /// <returns></returns>
        public List<SDepartment> QuerySDepartment()
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_SDEPARTMENT_REQUEST);
                    //string request = JsonConvert.SerializeObject(Request.QUERY_DEPARTMENT_REQUEST.ToString());
                    //scoket发送请求信息
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    //scoket接收请求信息
                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_SDEPARTMENT_SUCCESS.ToString())
                    {
                        List<SDepartment> list = new List<SDepartment>();

                        list = JsonConvert.DeserializeObject<List<SDepartment>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询部门列表失败!" + recMsg);
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询部门列表失败(服务器连接中断)!");
                    return null;
                }
                //});
            }
        }
        /// <summary>
        /// 查询部门员工
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public List<Employee> QueryEmployeeByDepartmentID(int departmentID)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_EMPLOYEE_REQUEST, departmentID);

                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_EMPLOYEE_SUCCESS.ToString())
                    {
                        List<Employee> emp = new List<Employee>();
                        emp = JsonConvert.DeserializeObject<List<Employee>>(Msg[2]);

                        return emp;
                    }
                    else
                    {
                        Logging.AddLog("查询部门员工失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询员工列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public string AddConTemplete(Templete temp)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    //scoket发送请求信息
                    SocketMessage sm = new SocketMessage(Request.INSERT_CONTRACT_TEMPLATE_REQUEST, temp);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    //scoket接收请求信息
                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.INSERT_CONTRACT_TEMPLATE_SUCCESS.ToString())
                    {
                        Logging.AddLog("添加模板成功!");
                    }
                    else
                    {
                        Logging.AddLog("添加失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("添加模板失败（服务器连接中断）!");
                    return "添加模板";
                }
           // });
            }
        }
        
         /// <summary>
         /// 查询模板信息
         /// </summary>
         /// <returns></returns>
        public List<Templete> QueryContractTemplate()
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_CONTRACT_TEMPLATE_REQUEST);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_CONTRACT_TEMPLATE_SUCCESS.ToString())
                    {
                        List<Templete> list = new List<Templete>();
                        list = JsonConvert.DeserializeObject<List<Templete>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询部模板列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询模板列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

        /// <summary>
        /// 查询特定模板信息
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        public Templete GetContractTemplate(int tempId)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.GET_CONTRACT_TEMPLATE_REQUEST, tempId);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.GET_CONTRACT_TEMPLATE_SUCCESS.ToString())
                    {
                        Templete list = new Templete();
                        list = JsonConvert.DeserializeObject<Templete>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询特定模板列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询特定模板列表失败(服务器连接中断)!");
                    return null;
                }
            }//);
        }

        /// <summary>
        /// 删除模板信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteContractTemplate(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.DELETE_CONTRACT_TEMPLATE_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.DELETE_CONTRACT_TEMPLATE_SUCCESS.ToString())
                    {
                        Logging.AddLog("删除模板成功!");
                    }
                    else
                    {
                        Logging.AddLog("删除模板失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("删除模板失败（服务器连接中断）!");
                    return "服务器连接中断";
                }
            //});
            }
        }
        
        /// <summary>
        /// 修改模板信息
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public string ModifyContractTemplate(Templete temp)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.MODIFY_CONTRACT_TEMPLATE_REQUEST, temp);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.DELETE_CONTRACT_TEMPLATE_SUCCESS.ToString())
                    {
                        Logging.AddLog("修改模板成功!");
                    }
                    else
                    {
                        Logging.AddLog("修改模板失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("修改模板失败（服务器连接中断）!");
                    return "服务器连接中断";
                }
            //});
            }
        }
        
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteDepartment(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.DELETE_DEPARTMENT_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.DELETE_DEPARTMENT_SUCCESS.ToString())
                    {
                        Logging.AddLog("删除部门成功!");
                    }
                    else
                    {
                        Logging.AddLog("删除部门失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("删除部门失败（服务器连接中断）!");
                    return "服务器连接中断";
                }
            //});
            }
        }
        
         /// <summary>
         /// 提交签单
         /// </summary>
         /// <param name="hdj"></param>
         /// <returns></returns>
        public string InsertHDJContract(HDJContract hdj)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.INSERT_HDJCONTRACT_REQUEST, hdj);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.INSERT_HDJCONTRACT_SUCCESS.ToString())
                    {
                        Logging.AddLog("提交签单成功!");
                    }
                    else
                    {
                        Logging.AddLog("提交签单失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("提交签单失败（服务器连接中断）!");
                    return "服务器连接中断";
                }
            //});
            }
        }
         
        /// <summary>
        /// 查询正在审批的方案
        /// </summary>
        /// <returns></returns>
        public List<SHDJContract> QuerySignPend(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_SIGN_PEND_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_SIGN_PEND_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询正在审批方案列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询正在审批方案列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }
        
        /// <summary>
        /// 查询已通过方案列表
        /// </summary>
        /// <returns></returns>
        public List<SHDJContract> QuerySignAgree(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_SIGN_AGREE_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_SIGN_AGREE_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询已通过方案列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询已通过方案列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

        /// <summary>
        /// 查询已拒绝方案列表
        /// </summary>
        /// <returns></returns>
        public List<SHDJContract> QuerySignRefuse(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_SIGN_REFUSE_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_SIGN_REFUSE_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询已拒绝方案列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询已拒绝方案列表失败(服务器连接中断)!");
                    return null;
                }
           //});
            }
        }

        /// <summary>
        /// 查询等待签字的签单列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<SHDJContract> QueryUnsignContract(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_UNSIGN_CONTRACT_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_UNSIGN_CONTRACT_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询待办列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询待办列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }


        
        /// <summary>
        /// 查询已经签字通过的签单列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<SHDJContract> QuerySignedContract(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.QUERY_SIGNED_CONTRACT_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.QUERY_SIGNED_CONTRACT_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询已办列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询已办列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

        /// <summary>
        /// 查询会签单信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<HDJContract> GetHDJContract(string Id)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.GET_HDJCONTRACT_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));
                    string recMsg = null;
                    lock (recivebuffer)
                    {
                        recLength = ClientSocket.Receive(recivebuffer);
                        recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    }
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);
                    
                    if (Msg[0] == Response.GET_HDJCONTRACT_SUCCESS.ToString())
                    {
                        HDJContract list = new HDJContract();
                        list = JsonConvert.DeserializeObject<HDJContract>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("查询会签单信息失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("查询会签单信息失败(服务器连接中断)!");
                    return null;
                }
            });
        }

        ///// <summary>
        ///// 查询会签单信息
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //public HDJContract GetHDJContract(string Id)
        //{
        //        try
        //        {
        //            SocketMessage sm = new SocketMessage(Request.GET_HDJCONTRACT_REQUEST, Id);
        //            ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

        //            recLength = ClientSocket.Receive(recivebuffer);
        //            string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
        //            string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

        //            if (Msg[0] == Response.GET_HDJCONTRACT_SUCCESS.ToString())
        //            {
        //                HDJContract list = new HDJContract();
        //                list = JsonConvert.DeserializeObject<HDJContract>(Msg[2]);

        //                return list;
        //            }
        //            else
        //            {
        //                Logging.AddLog("查询会签单信息失败!");
        //                return null;
        //            }
        //        }
        //        catch
        //        {
        //            Logging.AddLog("查询会签单信息失败(服务器连接中断)!");
        //            return null;
        //        }
        //}
        
        /// <summary>
        /// 签单签字情况
        /// </summary>
        /// <param name="sd"></param>
        /// <returns></returns>
        public string SignDetail(SignatureDetail sd)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.INSERT_SIGN_DETAIL_REQUEST, sd);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.INSERT_SIGN_DETAIL_SUCCESS.ToString())
                    {
                        Logging.AddLog("签单签字成功!");
                    }
                    else
                    {
                        Logging.AddLog("签单签字失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("签单签字失败(服务器连接中断)!");
                    return "服务器连接中断";
                }
            //});
            }
        }
        
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public int InsertEmployee(Employee emp)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.INSERT_EMPLOYEE_REQUEST, emp);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);
                    
                    if (Msg[0] == Response.INSERT_EMPLOYEE_SUCCESS.ToString())
                    {
                        Logging.AddLog("添加人员成功!");
                        return Convert.ToInt32(Msg[2]);
                    }
                    else if (Msg[0] == Response.INSERT_EMPLOYEE_EXIST.ToString())
                    {
                        Logging.AddLog("员工已经存在，无法继续插入");
                        return -2;
                    }
                    else
                    {
                        Logging.AddLog("添加人员失败!");
                        return -1;
                    }
                }
                catch
                {
                    Logging.AddLog("添加人员失败(服务器连接中断)!");
                    return 0;
                }
            //});
            }
        }
        
        /// <summary>
        /// 上传签字图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task UploadPicture(int id, string p)
        {
            await Task.Run(() =>
            {
                SocketMessage sm = new SocketMessage(Request.UPLOAD_PICTURE_REQUEST, id);

                //recLength = ClientSocket.Receive(recivebuffer);
                //string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                //string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                //if (Msg[0] == Response.UPLOAD_PICTURE_SUCCESS.ToString())
                //{
                    Socket sendpicture = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sendpicture.Connect(new IPEndPoint(IP, 6060));
                    FileStream fs = new FileStream(p, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] fssize = new byte[fs.Length];
                    BinaryReader reader = new BinaryReader(fs);
                    reader.Read(fssize, 0, fssize.Length - 1);
                    sendpicture.Send(Encoding.UTF8.GetBytes((sm.Package+"~").PadRight(50,'0')));
                    sendpicture.Send(fssize);
                    fs.Close();
                    sendpicture.Shutdown(System.Net.Sockets.SocketShutdown.Send);
                    sendpicture.Close();
                //}
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task DownLoadPicture(string id, string filepath)
        {
            await Task.Run(() =>
            {
                SocketMessage sm = new SocketMessage(Request.DOWNLOAD_PICTURE_REQUEST, id);
                Socket download = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                download.Connect(new IPEndPoint(IP, 6060));
                download.Send(Encoding.UTF8.GetBytes(sm.Package + "~"));
                FileStream fs = new FileStream(filepath, FileMode.Create);

                int len = 0;
                int size = 0;
                byte[] buffer = new byte[1024];

                //从终端不停的接受数据，然后写入文件里面，只到接受到的数据为0为止，则中断连接
                while ((size = download.Receive(buffer, 0, buffer.Length, SocketFlags.None)) > 0)
                {
                    fs.Write(buffer, 0, size);
                    len += size;
                }

                fs.Flush();
                fs.Close();
            });
        }

        /// <summary>
        /// 修改会签单信息
        /// </summary>
        /// <param name="hdj"></param>
        /// <returns></returns>
        public string ModifyContractTemplate(HDJContract hdj)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.MODIFY_HDJCONTRACT_REQUEST, hdj);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.MODIFY_HDJCONTRACT_SUCCESS.ToString())
                    {
                        Logging.AddLog("重新提交方案成功!");
                    }
                    else
                    {
                        Logging.AddLog("重新提交方案失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("重新提交方案失败(服务器连接中断)!");
                    return "服务器连接中断";
                }
            //});
            }
        }
        
        /// <summary>
        /// 下载会签单信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public async Task DownloadHDJContract(string Id, string filepath)
        {
            await Task.Run(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.DOWNLOAD_HDJCONTRACT_REQUEST, Id);
                    Socket download = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    download.Connect(new IPEndPoint(IP, 6060));
                    download.Send(Encoding.UTF8.GetBytes(sm.Package + "~"));
                    FileStream fs = new FileStream(filepath, FileMode.Create);

                    int len = 0;
                    int size = 0;
                    byte[] buffer = new byte[1024*1024];
                    //从终端不停的接受数据，然后写入文件里面，只到接受到的数据为0为止，则中断连接
                    while ((size = download.Receive(buffer, 0, buffer.Length, SocketFlags.None)) > 0)
                    {
                        fs.Write(buffer, 0, size);
                        len += size;
                    }


                    fs.Flush();
                    fs.Close();
                    download.Close();
                }
                catch
                {
                    
                }
            });
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteEmployee(int Id)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.DELETE_EMPLOYEE_REQUEST, Id);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.DELETE_EMPLOYEE_SUCCESS.ToString())
                    {
                        Logging.AddLog("删除人员成功!");
                    }
                    else
                    {
                        Logging.AddLog("删除人员失败!");
                    }
                    return Msg[0];
                }
                catch
                {
                    Logging.AddLog("删除人员失败（服务器连接中断）!");
                    return "服务器连接中断";
                }
            //});
            }
        }

        /// <summary>
        /// 条件查询已通过的方案
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<SHDJContract> SearchAgreeHDJConstract(Search s)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.SEARCH_AGREE_HDJCONTRACT_REQUEST, s);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.SEARCH_AGREE_HDJCONTRACT_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("条件查询已通过方案列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("条件查询已通过方案列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }
        
        /// <summary>
        /// 条件查询已办列表
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<SHDJContract> SearchSignedHDJConstract(Search s)
        {
            //return Task.Factory.StartNew(() =>
            {
                try
                {
                    SocketMessage sm = new SocketMessage(Request.SEARCH_SIGNED_HDJCONTRACT_REQUEST, s);
                    ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                    recLength = ClientSocket.Receive(recivebuffer);
                    string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                    string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                    if (Msg[0] == Response.SEARCH_SIGNED_HDJCONTRACT_SUCCESS.ToString())
                    {
                        List<SHDJContract> list = new List<SHDJContract>();
                        list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                        return list;
                    }
                    else
                    {
                        Logging.AddLog("条件查询已通过方案列表失败!");
                        return null;
                    }
                }
                catch
                {
                    Logging.AddLog("条件查询已通过方案列表失败(服务器连接中断)!");
                    return null;
                }
            //});
            }
        }

         public String ModifyEmployeePassword(User user)
         {
             //return Task.Factory.StartNew(() =>
             {
                 try
                 {
                     SocketMessage sm = new SocketMessage(Request.MODIFY_EMP_PWD_REQUEST, user);
                     ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                     recLength = ClientSocket.Receive(recivebuffer);
                     string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);

                     string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                     if (Msg[0] == Response.MODIFY_EMP_PWD_SUCCESS.ToString())
                     {
                     }
                     else
                     {
                         Logging.AddLog("重置用户密码失败!");
                     }
                     return Msg[0];
                 }
                 catch
                 {
                     Logging.AddLog("重置用户密码失败(服务器连接中断)!");
                     return "服务器连接中断";
                 }
             //});
             }
         }

         public String ModifyEmployee(Employee employee)
         {
             //return Task.Factory.StartNew(() =>
             {
                 try
                 {
                     SocketMessage sm = new SocketMessage(Request.MODIFY_EMPLOYEE_REQUEST, employee);
                     ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                     recLength = ClientSocket.Receive(recivebuffer);
                     string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);

                     string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                     if (Msg[0] == Response.MODIFY_EMPLOYEE_SUCCESS.ToString())
                     {
                         Logging.AddLog("重置密码成功");
                     }
                     else
                     {
                         Logging.AddLog("重置用户密码失败!");
                     }
                     return Msg[0];
                 }
                 catch
                 {
                     Logging.AddLog("重置用户密码失败(服务器连接中断)!");
                     return "服务器连接中断";
                 }
             //});
             }
         }

         public String ModifyDepartment(Department department)
         {
             //return Task.Factory.StartNew(() =>
             {
                 try
                 {
                     SocketMessage sm = new SocketMessage(Request.MODIFY_DEPARTMENT_REQUEST, department);
                     ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                     recLength = ClientSocket.Receive(recivebuffer);
                     string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);

                     string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                     if (Msg[0] == Response.MODIFY_EMPLOYEE_SUCCESS.ToString())
                     {

                         Logging.AddLog("修改部门成功");
                     }
                     else
                     {
                         Logging.AddLog("修改部门失败!");
                     }
                     return Msg[0];
                 }
                 catch
                 {
                     Logging.AddLog("重置用户密码失败(服务器连接中断)!");
                     return "服务器连接中断";
                 }
             //});
             }
         }


         public String ModifySDepartment(SDepartment department)
         {
             //return Task.Factory.StartNew(() =>
             {
                 try
                 {
                     SocketMessage sm = new SocketMessage(Request.MODIFY_SDEPARTMENT_REQUEST, department);
                     ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                     recLength = ClientSocket.Receive(recivebuffer);
                     string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);

                     string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                     if (Msg[0] == Response.MODIFY_SDEPARTMENT_SUCCESS.ToString())
                     {

                         Logging.AddLog("修改部门成功");
                     }
                     else
                     {
                         Logging.AddLog("修改部门失败!");
                     }
                     return Msg[0];
                 }
                 catch
                 {
                     Logging.AddLog("重置用户密码失败(服务器连接中断)!");
                     return "服务器连接中断";
                 }
                 //});
             }
         }



         public List<SHDJContract> QuerySignAgreeUndownload(int employeeId)
         {
             //return Task.Factory.StartNew(() =>
            // {
                 try
                 {
                     SocketMessage sm = new SocketMessage(Request.QUERY_AGREE_UNDOWN_REQUEST, employeeId);
                     ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

                     recLength = ClientSocket.Receive(recivebuffer);
                     string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
                     string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

                     if (Msg[0] == Response.QUERY_AGREE_UNDOWN_SUCCESS.ToString())
                     {
                         List<SHDJContract> list = new List<SHDJContract>();
                         list = JsonConvert.DeserializeObject<List<SHDJContract>>(Msg[2]);

                         return list;
                     }
                     else
                     {
                         Logging.AddLog("查询当前已通过但是未进行下载的方案列表失败!");
                         return null;
                     }
                 }
                 catch
                 {
                     Logging.AddLog("查询当前已通过但是未进行下载的方案列表失败(服务器连接中断)!");
                     return null;
                 }
            // });
         }

         /// <summary>
         /// 查询出所有
         /// </summary>
         /// <returns></returns>
         //public List<String> QueryDepartmentShortCall()
         //{
         //    try
         //    {
         //        SocketMessage sm = new SocketMessage(Request.QUERY_DEPARTMENT_SHORTCALL_REQUEST);
         //        ClientSocket.Send(Encoding.UTF8.GetBytes(sm.Package));

         //        recLength = ClientSocket.Receive(recivebuffer);
         //        string recMsg = Encoding.UTF8.GetString(recivebuffer, 0, recLength);
         //        string[] Msg = recMsg.Split(SocketMessage.DEFAULT_SEPARATOR);

         //        if (Msg[0] == Response.QUERY_DEPARTMENT_SHORTCALL_SUCCESS.ToString())
         //        {
         //            List<string> list = new List<string>();
         //            list = JsonConvert.DeserializeObject<List<string>>(Msg[2]);

         //            return list;
         //        }
         //        else
         //        {
         //            Logging.AddLog("查询当前已通过但是未进行下载的方案列表失败!");
         //            return null;
         //        }
         //    }
         //    catch
         //    {
         //        Logging.AddLog("查询当前已通过但是未进行下载的方案列表失败(服务器连接中断)!");
         //        return null;
         //    }
         //}


         public void Close()
         {
             this.ClientSocket.Close();
         }

    }
}

