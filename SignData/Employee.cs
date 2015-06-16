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


        private int m_canSubmit;     //  是否可以提交签单
        public int CanSubmit
        {
            get { return this.m_canSubmit; }
            set { this.m_canSubmit = value; }
        }

        private int m_canSign;     //  是否可以提交签单
        public int CanSign
        {
            get { return this.m_canSign; }
            set { this.m_canSign = value; }
        }


        private int m_isAdmin;     //  是否可以提交签单
        public int IsAdmin
        {
            get { return this.m_isAdmin; }
            set { this.m_isAdmin = value; }
        }

        private User m_user;        //  用户登录信息
        public User User
        {
            get { return this.m_user; }
            set { this.m_user = value; }
        }

        private String m_signatureUrl;      // 用户签名的存储路径
        public String SignatureUrl
        {
            get { return this.m_signatureUrl; }
            set { this.m_signatureUrl = value; }
        }


        public override String ToString( )
        {
            return "Id = " + this.Id.ToString() + "\n" 
                + "Name = " + this.Name.ToString( ) + "\n"
                + "Deaprtment = " + this.Department.ToString( ) + "\n"
                + "User = " + this.User.ToString() + "\n";

        }
    }
}
