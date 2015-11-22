using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class ContractProject
    {
        public int Id;
        private int m_id
        {
            get { return this.Id; }
            set { this.Id = value; }
        }

        //  要不存储ContractCategory信息，要不存储ContractCategory的id=ContractCategory
        //public ContractCategory Category;
        //private ContractCategory m_category
        //{
        //    get { return this.Category; }
        //    set { this.Category = value; }
        //}
        public int CategoryId;
        private int m_categoryId
        {
            get { return this.CategoryId; }
            set { this.CategoryId = value; }
        }



        public String Project;
        private String m_project
        {
            get { return this.Project; }
            set { this.Project = value; }
        }
    }
}
