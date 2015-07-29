using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class sEmployee
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

        private string m_department;    //签字人的所属部门
        public string Department
        {
            get { return this.m_department; }
            set { this.m_department = value; }
        }

        private string m_departmentId;    //签字人的所属部门
        public string DepartmentId
        {
            get { return this.m_departmentId; }
            set { this.m_departmentId = value; }
        }

        private string m_canSubmit;     //  是否可以提交签单
        public string CanSubmit
        {
            get { return this.m_canSubmit; }
            set { this.m_canSubmit = value; }
        }

        private string m_canSign;     //  是否可以提交签字
        public string CanSign
        {
            get { return this.m_canSign; }
            set { this.m_canSign = value; }
        }


        private string m_isAdmin;     //  是否可以提交签单
        public string IsAdmin
        {
            get { return this.m_isAdmin; }
            set { this.m_isAdmin = value; }
        }

        private string m_user;
        public string UserName
        {
            get { return this.m_user; }
            set { this.m_user = value; }
        }
    }
}
