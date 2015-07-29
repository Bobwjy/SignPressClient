using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace SignPressClient.Model
{

    public enum SocketMessageFlag
    {
        MESSAGE_PASTE,                  //  数据粘包 
        MESSAGE_MISSE,                  //  数据丢白
        MESSAGE_RIGHT,                  //  数据正常
    }

    public class SocketMessage
    {
        public static char DEFAULT_SEPARATOR = '~';


        private String m_head;
        public String Head
        {
            get { return this.m_head; }
            set { this.m_head = value; }
        }
        private int m_length;
        private String m_message;
        public String Message
        {
            get { return this.m_message; }
            set { this.m_message = value; }
        }
        private String[] m_splits;
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

        private SocketMessageFlag m_flag;
        public SocketMessageFlag Flag
        {
            get { return this.m_flag; }
            set { this.m_flag = value; }
        }

        private String m_cache;
        public String Cancle
        {
            get { return this.m_cache; }
            set { this.m_cache = value; }
        }

        /*
        发送的信息头         
         */
        public SocketMessage()
        {
        }

        public SocketMessage(Request request)
        {
            this.m_package = request.ToString() + DEFAULT_SEPARATOR + "1" + DEFAULT_SEPARATOR + "0";
        }

        public SocketMessage(Request request, Object obj)
        {
            String message = JsonConvert.SerializeObject(obj);

            this.m_package = request.ToString() + DEFAULT_SEPARATOR + message.Length + DEFAULT_SEPARATOR + message;
        }

        public SocketMessage(Response response, Object obj)
        {
            String message = JsonConvert.SerializeObject(obj);
            this.m_package = response.ToString() + DEFAULT_SEPARATOR + message.Length + DEFAULT_SEPARATOR + message;
        }

        public SocketMessage(Response response)
        {
            this.m_package = response.ToString() + DEFAULT_SEPARATOR + "1" + DEFAULT_SEPARATOR + "0";
        }

        /// <summary>
        /// 拆包
        /// </summary>
        /// <returns></returns>
        //public SocketMessageFlag Split()
        //{
        //    this.m_splits = this.m_package.Split(DEFAULT_SEPARATOR);    //返回由'/'分隔的子字符串数组
        //    Console.WriteLine("信息头的长度" + this.m_splits.Length);
        //    foreach (string s in m_splits)
        //    {
        //        Console.WriteLine(s);
        //    }


        //  近会出现一下几种粘包问题
        //QUERY_UNSIGN_CONTRACT_REQUEST; 1; 1; QUERY_SIGN_REFUSE_REQUEST; 1; 1;
        //QUERY_REQUEST; QUERY_SIGN_REFUSE_REQUEST; 1; 1;
        //QUERY_SIGN_REFUSE_REQUEST; 1; 1; QUERY_REQUEST;
        //QUERY_REQUEST; QUERY_REQUEST; 




        //    // 3数据格式说明数据中有[数据头 + 长度 + 数据]
        //    if (this.m_splits.Length == 3)
        //    {
        //        this.m_head = this.m_splits[0];
        //        this.m_length = int.Parse(this.m_splits[1]);

        //        // 此处应该处理丢包或者粘包的问题
        //        if(this.m_splits[2].Length == this.m_length)       ///  接收正常
        //        {
        //            this.m_message = this.m_splits[2];
        //            this.m_flag = SocketMessageFlag.MESSAGE_RIGHT;
        //        }
        //        else if (this.m_splits[2].Length > this.m_length) //  后面接收到的数据长度大于发送长度 -=> 粘包
        //        {   // 粘包时有两种处理方案，
        //            // 第一种方案是丢弃后面的包
        //            // 第二种方案是将后面的数据重新拆解
        //            this.m_message = this.m_splits[2].Substring(0, this.m_length);

        //            //  此时后面的数据还有信息，那么我们应该截取出后面的数据放到缓存里面
        //            this.m_flag = SocketMessageFlag.MESSAGE_PASTE;         //  粘包
        //            int currLength = this.m_head.Length + this.m_length.ToString().Length + this.m_message.Length;
        //            this.m_cache = this.m_package.Substring(currLength, this.m_package.Length);
        //        }
        //        else if (this.m_splits[2].Length < this.m_length)    // 丢包
        //        {   // 丢包时使用stringbuilder重复接收
        //            //////////////////////////////////////////
        //            this.m_message = this.m_splits[2];////////
        //            //////////////////////////////////////////

        //            this.m_flag = SocketMessageFlag.MESSAGE_RIGHT;
        //        }
        //    }
        //    else     //  出现了整个数据包的粘连，我们截取出需要的长度
        //    {
        //        this.m_head = this.m_splits[0];
        //        this.m_length = int.Parse(this.m_splits[1]);

        //        // 此处应该处理丢包或者粘包的问题
        //        if (this.m_splits[2].Length > this.m_length) //  后面接收到的数据长度大于发送长度 -=> 粘包
        //        {
        //            // 粘包时有两种处理方案，
        //            // 第一种方案是丢弃后面的包
        //            // 第二种方案是将后面的数据重新拆解
        //            this.m_message = this.m_splits[2].Substring(0, this.m_length);
        //            // QUERY_UNSIGN_CONTRACT_REQUEST;1;1QUERY_SIGN_REFUSE_REQUEST;1;1

        //            this.m_flag = SocketMessageFlag.MESSAGE_PASTE;         //  粘包
        //            int currLength = this.m_head.Length + this.m_length.ToString().Length + this.m_message.Length + 2 * DEFAULT_SEPARATOR.ToString().Length;
        //            this.m_cache = this.m_package.Substring(currLength);
        //            this.m_package = this.m_cache;
        //        }
        //    }

        //    return m_flag;
        //}
    }

}
