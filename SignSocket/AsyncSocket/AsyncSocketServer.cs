﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

///  SOKCET 
using System.Net.Sockets;  
using System.Net;
using Newtonsoft.Json;
/*
 *  在同步模式中，
 *  在服务器上使用Accept方法接入连接请求，
 *  而在客户端则使用Connect方法来连接服务器。
 *  
 *  相对地，在异步模式下，
 *  服务器可以使用BeginAccept方法和EndAccept方法来完成连接到客户端的任务，
 *  在客户端则通过BeginConnect方法和EndConnect方法来实现与服务器的连接。
 *  
 *  BeginAccept在异步方式下传入的连接尝试，它允许其他动作而不必等待连接建立才继续执行后面程序。
 *  在调用BeginAccept之前，必须使用Listen方法来侦听是否有连接请求，
 *  
 *  BeginAccept的函数原型为
 *  BeginAccept(AsyncCallback AsyncCallback, Ojbect state)
 *  参数：
 *  AsyncCallBack：代表回调函数
 *  state：表示状态信息，必须保证state中包含socket的句柄
 *  
 *  使用BeginAccept的基本流程是：
 *  (1)创建本地终节点，并新建套接字与本地终节点进行绑定；
 *  (2)在端口上侦听是否有新的连接请求；
 *  (3)请求开始接入新的连接，传入Socket的实例或者StateOjbect的实例。
 *  
 *  当BeginAccept()方法调用结束后，一旦新的连接发生，将调用回调函数，
 *  而该回调函数必须包括用来结束接入连接操作的EndAccept()方法。
 *  该方法参数列表为 Socket EndAccept(IAsyncResult iar)
 *  
 *  如何获取连接的客户端的IP和端口
 *  Accpet和BeginAccept获取到来自客户端的连接
 *  然后创建并返回新的 Socket。
 *  不能使用返回的这个 Socket 接受连接队列中的任何附加连接。
 *  然而，可以调用返回的 Socket 的 RemoteEndPoint 方法来标识远程主机的网络地址和端口号
 * 
 * 
 *  最后附上同步还是异步服务器的性能对比
 *  同步:  编程简单,不易出错
 *  异步:  流量较少,性能较高
 *  项目实战中, 一般采用同步, 短连接的形式
 *  如果确实有必要, 才会选用异步, 长连接的形式
 *  
 *  老板要的是 短时间能出成果,稳定的程序, 所以项目经理定方案时, 首选同步短连接
 *  如果频繁建立连接成为性能瓶颈的话, 那就要采用长连接了.
 * 
 *  大量客户端的话用异步加多线程，小量客户端用同步加线程池 
 * 
 */



/*
 * 一下内容摘自http://s.yanghao.org/program/viewdetail.php?i=2945
 * 自己写的客户端马上要发布了，忽然发现了一大堆问题，
 * 主要集中在与服务器的TCP连接经常莫名断开，
 * 客户端又检测不到，不能及时重连。
 * 一个多星期的改，有一些心得，与大家分享。
 * 也希望大家多发表意见，您的意见也许最后就实现在我的软件中了！
 * 
 * 
 * 主要分为两部分：
一，如何更好的检测TCP连接是否正常
二，如何提取本机TCP连接状态

一，如何更好的检测TCP连接是否正常
这方面问题，我上网查了很久，一般来说比较成熟的有两种方法：
1是在应用层制定协议，发心跳包，这也是C#，JAVA等高级语言比较常用的方法。客户端和服务端制定一个通讯协议，每隔一定时间（一般15秒左右），由一方发起，向对方发送协议包；对方收到这个包后，按指定好的通讯协议回一个。若没收到回复，则判断网络出现问题，服务器可及时的断开连接，客户端也可以及时重连。
2通过TCP协议层发送KeepAlive包。这个方法只需设置好你使用的TCP的KeepAlive项就好，其他的操作系统会帮你完成。操作系统会按时发送KeepAlive包，一发现网络异常，马上断开。我就是使用这个方法，也是重点向大家介绍的。

使用第二种方法的好处，是我们在应用层不需自己定协议，通信的两端，只要有一端设好这个值，两边都能及时检测出TCP连接情况。而且这些都是操作系统帮你自动完成的。像我们公司的服务端代码就是早写好的，很难改动。以前也没加入心跳机制，后面要改很麻烦，boss要求检测连接的工作尽量客户端单独完成....
还有一个好处就是节省网络资源。KeepAlive包，只有很简单的一些TCP信息，无论如何也是比你自己设计的心跳包短小的。然后就是它的发送机制，在TCP空闲XXX秒后才开始发送。自己设计心跳机制的话，很难做到这一点。

这种方法也是有些缺陷的。比如某一时刻，网线松了，如果刚好被KeepAlive包检测到，它会马上断开TCP连接。但其实这时候TCP连接也算是established的，只要网线再插好，这个连接还是可以正常工作的。这种情况，大家有什么好办法处理吗？

C#中设置KeepAlive的代码
  uint dummy = 0;
  byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
  BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
  BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
  BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);

  IPEndPoint iep = new IPEndPoint(this._IPadd, xxxx);
  this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
  this._socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
  this._socket.Connect(iep);
这里我设定TCP15秒钟空闲，就开始发送KeepAlive包，其实完全可是设定得长一点。


二，如何提取本机TCP连接状态
设好了KeepAlive值，又遇到麻烦了，我没找到当网络异常时，它断开连接后怎么通知我...我搜了很久都没找到，要是哪位兄弟知道的话告诉我吧。我是使用笨办法的，找到所有本地TCP连接的信息，筛选出我需要的那个TCP。
查看本机所有TCP连接信息，网上一般的方法，都是通过程序调用CMD命令里的netstat进行，然后再分析其内容。但在CMD窗口用过这个命令的都知道，悲剧的时候它显示完所有TCP信息需要15s，或者更长时间，这在我的程序中是不能忍受的。
然后我又查找了一些牛人的博客，发现有人提到用iphlpapi.dll。这是一个在win98以上操作系统目录System32都包含的库函数，功能异常强大，大家可以放心使用！但是使用起来比较麻烦，基本找不到C#现成使用的例子，就算有，也是很老版本的，完全不能用
我参考了这两位高人的博客
http://blog.csdn.net/yulinlover/archive/2009/02/08/3868824.aspx
（另一位的博客连接找不到了..悲剧啊！）
下载了里面提到的项目，仔细结合自己体会进行修改，终于能用了。每隔一段时间，我的客户端就用这个方法扫描一遍本地TCP信息，若发现连接有问题，则断开重连。
这个方法能瞬间得到本机所有TCP连接信息（如果你有兴趣可以扩充，它的功能真的是太强大了），没有CMD命令netstat那不能忍受的延迟，相当好用。代码比较长，就不贴出来了。


这些是我不太成熟的做法，下星期项目就要提交了，不能再出啥岔子，希望大家多提意见，帮我改善一下。
本版人气很旺，但貌似用socket的人不多，不知道帖子发这是否合适。要是不合适，请前辈提点下发在哪个版比较好？
 
 * 
 * 关于Keep-alive
在TCP中有一个Keep-alive的机制可以检测死连接，原理很简单，
TCP会在空闲了一定时间后发送数据给对方：
1.如果主机可达，对方就会响应ACK应答，就认为是存活的。
2.如果可达，但应用程序退出，对方就发RST应答，发送TCP撤消连接。
3.如果可达，但应用程序崩溃，对方就发FIN消息。
4.如果对方主机不响应ack, rst，继续发送直到超时，就撤消连接。这个时间就是默认
的二个小时。
 */


