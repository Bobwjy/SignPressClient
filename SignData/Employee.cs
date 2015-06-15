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
            set { this.m_name = value; }
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


        private bool m_canSubmit;     //  是否可以提交签单
        public bool CanSubmit
        {
            get { return this.m_canSubmit; }
            set { this.m_canSubmit = value; }
        }

        private bool m_canSign;     //  是否可以提交签单
        public bool CanSign
        {
            get { return this.m_canSign; }
            set { this.m_canSign = value; }
        }


        private bool m_isAdmin;     //  是否可以提交签单
        public bool IsAdmin
        {
            get { return this.m_isAdmin; }
            set { this.m_isAdmin = value; }
        }

        private User m_user;
        public User User
        {
            get { return this.m_user; }
            set { this.m_user = value; }
        }

        public void Show()
        {
            Console.WriteLine("输出员工的信息");
            Console.WriteLine("ID :" + this.Id.ToString());
            Console.WriteLine("NAME : " + this.Name);
            Console.WriteLine("DEPARTMENT : " + this.Department.Id.ToString() + ", " + this.Department.Name);
            Console.WriteLine("USERNAME : " + this.User.Username);
            Console.WriteLine("PASSWORD : " + this.User.Password);
        }
    }
}
