using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{
    /// <summary>
    /// 对应数据库中的item表
    /// 标识工作两的类型
    /// </summary>
    public class ContractItem
    {
        private int m_id;
        public int Id                  //  编号
        
        { 
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private int m_projectId;
        public int ProjectId
        {
            get { return this.m_projectId; }
            set { this.m_projectId = value; }
        }

        private string m_item;
        public string Item
        {
            get { return this.m_item; }
            set { this.m_item = value; }
        }
    }
}
