using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// SOCKET
using System.Net.Sockets;


namespace SignPressServer.SignSocket.AsyncSocket
{
    /// <summary>
    /// 异步SOCKET TCP 中用来存储客户端状态信息的类
    /// </summary>
    public class AsyncSocketState
    {

        #region 字段
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        private byte[] m_recvBuffer;

        /// <summary>
        /// 本次通信中接收到的数据的大小
        /// </summary>
        private int m_recvLength;
       
        /// <summary>
        /// 客户端发送到服务器的报文
        /// 注意:在有些情况下报文可能只是报文的片断而不完整
        /// </summary>
        private string m_datagram;

        /// <summary>
        /// 客户端的Socket
        /// </summary>
        private Socket m_clientSocket;

        #endregion

        #region 属性

        /// <summary>
        /// 接收数据缓冲区 
        /// </summary>
        public byte[] RecvDataBuffer
        {
            get { return this.m_recvBuffer;}
            set
            {
                this.m_recvBuffer = value;
            }
        }

        public int RecvLength
        {
            get { return this.m_recvLength; }
            set { this.m_recvLength = value; }
        }
        /// <summary>
        /// 存取会话的报文
        /// </summary>
        public string Datagram
        {
            get{ return this.m_datagram; }
            set{ this.m_datagram = value; }
        }

        /// <summary>
        /// 获得与客户端会话关联的Socket对象
        /// </summary>
        public Socket ClientSocket
        {
            get { return this.m_clientSocket; }
        }


        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientSocket">会话使用的Socket连接</param>
        public AsyncSocketState(Socket clientSocket)
        {
            this.m_clientSocket = clientSocket;
        }

        /// <summary>
        /// 初始化数据缓冲区
        /// </summary>
        public void InitBuffer( )
        {
            if (this.m_recvBuffer == null&& this.m_clientSocket != null)
            {
                this.m_recvBuffer = new byte[this.m_clientSocket.ReceiveBufferSize];
            }
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        public void Close( )
        {

            //关闭数据的接受和发送
            this.m_clientSocket.Shutdown(SocketShutdown.Both);

            //清理资源
            this.m_clientSocket.Close();
        }
    }
}
