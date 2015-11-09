using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{

    ///  存储了航道局会签单中的项目结构体信息
    ///  对应数据库中的project表
    public class ContractProject
    {
        private int m_id;
        public int Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        //  要不存储ContractCategory信息，要不存储ContractCategory的id=ContractCategory

        private int m_categoryId;
        public int CategoryId
        {
            get { return this.m_categoryId; }
            set { this.m_categoryId = value; }
        }



        private String m_project;
        public String Project
        {
            get { return this.m_project; }
            set { this.m_project = value; }
        }
        
    }
}