using SignPressServer.SignDAL;
using SignPressServer.SignData;
using SignPressServer.SignTools;
using SignPressServer.SignLog;


namespace SignPressServer.SignSocket.AsyncSocket
{
    /// <summary>
    /// Socket实现的异步TCP服务器
    /// </summary>
    class AsyncSocketServer: IDisposable
    {

        #region 字段信息Fields
        /// <summary>
        /// 服务器的监听端口
        /// </summary>
        private const int  LISENT_PORT = 6666;

        /// <summary>
        /// 服务器程序允许的最大客户端连接数
        /// </summary>
        private int m_maxClientCount;

        /// <summary>
        /// 当前的连接的客户端数
        /// </summary>
        private int m_currClientCount;

        /// <summary>
        /// 服务器使用的异步socket
        /// </summary>
        private Socket m_serverSocket;

        /// <summary>
        /// 客户端会话列表
        /// </summary>
        private List<AsyncSocketState> m_clientList;

        private bool disposed = false;

        #endregion

        #region 属性信息Properties

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }


        #endregion



        #region 扩展信息域
        LogerHelper m_log;
        LogerHelper Log
        {
            get { return this.m_log;}
        }
        #endregion

        #region 构造函数

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        public AsyncSocketServer(int listenPort)
            : this(IPAddress.Any, listenPort,1024)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        public AsyncSocketServer(IPEndPoint localEP)
            : this(localEP.Address, localEP.Port, 1024)
        {
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localIPAddress">监听的IP地址</param>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClientCount">最大客户端数量</param>
        public AsyncSocketServer(IPAddress localIPAddress, int listenPort, int maxClientCount)
        {
            this.Address = localIPAddress;
            this.Port = listenPort;
            this.Encoding = Encoding.Default;

            this.m_maxClientCount = maxClientCount;
            this.m_clientList = new List<AsyncSocketState>();
            this.m_serverSocket = new Socket(localIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                /*
             * 参数
             * ioControlCode
             * 一个 IOControlCode 值，它指定要执行的操作的控制代码。
             * optionInValue
             * Byte 类型的数组，包含操作要求的输入数据。
             * optionOutValue
             * Byte 类型的数组，包含由操作返回的输出数据。
             */

            //this.m_serverSocket.IOControl(IOControlCode.DataToRead, null, BitConverter.GetBytes(0));
            // 创建日志文件[2015-6-16 21:10]
            this.m_log = new LogerHelper();     // 创建一个日志对象，默认设置每天更新一个日志文件
        
        }

        #endregion

        #region 方法-成员函数Method

        /// <summary>
        /// 启动服务器
        /// 服务器所允许的挂起连接序列的最大长度  默认为1024
        /// </summary>
        public void Start( )
        {
            if (!IsRunning)
            {
                this.IsRunning = true;
                this.m_serverSocket.Bind(new IPEndPoint(this.Address, this.Port));  //  绑定服务器和端口
                this.m_serverSocket.Listen(1024);

                /**
                BeginAccept在异步方式下传入的连接尝试，它允许其他动作而不必等待连接建立才继续执行后面程序。在调用BeginAccept之前，必须使用Listen方法来侦听是否有连接请求，BeginAccept的函数原型为：

                BeginAccept(AsyncCallback AsyncCallback, Ojbect state)
                参数:
                AsyncCallBack：代表回调函数
                state：表示状态信息，必须保证state中包含socket的句柄

                使用BeginAccept的基本流程是：
                (1)创建本地终节点，并新建套接字与本地终节点进行绑定；
                (2)在端口上侦听是否有新的连接请求；
                (3)请求开始接入新的连接，传入Socket的实例或者StateOjbect的实例。
                 */
                this.m_serverSocket.BeginAccept(new AsyncCallback(HandleAcceptConnected), this.m_serverSocket);
                /*
                 当BeginAccept()方法调用结束后，一旦新的连接发生，将调用回调函数，
                 而该回调函数必须包括用来结束接入连接操作的EndAccept()方法。
                 该方法参数列表为 Socket EndAccept(IAsyncResult iar)
                 * **/
            }
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="backlog">
        /// 服务器所允许的挂起连接序列的最大长度
        /// </param>
        public void Start(int backlog)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                this.m_serverSocket.Bind(new IPEndPoint(this.Address, this.Port));
                this.m_serverSocket.Listen(backlog);
                this.m_serverSocket.BeginAccept(new AsyncCallback(HandleAcceptConnected), this.m_serverSocket);
            }
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                this.IsRunning = false;
                this.m_serverSocket.Close( );

                //  TODO 关闭对所有客户端的连接
      
            }
        }

