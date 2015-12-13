using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{
    public class ContractCategory
    {

        //public ContractCategory(int id, string category, string categoryShortCall)
        //{ 
        //    this.m_id = id;
        //    this.m_category = category;
        //    this.m_categoryShortCall = categoryShortCall;
        //}    

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
        //const ContractCategory BOUNDARY_CONTRACT_CATEGORY   = new ContractCategory(1, "界河项目", "界");
        //const ContractCategory INLAND_CONTRACT_CATEGORY     = new ContractCategory(2, "内河项目", "内");
        //const ContractCategory EMERGENCY_CONTRACT_CATEGORY  = new ContractCategory(3, "应急项目", "应");
        //const ContractCategory REGULAR_CONTRACT_CATEGORY    = new ContractCategory(4, "例会项目", "例");

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

        private ContractCategory m_contractCategory;
        public ContractCategory ContractCategory
        {
            get { return this.m_contractCategory; }
            set { this.m_contractCategory = value; }
        }
    }
}
