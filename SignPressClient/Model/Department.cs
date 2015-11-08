using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class Department
    {
        protected int m_id;           // 部门编号
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        protected String m_name;      // 部门姓名
        public String Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }


        //  modify by gatieme at 2015-08-08 16:09
        //  为部门添加部门简称
        protected String m_shortCall;     //  部门简称
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


    //  此部门信息包含了当前部门的申请权限
    //  申请选线标识是当前部门是否可以申请，界河项目，内河项目，应急项目和例会项目
    public class SDepartment : Department
    {
        //private int m_id;           // 部门编号
        //public int Id
        //{
        //    get { return this.m_id; }
        //    set { this.m_id = value; }
        //}

        //private String m_name;      // 部门姓名
        //public String Name
        //{
        //    get { return this.m_name; }
        //    set { this.m_name = value; }
        //}

        ////  modify by gatieme at 2015-08-08 16:09
        ////  为部门添加部门简称
        //private String m_shortCall;     //  部门简称
        //public String ShortCall
        //{
        //    get { return this.m_shortCall; }
        //    set { this.m_shortCall = value; }
        //}
        public Department ToDepartment()
        {
            Department department = new Department();
            department.Id = this.Id;
            department.Name = this.Name;
            department.ShortCall = this.ShortCall;

            return department;
        }

        private string m_canBoundary;   //  当前部门是否可以申请界河项目
        public string CanBoundary
        {
            get { return this.m_canBoundary; }
            set { this.m_canBoundary = value; }
        }

        private string m_canInland;     //  当前部门是否可以申请内河项目1
        public string CanInland
        {
            get { return this.m_canInland; }
            set { this.m_canInland = value; }
        }

        private string m_canEmergency;           //  当前部门是否可以申请应急项目
        public string CanEmergency
        {
            get { return this.m_canEmergency; }
            set { this.m_canEmergency = value; }
        }
        private string m_canRegular;            //  当前部门是否可以申请例会项目
        public string CanRegular
        {
            get { return this.m_canRegular; }
            set { this.m_canRegular = value; }
        }

        public override string ToString()
        {
            return "Id = " + this.m_id.ToString() + ", Name = " + this.m_name.ToString() + ", ShortCall = " + this.m_shortCall.ToString();
        }
    }
}