        /// <summary>
        /// 处理客户端连接
        /// BeginAccept的回调函数
        /// 当BeginAccept()方法调用结束后，一旦新的连接发生，将调用回调函数，
        /// 而该回调函数必须包括用来结束接入连接操作的EndAccept()方法。
        /// 该方法参数列表为 Socket EndAccept(IAsyncResult iar)
        /// </summary>
        /// <param name="ar"></param>
        private void HandleAcceptConnected(IAsyncResult ar)
        {
            if (IsRunning)
            {
                Socket server = (Socket)ar.AsyncState;
                Socket client = server.EndAccept(ar);   //  EndAccept后，就可以进行正常的通信了
               
                //  检查是否达到最大的允许的客户端数目
                if (this.m_currClientCount >= this.m_maxClientCount)
                {
                    //C-TODO 触发事件
                    RaiseOtherException(null);
                }
                else    // 处理客户端的连接
                {


                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("获取到一个来自{0} : {1}的连接", ip.Address, ip.Port);
                    this.Log.Write(new LogMessage("获取到一个来自" + ip.Address + " : " + ip.Port + "的连接...", LogMessageType.Information));

                    AsyncSocketState state = new AsyncSocketState(client);
                    lock (this.m_clientList)        //  将连接的客户端的信息添加到客户端列表
                    {
                        this.m_clientList.Add(state);
                        this.m_currClientCount++;
                        RaiseClientConnected(state); //  触发客户端连接事件
                    }
                    //  设置与客户端的通信的缓冲区
                    state.RecvDataBuffer = new byte[client.ReceiveBufferSize];
                    
                    ///  开始接受来自该客户端的数据
                    ///
                    /// 接收数据自BeginRecive开始，
                    /// 调用回调函数函数HandleDataReceived
                    /// 在回调函数中以EndReceive结束
                    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                     new AsyncCallback(HandleDataReceived), state);
                }

                //接受下一个请求
                server.BeginAccept(new AsyncCallback(HandleAcceptConnected), ar.AsyncState);
            }
        }


        /// <summary>
        /// 处理客户端数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行数据传输...");
                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行数据传输...", LogMessageType.Information));

                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive

                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }
                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    Console.WriteLine("接收了" + state.RecvLength + "个数据单元");
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength + "个数据单元", LogMessageType.Information));

                    // 处理客户端的数据请求
                    DealClientRequest(state);

                    //C- TODO 触发数据接收事件
                    //RaiseDataReceived(state);
                    /// [服务器异步接收来自客户端的数据]
                    /// 
                    /// 首先是服务器通过BeginReceive开始异步接收客户端的数据
                    /// 在BeginReceive的回调函数HandleDataReceived中，当接收完毕后，服务器调用EndReceive
                    /// 至此异步接收来自服务器的数据完毕
                    Console.WriteLine("前一个处理信息已经结束, 正在准备等待下一个接收");
                    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                     new AsyncCallback(HandleDataReceived), state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    this.Log.Write(new LogMessage(e.ToString(), LogMessageType.Exception));
                    RaiseNetError(state);
                }
                catch (IOException ex)
                {
                    //if (IOException.InnerException is System.Net.Sockets.SocketException)
                    //{
                    //    Console.WriteLine("网络中断");
                    //}
                    //else
                    {
                        Console.WriteLine(ex.Message);

                    }
                }
                finally
                {
                    ///// [服务器异步接收来自客户端的数据]
                    ///// 
                    ///// 首先是服务器通过BeginReceive开始异步接收客户端的数据
                    ///// 在BeginReceive的回调函数HandleDataReceived中，当接收完毕后，服务器调用EndReceive
                    ///// 至此异步接收来自服务器的数据完毕
                    //Console.WriteLine("前一个处理信息已经结束, 正在准备等待下一个接收");
                    //client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                    // new AsyncCallback(HandleDataReceived), state);
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="state">接收数据的客户端会话</param>
        /// <param name="data">数据报文</param>
        public void Send(AsyncSocketState state, byte[] data)
        {
            RaisePrepareSend(state);
            Send(state.ClientSocket, data);
        }

        /// <summary>
        /// 异步发送数据至指定的客户端
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="data">报文</param>
        public void Send(Socket client, byte[] data)
        {
            if (!IsRunning)
                throw new InvalidProgramException("This TCP Scoket server has not been started.");

            if (client == null)
                throw new ArgumentNullException("client");

            if (data == null)
                throw new ArgumentNullException("data");
            client.BeginSend(data, 0, data.Length, SocketFlags.None,
             new AsyncCallback(SendDataEnd), client);
        }

        /// <summary>
        /// 发送数据完成处理函数
        /// </summary>
        /// <param name="ar">目标客户端Socket</param>
        private void SendDataEnd(IAsyncResult ar)
        {
            ((Socket)ar.AsyncState).EndSend(ar);
            RaiseCompletedSend(null);
        }
        #endregion

        #region 事件

        /// <summary>
        /// 与客户端的连接已建立事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> ClientConnected;
        /// <summary>
        /// 与客户端的连接已断开事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> ClientDisconnected;

        /// <summary>
        /// 触发客户端连接事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseClientConnected(AsyncSocketState state)
        {
            if (ClientConnected != null)
            {
                IPEndPoint ip = (IPEndPoint)state.ClientSocket.RemoteEndPoint;
                Console.WriteLine("获取到一个来自" + ip.Address + " : " + ip.Port + "的连接...");
           
                ClientConnected(this, new AsyncSocketEventArgs(state));
            }
        }
        /// <summary>
        /// 触发客户端连接断开事件
        /// </summary>
        /// <param name="client"></param>
        private void RaiseClientDisconnected(Socket client)
        {
            if (ClientDisconnected != null)
            {
                ClientDisconnected(this, new AsyncSocketEventArgs("连接断开"));
            }
        }

        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> DataReceived;

        private void RaiseDataReceived(AsyncSocketState state)
        {
            if (DataReceived != null)
            {
                this.DataReceived(this, new AsyncSocketEventArgs(state));
            }
        }

        /// <summary>
        /// 发送数据前的事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> PrepareSend;

        /// <summary>
        /// 触发发送数据前的事件
        /// </summary>
        /// <param name="state"></param>
        private void RaisePrepareSend(AsyncSocketState state)
        {
            if (PrepareSend != null)
            {
                PrepareSend(this, new AsyncSocketEventArgs(state));
            }
        }

        /// <summary>
        /// 数据发送完毕事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> CompletedSend;
        
        /// <summary>
        /// 触发数据发送完毕的事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseCompletedSend(AsyncSocketState state)
        {
            if (CompletedSend != null)
            {
                CompletedSend(this, new AsyncSocketEventArgs(state));
            }
        }

        /// <summary>
        /// 网络错误事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> NetError;
        /// <summary>
        /// 触发网络错误事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseNetError(AsyncSocketState state)
        {
            if (NetError != null)
            {
                NetError(this, new AsyncSocketEventArgs(state));
            }
        }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<AsyncSocketEventArgs> OtherException;
        /// <summary>
        /// 触发异常事件
        /// </summary>
        /// <param name="state"></param>
        private void RaiseOtherException(AsyncSocketState state, string descrip)
        {
            if (OtherException != null)
            {
                OtherException(this, new AsyncSocketEventArgs(descrip, state));
            }
        }
        private void RaiseOtherException(AsyncSocketState state)
        {
            RaiseOtherException(state, "");
        }
        #endregion

        #region Close
        /// <summary>
        /// 关闭一个与客户端之间的会话
        /// </summary>
        /// <param name="state">需要关闭的客户端会话对象</param>
        public void Close(AsyncSocketState state)
        {
            if (state != null)
            {
                state.Datagram = null;
                state.RecvDataBuffer = null;

                this.m_clientList.Remove(state);
                this.m_currClientCount--;
                //TODO 触发关闭事件
                state.Close();
            }
        }
        /// <summary>
        /// 关闭所有的客户端会话,与所有的客户端连接会断开
        /// </summary>
        public void CloseAllClient()
        {
            foreach (AsyncSocketState client in this.m_clientList)
            {
                Close(client);
            }
            this.m_currClientCount = 0;
            this.m_clientList.Clear();
        }
        #endregion

        #region 释放
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (this.m_serverSocket != null)
                        {
                            this.m_serverSocket = null;
                        }
                    }
                    catch (SocketException)
                    {
                        //TODO
                        RaiseOtherException(null);
                    }
                }
                disposed = true;
            }
        }
        #endregion

        /// 前面我们提到了服务器异步接收客户端请求的过程
        /// 
        /// 首先是服务器通过BeginReceive开始异步接收客户端的数据
        /// 在BeginReceive的回调函数HandleDataReceived中，当接收完毕后，服务器调用EndReceive
        /// 至此异步接收来自服务器的数据完毕
        /// 
        /// 而我们客户端默认发送数据是先发一个信息请求头，再发请求数据
        /// 因此我们可以这样处理，在BeginReceive回调函数中，异步接收[信息请求头]完毕后，
        /// 枚举信息头，来进行下面的处理，接收进一步的请求数据
        /// 
        /// 因此每增加一项客户端和服务器的通信过程，就可以这样实现，
        /// 在DealClientRequest增加一个枚举头（信息请求头），然后开始异步接收进一步的信息数据
        /// 在其回调函数中接收我们的信息，并将信息进行处理后，将事件处理函数中反馈信息发送至客户端
        /// 
        ///
        /// 例如处理客户端的登录请求
        /// 对应客户端会先发登录请求头LOGIN_REQUEST，然后接着发登录信息User
        /// 在DealClientRequest中增加相应的信息头，然后服务器BeginReceive异步接收登录信息User，
        /// 设置的接收登录信息User的回调函数HandleLoginRequestDataReceived等待接收信息完毕后
        /// 在函数RaiseLoginRequestEvent事件处理函数中处理，进行查询数据库完毕后，将数据发送至客户端
        #region 处理客户端的所有请求
        /// <summary>
        /// 处理客户端的所有请求
        /// </summary>
        /// <param name="ar"></param>
        public void DealClientRequest(AsyncSocketState state)
        {
            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);

            Console.WriteLine("接收到的数据{0}, 大小{1}", recvMsg, recvMsg.Length);
            this.Log.Write(new LogMessage("接收到的数据 " + recvMsg + ", 大小" + recvMsg.Length.ToString(), LogMessageType.Information));
            
            switch (recvMsg)
            { 
                /// <summary>
                /// ==用户操作==
                /// 用户登录  LOGIN_REQUEST
                /// 用户退出  QUIT_REQUEST
                /// </summary>
                case "LOGIN_REQUEST" :       //  用户登录信息
                    // 开始接收用户登录的用户名和密码
                    //在[回调函数]HandleLoginRequestDataReceived中接收完成后, 进行登录验证
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleLoginRequestDataReceived), state);
                    break;
                case "QUIT_REQUEST"  :
                    ///
                    ///   代码未实现
                    ///
                    break;

                /// <summary>
                /// ==部门操作==
                /// 增加部门  INSERT_DEPARTMENT_REQUEST
                /// 删除部门  DELETE_DEPARTMENT_REQUEST
                /// 修改部门  MODIFY_DEPARTMENT_REQUEST
                /// 查询部门  QUERY_DEPARTMENT_REQUEST
                /// </summary>
                #region 部门操作
                case "INSERT_DEPARTMENT_REQUEST" :  //  添加部门请求
                    // 开始接收期望添加进入数据的库的部门的信息
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleInsertDepartmentRequestDataReceived), state);
                    break;

                case "DELETE_DEPARTMENT_REQUEST" :
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleDeleteDepartmentRequestDataReceived), state);
                    break;

                case "MODIFY_DEPARTMENT_REQUEST" :
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleModifyDepartmentRequestDataReceived), state);
                    break;

                case "QUERY_DEPARTMENT_REQUEST"  :
                    // 注意查询部门的时候，客户端只需要发送请求信息头就可以了
                    /*state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleQueryDepartmentRequestDataReceived), state);*/
                    RaiseQueryDepartmentRequestEvent(state);
                    break;
                #endregion  部门操作

                /// <summary>
                /// ==员工操作==
                /// 增加员工  INSERT_EMPLOYEE_REQUEST
                /// 删除员工  DELETE_EMPLOYEE_REQUEST
                /// 修改员工  MODIFY_EMPLOYEE_REQUEST
                /// 查询员工  QUERY_ERMPLOYEE_REQUEST
                /// </summary>
                #region 员工操作
                
                case "INSERT_EMPLOYEE_REQUEST" :
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleInsertEmployeeRequestDataReceived), state);
                    break;
                case "DELETE_EMPLOYEE_REQUEST" :
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleDeleteEmployeeRequestDataReceived), state);
                    break;
                case "MODIFY_EMPLOYEE_REQUEST": 
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleModifyEmployeeRequestDataReceived), state);
                    break;
                case "QUERY_EMPLOYEE_REQUEST":
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                        new AsyncCallback(HandleQueryEmployeeRequestDataReceived), state);
                    break;
                #endregion

                /// <summary>
                /// ==会签单模版操作==
                /// 增加会签单模版  INSERT_CONTRACT_TEMPLATE_REQUEST
                /// 删除会签单模版  DELETE_CONTRACT_TEMPLATE_REQUEST
                /// 修改会签单模版  MODIFY_CONTRACT_TEMPLATE_REQUEST
                /// 查询会签单模版  QUERY_CONTRACT_TEMPLATE_REQUEST
                /// </summary>
                case "INSERT_CONTRACT_TEMPLATE_REQUEST" :
                    break;
                case "DELETE_CONTRACT_TEMPLATE_REQUEST" :
                    break;
                case "MODIFY_CONTRACT_TEMPLATE_REQUEST" :
                    break;
                case "QUERY_CONTRACT_TEMPLATE_REQUEST"  :
                    break;
            }
        }
        #endregion

        

        #region 处理客户端的登录请求
        /// <summary>
        /// 处理客户端登录请求数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleLoginRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {


                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行登录验证...");
                    Console.WriteLine("接收了{0}个数据单元[登录信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行登录验证...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" +  state.RecvLength.ToString( ) +"个数据单元[登录信息]", LogMessageType.Information));
                    
                    //C- TODO 触发用户登录的事件
                    RaiseLoginRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    this.Log.Write(new LogMessage(e.ToString(), LogMessageType.Exception));

                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户登录请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseLoginRequestEvent(AsyncSocketState state)
        {
            
            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string LOGIN_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            User user = JsonConvert.DeserializeObject<User>(recvMsg);
            Employee employee = new Employee();
            employee = DALEmployee.LoginEmployee(user);        //  如果用户名和密码验证成功
            if(employee.Id != -1)
            {
                Console.WriteLine(user + "用户名密码均正确，可以登录");
                this.Log.Write(new LogMessage(user + "用户名密码均正确，可以登录", LogMessageType.Success));
                response = ServerResponse.LOGIN_SUCCESS;               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine(user + "用户名密码验证失败，无法正常登录");
                this.Log.Write(new LogMessage(user + "用户名密码验证失败，无法正常登录", LogMessageType.Error));

                response = ServerResponse.LOGIN_FAILED;               //  用户登录成功信号   
            }
            
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            if (response.Equals(ServerResponse.LOGIN_SUCCESS))
            {
                String json = JsonConvert.SerializeObject(employee);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion

        #region 部门信息的处理（多个处理段[插入-删除-修改-查询]）


        #region 处理客户端的插入部门请求
        /// <summary>
        /// 处理客户端登录请求数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleInsertDepartmentRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {


                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行插入部门...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行插入部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength + "个数据单元[部门信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseInsertDepartmentRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户登录请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseInsertDepartmentRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string INSERT_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse(); 

            // json数据解包
            String departmentName = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.InsertDepartment(departmentName);
            if (result == true)
            {
                Console.WriteLine("部门{0}插入成功", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "插入成功", LogMessageType.Success));
                
                //INSERT_DEPARTMENT_RESPONSE = "INSERT_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.INSERT_DEPARTMENT_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门{0}插入失败", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "插入失败", LogMessageType.Error));
                
                //INSERT_DEPARTMENT_RESPONSE = "INSERT_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.INSERT_DEPARTMENT_FAILED;
            }

            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
        }

        #endregion


        #region 处理客户端的删除部门请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDeleteDepartmentRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {


                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行删除部门...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行删除部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
                        //C- TODO 触发用户登录的事件
                    RaiseDeleteDepartmentRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户删除部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseDeleteDepartmentRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string DELETE_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse(); 

            // json数据解包
            String departmentName = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.DeleteDepartment(departmentName);

            if (result == true)
            {
                Console.WriteLine("部门{0}删除成功", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "删除成功", LogMessageType.Success));

                response = ServerResponse.DELETE_DEPARTMENT_SUCCESS;               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "删除失败", LogMessageType.Error));

                //DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.DELETE_DEPARTMENT_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        #region 处理客户端的修改部门请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleModifyDepartmentRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行修改部门...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行修改部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseModifyDepartmentRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户修改部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseModifyDepartmentRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string MODIFY_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            Department department = JsonConvert.DeserializeObject<Department>(recvMsg);
            bool result = DALDepartment.ModifyDepartment(department);

            if (result == true)
            {
                Console.WriteLine("部门{0}, {1}修改成功", department.Id, department.Name);
                this.Log.Write(new LogMessage("部门" + department.Id + ", " + department.Name + "修改成功", LogMessageType.Success));

                //MODIFY_DEPARTMENT_RESPONSE = "MODIFY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.MODIFY_DEPARTMENT_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门{0}, {1}修改失败", department.Id, department.Name);
                this.Log.Write(new LogMessage("部门" + department.Id + ", " + department.Name + "修改失败", LogMessageType.Error));

                //MODIFY_DEPARTMENT_RESPONSE = "MODIFY_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.MODIFY_DEPARTMENT_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        #region 处理客户端的查询部门请求
        /// <summary>
        /// 处理客户端的查询部门请求
        /// </summary>
        /// <param name="state"></param>
        private void RaiseQueryDepartmentRequestEvent(AsyncSocketState state)
        {
            List<Department> departments = new List<Department>();
            //String QUERY_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse();

            // 向数据库中查询部门的信息
            departments = DALDepartment.QueryDepartment();
            if (departments != null)
            {
                Console.WriteLine("部门信息查询成功");
                this.Log.Write(new LogMessage(state.ClientIp + "部门信息查询成功", LogMessageType.Success));
                
                //QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.QUERY_DEPARTMENT_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门信息查询失败");
                this.Log.Write(new LogMessage(state.ClientIp + "部门信息查询失败", LogMessageType.Error));

                //QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.QUERY_DEPARTMENT_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            if (response.Equals(ServerResponse.QUERY_DEPARTMENT_SUCCESS))
            {
                String json = JsonConvert.SerializeObject(departments);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion
        #endregion // 部门信息的处理（多个处理段[插入-删除-修改-查询]）

        #region 员工信息的处理(多个处理端[插入-删除-修改-查询])


        #region 处理客户端的插入人员请求
        /// <summary>
        /// 处理客户端插入员工请求数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleInsertEmployeeRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {


                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行插入员工...");
                    Console.WriteLine("接收了{0}个数据单元[员工信息]", state.RecvLength);

                    this.Log.Write(new LogMessage(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行插入员工...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength + "个数据单元[员工信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseInsertEmployeeRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 插入员工请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseInsertEmployeeRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string INSERT_EMPLOYEE_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            Employee employee = JsonConvert.DeserializeObject<Employee>(recvMsg);
            bool result = DALEmployee.InsertEmployee(employee);
            if (result == true)
            {
                Console.WriteLine("员工{0}插入成功", employee.Name);
                this.Log.Write(new LogMessage("员工" + employee.Name + "插入成功", LogMessageType.Success));

                response = ServerResponse.INSERT_EMPLOYEE_SUCCESS;               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("员工{0}插入失败", employee.Name);
                this.Log.Write(new LogMessage("员工" + employee.Name + "插入失败", LogMessageType.Error));

                //INSERT_EMPLOYEE_RESPONSE = "INSERT_EMPLOYEE_FAILED";                //  用户登录失败信号
                response = ServerResponse.INSERT_EMPLOYEE_FAILED;
            }

            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端

        }

        #endregion


        #region 处理客户端的删除员工请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDeleteEmployeeRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseDeleteEmployeeRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户删除部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseDeleteEmployeeRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string DELETE_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            String employeeId = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.DeleteDepartment(employeeId);

            if (result == true)
            {
                Console.WriteLine("员工{0}删除成功", employeeId);
                this.Log.Write(new LogMessage(state.ClientIp + "员工" + employeeId + "删除成功", LogMessageType.Success));

                //DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.DELETE_DEPARTMENT_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", employeeId);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employeeId + "删除失败", LogMessageType.Error));

                //DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.DELETE_DEPARTMENT_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        #region 处理客户端的修改员工请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleModifyEmployeeRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行修改部门...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行修改部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseModifyEmployeeRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户修改部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseModifyEmployeeRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            //string MODIFY_EMPLOYEE_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            Employee employee = JsonConvert.DeserializeObject<Employee>(recvMsg);
            bool result = DALEmployee.ModifyEmployee(employee);

            if (result == true)
            {
                Console.WriteLine(state.ClientIp + "部门{0}, {1}修改成功", employee.Id, employee.Name);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employee.Id + ", " + employee.Name + "修改成功", LogMessageType.Success));

                //MODIFY_EMPLOYEE_RESPONSE = "MODIFY_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.MODIFY_EMPLOYEE_SUCCESS;
            }
            else
            {
                Console.WriteLine(state.ClientIp + "部门{0}删除失败", employee.Name);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employee.Name + "修改失败", LogMessageType.Error));

                //MODIFY_EMPLOYEE_RESPONSE = "MODIFY_EMPLOYEE_FAILED";                //  用户登录失败信号
                response = ServerResponse.MODIFY_EMPLOYEE_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        //#region 处理客户端的查询员工请求
        ///// <summary>
        ///// 处理客户端的查询部门请求
        ///// </summary>
        ///// <param name="state"></param>
        //private void RaiseQueryEmployeeRequestEvent(AsyncSocketState state)
        //{
        //    List<Department> departments = new List<Department>();
        //    String QUERY_DEPARTMENT_RESPONSE;

        //    // 向数据库中查询部门的信息
        //    departments = DALDepartment.QueryDepartment();
        //    if (departments != null)
        //    {
        //        Console.WriteLine(state.ClientIp + "员工信息查询成功");
        //        this.Log.Write(new LogMessage(state.ClientIp + "部门信息查询成功", LogMessageType.Success));

        //        QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
        //    }
        //    else
        //    {
        //        Console.WriteLine(state.ClientIp + "员工信息查询失败");
        //        this.Log.Write(new LogMessage(state.ClientIp + "部门信息查询失败", LogMessageType.Error));

        //        QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_FAILED";                //  用户登录失败信号
        //    }
        //    // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
        //    //  响应信息头LOGIN_RESPONSE
        //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(QUERY_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
        //    if (QUERY_DEPARTMENT_RESPONSE.Equals("QUERY_DEPARTMENT_SUCCESS"))
        //    {
        //        String json = JsonConvert.SerializeObject(departments);
        //        this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
        //    }
        //}
        //
        //#endregion


        #region 查询部门员工的信息请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleQueryEmployeeRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行查询部门的员工信息...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门的编号信息]", state.RecvLength);

                    this.Log.Write(new LogMessage(state.ClientIp + "正在与" + ip.Address + " : " + ip.Port + "进行查询部门的员工信息...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门的编号信息]", LogMessageType.Information));
                    
                    //C- TODO 触发用户登录的事件
                    RaiseQueryEmployeeRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 处理客户端的查询员工请求
        /// </summary>
        /// <param name="state"></param>
        private void RaiseQueryEmployeeRequestEvent(AsyncSocketState state)
        {
            String recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            
            Console.WriteLine(recvMsg);

            Console.WriteLine("客户端{0}待查询的部门的ID = {1}", state.ClientIp, recvMsg);
            this.Log.Write(new LogMessage(state.ClientIp + "接收到的数据" + recvMsg, LogMessageType.Information));
            this.Log.Write(new LogMessage(state.ClientIp + "待查询的员工的ID = " + recvMsg, LogMessageType.Information));
            
            
            //String QUERY_EMPLOYEE_RESPONSE;
            ServerResponse response = new ServerResponse();

            List<Employee> employees = DALEmployee.QueryEmployee(int.Parse(recvMsg));
            // List<Employee> employees = DALEmployee.QueryEmployee(int.Parse(recvMsg));

            this.Log.Write(new LogMessage(state.ClientIp + "查询到的部门编号为" + recvMsg + "的所有员工", LogMessageType.Information));

            if (employees != null)
            {
                Console.WriteLine("部门{0}的员工信息查询成功", recvMsg);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + recvMsg + "的员工信息查询成功", LogMessageType.Success));

                //QUERY_EMPLOYEE_RESPONSE = "QUERY_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.QUERY_EMPLOYEE_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门{0}的员工信息查询失败", recvMsg);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + recvMsg+ "的员工信息查询失败", LogMessageType.Error));

                //QUERY_EMPLOYEE_RESPONSE = "QUERY_EMPLOYEE_FAILED";                //  用户登录失败信号
                response = ServerResponse.QUERY_EMPLOYEE_FAILED;
            }

            //  先发响应头
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            Console.WriteLine("发送的头信息" + response.ToString());
            
            // 再发查询到的员工信息
            if (response.Equals(ServerResponse.QUERY_EMPLOYEE_SUCCESS))
            {
                Console.WriteLine("发送查询到的员工信息");
                String json = JsonConvert.SerializeObject(employees);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion
        #endregion // 员工信息的处理(多个处理端[插入-删除-修改-查询])

        #region 会签单模版的处理(多个处理段[插入-删除-修改-查询])
        
        
        #region 处理客户端的插入会签单的请求
        /// <summary>
        /// 处理客户端插入员工请求数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleInsertContractTemplateRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行插入员工...");
                    Console.WriteLine("接收了{0}个数据单元[会签单模版信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行插入员工...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength + "个数据单元[会签单模版信息]", LogMessageType.Information));
                    //C- TODO 触发用户登录的事件
                    RaiseInsertContractTemplateRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
            }
        }

        /// <summary>
        /// 插入员工请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseInsertContractTemplateRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine("接收到的会签单模版的信息" + recvMsg);
            this.Log.Write(new LogMessage("接收到的会签单模版的信息" + recvMsg, LogMessageType.Information));

            ServerResponse response = new ServerResponse();


            // json数据解包
            ContractTemplate conTemp = JsonConvert.DeserializeObject<ContractTemplate>(recvMsg);
            bool result = DALContractTemplate.InsertContractTemplate(conTemp);
            
            if (result == true)
            {
                Console.WriteLine("会签单{0}插入成功", conTemp.Name);
                this.Log.Write(new LogMessage("会签单" + conTemp.Name + "插入成功", LogMessageType.Success));
                
                //  用户登录成功信号
                response = ServerResponse.INSERT_CONTRACT_TEMPLATE_SUCCESS;
                   
            }
            else
            {
                Console.WriteLine("会签单{0}插入失败", conTemp.Name);
                this.Log.Write(new LogMessage("会签单" + conTemp.Name + "插入失败", LogMessageType.Error));

                response = ServerResponse.INSERT_CONTRACT_TEMPLATE_FAILED;                //  用户登录失败信号
            }

            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端

        }

        #endregion


        #region 处理客户端的删除会签单的请求
        /// <summary>
        /// 处理客户端的删除会签单模版请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDeleteContractTemplateRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行删除会签单...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[会签单信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行删除会签单...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[会签单信息]", LogMessageType.Information));
                    
                    //C- TODO 触发用户登录的事件
                    RaiseDeleteContractTemplateRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    this.Log.Write(new LogMessage("删除会签单信息时异常\n异常信息\n" + e.ToString(), LogMessageType.Exception));
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 用户删除部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseDeleteContractTemplateRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            this.Log.Write(new LogMessage("接收到的模版的名称或者编号" + recvMsg, LogMessageType.Information));
            //string DELETE_DEPARTMENT_RESPONSE;
            ServerResponse response = new ServerResponse();

            // json数据解包
            /////////////////////////////////////////////////////////////////////
            String employeeId = JsonConvert.DeserializeObject<String>(recvMsg);//
            bool result = DALDepartment.DeleteDepartment(employeeId);////////////
            /////////////////////////////////////////////////////////////////////
           
            if (result == true)
            {
                Console.WriteLine("员工{0}删除成功", employeeId);
                this.Log.Write(new LogMessage(state.ClientIp + "员工" + employeeId + "删除成功", LogMessageType.Success));

                //DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
                response = ServerResponse.DELETE_CONTRACT_TEMPLATE_SUCCESS;
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", employeeId);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employeeId + "删除失败", LogMessageType.Error));

                //DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_FAILED";                //  用户登录失败信号
                response = ServerResponse.DELETE_CONTRACT_TEMPLATE_FAILED;
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(response.ToString()));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        #region 处理客户端的修改员工请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleModifyContractTemplateRequestDataReceived(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行修改会签单模版...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[会签单模版信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行修改会签单模版信息...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[会签单模版信息]", LogMessageType.Information));
                    
                    //C- TODO 触发用户登录的事件
                    RaiseModifyContractTemplateRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
            }
        }

        /// <summary>
        /// 用户修改部门请求的事件的具体信息
        /// </summary>
        /// <param name="state"></param>
        private void RaiseModifyContractTemplateRequestEvent(AsyncSocketState state)
        {

            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            Console.WriteLine(recvMsg);
            string MODIFY_EMPLOYEE_RESPONSE;


            // json数据解包
            Employee employee = JsonConvert.DeserializeObject<Employee>(recvMsg);
            bool result = DALEmployee.ModifyEmployee(employee);

            if (result == true)
            {
                Console.WriteLine(state.ClientIp + "部门{0}, {1}修改成功", employee.Id, employee.Name);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employee.Id + ", " + employee.Name + "修改成功", LogMessageType.Success));

                MODIFY_EMPLOYEE_RESPONSE = "MODIFY_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine(state.ClientIp + "部门{0}删除失败", employee.Name);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + employee.Name + "修改失败", LogMessageType.Error));

                MODIFY_EMPLOYEE_RESPONSE = "MODIFY_EMPLOYEE_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(MODIFY_EMPLOYEE_RESPONSE));      //  将响应信号发送至客户端
            //if (INSERT_DEPARTMENT_RESPONSE.Equals("INSERT_DEPARTMENT_SUCCESS"))
            //{
            //    String json = JsonConvert.SerializeObject(employee);
            //    this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            //}
        }

        #endregion


        #region 查询部门员工的信息请求
        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleQueryContractTemplateRequestEvent(IAsyncResult ar)
        {
            if (IsRunning)
            {
                AsyncSocketState state = (AsyncSocketState)ar.AsyncState;
                Socket client = state.ClientSocket;
                try
                {
                    state.RecvLength = client.EndReceive(ar);   //  异步接收数据结束
                    if (state.RecvLength == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }

                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    IPEndPoint ip = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行查询部门的员工信息...");
                    Console.WriteLine(state.ClientIp + "接收了{0}个数据单元[部门的编号信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行查询部门的员工信息...", LogMessageType.Information));
                    this.Log.Write(new LogMessage(state.ClientIp + "接收了" + state.RecvLength.ToString() + "个数据单元[部门的编号信息]", LogMessageType.Information));

                    //C- TODO 触发用户登录的事件
                    RaiseQueryContractTemplateRequestEvent(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                //finally
                //{
                //    //  继续接收来自来客户端的数据
                //    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                //     new AsyncCallback(HandleDataReceived), state);
                //}
            }
        }

        /// <summary>
        /// 处理客户端的查询员工请求
        /// </summary>
        /// <param name="state"></param>
        private void RaiseQueryContractTemplateRequestEvent(AsyncSocketState state)
        {
            String recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);

            Console.WriteLine(recvMsg);
            this.Log.Write(new LogMessage(state.ClientIp + "接收到的数据" + recvMsg + "来自", LogMessageType.Information));
            this.Log.Write(new LogMessage(state.ClientIp + "待查询的会签单的编号ID = " + recvMsg, LogMessageType.Information));
            String QUERY_EMPLOYEE_RESPONSE;


            List<Employee> employees = DALEmployee.QueryEmployee(int.Parse(recvMsg));
            // List<Employee> employees = DALEmployee.QueryEmployee(int.Parse(recvMsg));

            this.Log.Write(new LogMessage(state.ClientIp + "查询到的部门编号为" + recvMsg + "的所有员工", LogMessageType.Information));

            if (employees != null)
            {
                Console.WriteLine("部门{0}的员工信息查询成功", recvMsg);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + recvMsg + "的员工信息查询成功", LogMessageType.Success));

                QUERY_EMPLOYEE_RESPONSE = "QUERY_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}的员工信息查询失败", recvMsg);
                this.Log.Write(new LogMessage(state.ClientIp + "部门" + recvMsg + "的员工信息查询失败", LogMessageType.Error));

                QUERY_EMPLOYEE_RESPONSE = "QUERY_EMPLOYEE_FAILED";                //  用户登录失败信号
            }

            //  先发响应头
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(QUERY_EMPLOYEE_RESPONSE));      //  将响应信号发送至客户端
            // 再发查询到的员工信息
            if (QUERY_EMPLOYEE_RESPONSE.Equals("QUERY_EMPLOYEE_SUCCESS"))
            {
                String json = JsonConvert.SerializeObject(employees);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion
        #endregion  //  处理会签单模版的信息 


    }
}

