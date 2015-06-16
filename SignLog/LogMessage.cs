using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignLog
{
    /// <summary>
    /// 表示一个日志记录的对象
    /// </summary>
    public class LogMessage
    {        //日志记录的时间
        private DateTime datetime;
        //日志记录的内容
        private string text;
        //日志记录的类型
        private LogMessageType type;

        /// <summary>
        /// 创建新的日志记录实例;日志记录的内容为空,消息类型为LogMessageType.Unknown,日志时间为当前时间
        /// </summary>
        public LogMessage()
            : this("", LogMessageType.Unknown)
        {
        }

        /// <summary>
        /// 创建新的日志记录实例;日志事件为当前时间
        /// </summary>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public LogMessage(string t, LogMessageType p)
            : this(DateTime.Now, t, p)
        {
        }

        /// <summary>
        /// 创建新的日志记录实例;
        /// </summary>
        /// <param name="dt">日志记录的时间</param>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public LogMessage(DateTime dt, string t, LogMessageType p)
        {
            datetime = dt;
            type = p;
            text = t;
        }

        /// <summary>
        /// 获取或设置日志记录的时间
        /// </summary>
        public DateTime Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }

        /// <summary>
        /// 获取或设置日志记录的文本内容
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 获取或设置日志记录的消息类型
        /// </summary>
        public LogMessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public new string ToString()
        {
            return datetime.ToString() + "\t" + text + "\n";
        }
    }
}
