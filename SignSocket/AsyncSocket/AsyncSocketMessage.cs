using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using SignPressServer.SignTools;
using Newtonsoft.Json;

namespace SignPressServer.SignSocket.AsyncSocket
{
    public class AsyncSocketMessage
    {

        private String      m_head;
        public String Head
        {
            get{ return this.m_head; }
            set{ this.m_head = value; }    
        }
        private int         m_length;
        private String      m_message;
        public String Message
        {
            get { return this.m_message; }
            set { this.m_message = value; }
        }
        private String[]    m_splits;
        public String[] Splits
        {
            get { return this.m_splits; }
            set { this.m_splits = value; }
        }
        private String m_package;
        public String Package
        {
            get { return this.m_package; }
            set { this.m_package = value; }
        }


        /*
        发送的信息头         
         */
        public AsyncSocketMessage()
        {
        }
        public AsyncSocketMessage(ClientRequest request)
        {
            this.m_package = request.ToString() + ";";
        }
        public AsyncSocketMessage(ClientRequest request, Object obj)
        {
            String message = JsonConvert.SerializeObject(obj);

            this.m_package = request.ToString() + ";" + message.Length + ";" + message;
        }

        public AsyncSocketMessage(ServerResponse response, Object obj)
        {
            String message = JsonConvert.SerializeObject(obj);
            this.m_package = response.ToString() + ";" + message.Length + ";" + message;
        }
        public AsyncSocketMessage(ServerResponse response)
        {
            this.m_package = response.ToString() + ";";
        }

        /// <summary>
        /// 拆包
        /// </summary>
        /// <returns></returns>
        public void Split()
        {
            this.m_splits = this.m_package.Split(';');    //返回由'/'分隔的子字符串数组
            
            foreach (string s in m_splits)
            {
                Console.WriteLine(s);
            }

            if(this.m_splits.Length == 3)
            {
                this.m_head = this.m_splits[0];
                this.m_length = int.Parse(this.m_splits[1]);
                this.m_message = this.m_splits[2];
            }
            else
            {
                this.m_head = this.m_splits[0]; 
            }
        }


        /// <summary>
        /// 获取信息的大小
        /// </summary>
        /// <returns></returns>
    }
}
