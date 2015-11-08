using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class ContractCategory
    {

        private int m_id;       ///  编号信息
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private string m_category;      //  类别信息
        public string Category
        {
            get { return this.m_category; }
            set { this.m_category = value; }
        }

        private string m_categoryShortCall;       //  类别信息对应的编号前缀
        public string CategoryShortCall
        {
            get { return this.m_categoryShortCall; }
            set { this.m_categoryShortCall = value; }
        }
            

    }

    public class ContractIdCategory
    {

        private int m_id;       ///  编号信息
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        //private int m_departmentId; ///  部门编号信息
        //public int DepartmentId
        //{
        //    get { return this.m_departmentId; }
        //    set { this.m_departmentId = value; }
        //}

        private Department m_department;       ///  部门信息
        public Department Department
        {
            get { return this.m_department; }
            set { this.m_department = value; }
        }

        private String m_category;
        public String Category
        {
            get { return this.m_category; }
            set { this.m_category = value; }
        }
    }
}
