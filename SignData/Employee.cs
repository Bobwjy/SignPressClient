using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{
    /*
     *  人员信息
     */
    class Employee
    {
        private int m_id;   //  签字人的ID
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }


        private String m_name;  // 签字人的姓名
        public String Name
        {
            get { return this.m_name; }
            set { this.m_name = value;}
        }

        private String m_positon;   //  签字人的职位位置
        public String Position
        {
            get { return this.m_positon; }
            set { this.m_positon = value; }
        }

        private Department m_department;    //签字人的所属部门
        public Department Department
        {
            get { return this.m_department; }
            set { this.m_department = value; }
        }

        private String m_username;      // 登录系统的用户名
        public String Username
        {
            get { return this.m_username; }
            set { this.m_username = value; }
        }

        private String m_password;      // 登录系统的密码
        public String Password
        {
            get { return this.m_password; }
            set { this.m_password = value; }
        }

        public void Show( )
        {
            Console.WriteLine("输出员工的信息");
            Console.WriteLine("ID :" + this.Id.ToString( ));
            Console.WriteLine("NAME : " + this.Name);
            Console.WriteLine("DEPARTMENT : " + this.Department.Id.ToString() + ", " + this.Department.Name);
            Console.WriteLine("USERNAME : " + this.Username);
            Console.WriteLine("PASSWORD : " + this.Password);
        }
    }
}
