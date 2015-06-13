using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{
    /*
     * 
     *  部门类Department
     *  对应于数据库中的部门表department
     *  
     */
    class Department
    {
        private int m_id;           // 部门编号
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private String m_name;      // 部门姓名
        public String Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
    }
}
