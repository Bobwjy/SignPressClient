using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


///  SOKCET 
using System.Net.Sockets;  
using System.Net;

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
            : this(localEP.Address, localEP.Port,1024)
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
                    Console.WriteLine("获取到一个来自" + ip.Address + " : " + ip.Port + "的连接...");

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

                    //  如果两次开始了异步的接收,所以当客户端退出的时候
                    //  会两次执行EndReceive
                    int recv = client.EndReceive(ar);   //  异步接收数据结束
                    if (recv == 0)
                    {
                        //  C - TODO 触发事件 (关闭客户端)
                        Close(state);
                        RaiseNetError(state);
                        return;
                    }
                    //TODO 处理已经读取的数据 ps:数据在state的RecvDataBuffer中
                    Console.WriteLine("接收了" + recv + "个数据单元");
                    Console.WriteLine(state.RecvDataBuffer);
                    //C- TODO 触发数据接收事件
                    RaiseDataReceived(state);
                }
                catch (SocketException e)
                {
                    //  C- TODO 异常处理
                    Console.WriteLine(e.ToString());
                    RaiseNetError(state);
                }
                finally
                {
                    //  继续接收来自来客户端的数据
                    client.BeginReceive(state.RecvDataBuffer, 0, state.RecvDataBuffer.Length, SocketFlags.None,
                     new AsyncCallback(HandleDataReceived), state);
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
    }
}
