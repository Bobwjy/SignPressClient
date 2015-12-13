using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    /*
     * 会签单编号前缀信息串
     */

    public class ContractNoPrefix
    {
        private int m_id;
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private Department m_department;
        public Department Deaprtment
        {
            get { return this.m_department; }
            set { this.m_department = value; }
        }


        private String m_prefix;            //  会签单编号的前缀
        public String Prefix
        {
            get { return this.m_prefix; }
            set { this.m_prefix = value; }
        }
    }
}
