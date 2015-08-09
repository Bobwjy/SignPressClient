﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class Department
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


        //  modify by gatieme at 2015-08-08 16:09
        //  为部门添加部门简称
        private String m_shortCall;     //  部门简称
        public String ShortCall
        {
            get { return this.m_shortCall; }
            set { this.m_shortCall = value; }
        }


        public override string ToString()
        {
            return "Id = " + this.m_id.ToString() + ", Name = " + this.m_name.ToString() + ", ShortCall = " + this.m_shortCall.ToString();
        }
    }
}
