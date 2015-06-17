using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
                    this.Log.Write(new LogMessage(e.ToString( ), LogMessageType.Exception));
                    RaiseNetError(state);
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

                /// <summary>
                /// ==员工操作==
                /// 增加员工  INSERT_EMPLOYEE_REQUEST
                /// 删除员工  DELETE_EMPLOYEE_REQUEST
                /// 修改员工  MODIFY_EMPLOYEE_REQUEST
                /// 查询员工  QUERY_ERMPLOYEE_REQUEST
                /// </summary>
                case "INSERT_EMPLOYEE_REQUEST" :
                    break;
                case "DELETE_EMPLOYEE_REQUEST" :
                    break;
                case "MODIFY_EMPLOYEE_REQUEST" :
                    break;
                case "QUERY_ERMPLOYEE_REQUEST" :
                    break;
                case "QUERY_DEPARTMENT_EMPLOYEE_REQUEST" :
                    // 查询部门员工的信息头
                    state.ClientSocket.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
    new AsyncCallback(HandleQueryDepartmentEmployeeRequestEvent), state);
                    //RaiseQueryDepartmentEmployeeRequestEvent(state);
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
            string LOGIN_RESPONSE;


            // json数据解包
            User user = JsonConvert.DeserializeObject<User>(recvMsg);
            Employee employee = new Employee();
            employee = DALEmployee.LoginEmployee(user);        //  如果用户名和密码验证成功
            if(employee.Id != -1)
            {
                Console.WriteLine(user + "用户名密码均正确，可以登录");
                this.Log.Write(new LogMessage(user + "用户名密码均正确，可以登录", LogMessageType.Success));
                LOGIN_RESPONSE = "LOGIN_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine(user + "用户名密码验证失败，无法正常登录");
                this.Log.Write(new LogMessage(user + "用户名密码验证失败，无法正常登录", LogMessageType.Error));

                LOGIN_RESPONSE = "LOGIN_FAILED";                //  用户登录失败信号
            }
            
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(LOGIN_RESPONSE));      //  将响应信号发送至客户端
            if(LOGIN_RESPONSE.Equals("LOGIN_SUCCESS"))
            {
                String json = JsonConvert.SerializeObject(employee);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion


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
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行插入部门...");
                    Console.WriteLine("接收了{0}个数据单元[部门信息]", state.RecvLength);
                    
                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行插入部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength + "个数据单元[部门信息]", LogMessageType.Information));
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
            string INSERT_DEPARTMENT_RESPONSE;


            // json数据解包
            String departmentName = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.InsertDepartment(departmentName);
            if (result == true)
            {
                Console.WriteLine("部门{0}插入成功", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "插入成功", LogMessageType.Success));
                
                INSERT_DEPARTMENT_RESPONSE = "INSERT_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}插入失败", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "插入失败", LogMessageType.Error));
                
                INSERT_DEPARTMENT_RESPONSE = "INSERT_DEPARTMENT_FAILED";                //  用户登录失败信号
            }

            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(INSERT_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端

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
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...");
                    Console.WriteLine("接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength.ToString( ) + "个数据单元[部门信息]", LogMessageType.Information));
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
            string DELETE_DEPARTMENT_RESPONSE;


            // json数据解包
            String departmentName = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.DeleteDepartment(departmentName);

            if (result == true)
            {
                Console.WriteLine("部门{0}删除成功", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "删除成功", LogMessageType.Success));

                DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", departmentName);
                this.Log.Write(new LogMessage("部门" + departmentName + "删除失败", LogMessageType.Error));

                DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(DELETE_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
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
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行修改部门...");
                    Console.WriteLine("接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行修改部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
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
            string MODIFY_DEPARTMENT_RESPONSE;


            // json数据解包
            Department department = JsonConvert.DeserializeObject<Department>(recvMsg);
            bool result = DALDepartment.ModifyDepartment(department);

            if (result == true)
            {
                Console.WriteLine("部门{0}, {1}修改成功", department.Id, department.Name);
                this.Log.Write(new LogMessage("部门" + department.Id + ", " + department.Name + "修改成功", LogMessageType.Success));

                MODIFY_DEPARTMENT_RESPONSE = "MODIFY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}, {1}修改失败", department.Id, department.Name);
                this.Log.Write(new LogMessage("部门" + department.Id + ", " + department.Name + "修改失败", LogMessageType.Error));

                MODIFY_DEPARTMENT_RESPONSE = "MODIFY_DEPARTMENT_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(MODIFY_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
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
            String QUERY_DEPARTMENT_RESPONSE;

            // 向数据库中查询部门的信息
            departments = DALDepartment.QueryDepartment();
            if (departments != null)
            {
                Console.WriteLine("部门信息查询成功");
                this.Log.Write(new LogMessage("部门信息查询成功", LogMessageType.Success));
                
                QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门信息查询失败");
                this.Log.Write(new LogMessage("部门信息查询失败", LogMessageType.Error));

                QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(QUERY_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
            if (QUERY_DEPARTMENT_RESPONSE.Equals("QUERY_DEPARTMENT_SUCCESS"))
            {
                String json = JsonConvert.SerializeObject(departments);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion


        #region 处理客户端的插入人员请求
        /// <summary>
        /// 处理客户端插入员工请求数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleInsertEmployRequestDataReceived(IAsyncResult ar)
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

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行插入员工...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength + "个数据单元[员工信息]", LogMessageType.Information));
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
            string INSERT_EMPLOYEE_RESPONSE;


            // json数据解包
            Employee employee = JsonConvert.DeserializeObject<Employee>(recvMsg);
            bool result = DALEmployee.InsertEmployee(employee);
            if (result == true)
            {
                Console.WriteLine("员工{0}插入成功", employee.Name);
                this.Log.Write(new LogMessage("员工" + employee.Name + "插入成功", LogMessageType.Success));

                INSERT_EMPLOYEE_RESPONSE = "INSERT_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("员工{0}插入失败", employee.Name);
                this.Log.Write(new LogMessage("员工" + employee.Name + "插入失败", LogMessageType.Error));

                INSERT_EMPLOYEE_RESPONSE = "INSERT_EMPLOYEE_FAILED";                //  用户登录失败信号
            }

            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(INSERT_EMPLOYEE_RESPONSE));      //  将响应信号发送至客户端

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
                    Console.WriteLine("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...");
                    Console.WriteLine("接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行删除部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
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
            string DELETE_DEPARTMENT_RESPONSE;


            // json数据解包
            String employeeId = JsonConvert.DeserializeObject<String>(recvMsg);
            bool result = DALDepartment.DeleteDepartment(employeeId);

            if (result == true)
            {
                Console.WriteLine("员工{0}删除成功", employeeId);
                this.Log.Write(new LogMessage("员工" + employeeId + "删除成功", LogMessageType.Success));

                DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", employeeId);
                this.Log.Write(new LogMessage("部门" + employeeId + "删除失败", LogMessageType.Error));

                DELETE_DEPARTMENT_RESPONSE = "DELETE_DEPARTMENT_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(DELETE_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
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
                    Console.WriteLine("接收了{0}个数据单元[部门信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行修改部门...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength.ToString() + "个数据单元[部门信息]", LogMessageType.Information));
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
            string MODIFY_EMPLOYEE_RESPONSE;


            // json数据解包
            Employee employee = JsonConvert.DeserializeObject<Employee>(recvMsg);
            bool result = DALEmployee.ModifyEmployee(employee);

            if (result == true)
            {
                Console.WriteLine("部门{0}, {1}修改成功", employee.Id, employee.Name);
                this.Log.Write(new LogMessage("部门" + employee.Id + ", " + employee.Name + "修改成功", LogMessageType.Success));

                MODIFY_EMPLOYEE_RESPONSE = "MODIFY_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}删除失败", employee.Name);
                this.Log.Write(new LogMessage("部门" + employee.Name + "修改失败", LogMessageType.Error));

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


        #region 处理客户端的查询部门请求
        /// <summary>
        /// 处理客户端的查询部门请求
        /// </summary>
        /// <param name="state"></param>
        private void RaiseQueryEmployeeRequestEvent(AsyncSocketState state)
        {
            List<Department> departments = new List<Department>();
            String QUERY_DEPARTMENT_RESPONSE;

            // 向数据库中查询部门的信息
            departments = DALDepartment.QueryDepartment();
            if (departments != null)
            {
                Console.WriteLine("部门信息查询成功");
                this.Log.Write(new LogMessage("部门信息查询成功", LogMessageType.Success));

                QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门信息查询失败");
                this.Log.Write(new LogMessage("部门信息查询失败", LogMessageType.Error));

                QUERY_DEPARTMENT_RESPONSE = "QUERY_DEPARTMENT_FAILED";                //  用户登录失败信号
            }
            // 将响应信息发送回客户端，先发响应信息头，再发响应信息域
            //  响应信息头LOGIN_RESPONSE
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(QUERY_DEPARTMENT_RESPONSE));      //  将响应信号发送至客户端
            if (QUERY_DEPARTMENT_RESPONSE.Equals("QUERY_DEPARTMENT_SUCCESS"))
            {
                String json = JsonConvert.SerializeObject(departments);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion


        #region 

        /// <summary>
        /// 处理客户端的删除部门请求
        /// </summary>
        /// <param name="ar"></param>
        private void HandleQueryDepartmentEmployeeRequestEvent(IAsyncResult ar)
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
                    Console.WriteLine("接收了{0}个数据单元[部门的编号信息]", state.RecvLength);

                    this.Log.Write(new LogMessage("正在与" + ip.Address + " : " + ip.Port + "进行查询部门的员工信息...", LogMessageType.Information));
                    this.Log.Write(new LogMessage("接收了" + state.RecvLength.ToString() + "个数据单元[部门的编号信息]", LogMessageType.Information));
                    
                    //C- TODO 触发用户登录的事件
                    RaiseQueryDepartmentEmployeeRequestEvent(state);
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
        private void RaiseQueryDepartmentEmployeeRequestEvent(AsyncSocketState state)
        {
            string recvMsg = Encoding.UTF8.GetString(state.RecvDataBuffer, 0, state.RecvLength);
            
            Console.WriteLine(recvMsg);
            this.Log.Write(new LogMessage("接收到的数据" + recvMsg, LogMessageType.Information));
            
            string QUERY_DEPARTMENT_EMPLOYEE_RESPONSE;


            List<Employee> employees = DALEmployee.QueryEmployee(recvMsg);
            // List<Employee> employees = DALEmployee.QueryEmployee(int.Parse(recvMsg));

            this.Log.Write(new LogMessage("查询到的部门编号为"+ recvMsg + "的所有员工", LogMessageType.Information));

            if (employees != null)
            {
                Console.WriteLine("部门{0}的员工信息查询成功", recvMsg);
                this.Log.Write(new LogMessage("部门" + recvMsg+ "的员工信息查询成功", LogMessageType.Success));

                QUERY_DEPARTMENT_EMPLOYEE_RESPONSE = "QUERY_DEPARTMENT_EMPLOYEE_SUCCESS";               //  用户登录成功信号   
            }
            else
            {
                Console.WriteLine("部门{0}的员工信息查询失败", recvMsg);
                this.Log.Write(new LogMessage("部门" + recvMsg+ "的员工信息查询失败", LogMessageType.Error));

                QUERY_DEPARTMENT_EMPLOYEE_RESPONSE = "QUERY_DEPARTMENT_EMPLOYEE_FAILED";                //  用户登录失败信号
            }

            //  先发响应头
            this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(QUERY_DEPARTMENT_EMPLOYEE_RESPONSE));      //  将响应信号发送至客户端
            // 再发查询到的员工信息
            if (QUERY_DEPARTMENT_EMPLOYEE_RESPONSE.Equals("QUERY_DEPARTMENT_EMPLOYEE_SUCCESS"))
            {
                String json = JsonConvert.SerializeObject(employees);
                this.Send(state.ClientSocket, Encoding.UTF8.GetBytes(json));                    //  将
            }
        }

        #endregion
    
    }
}

