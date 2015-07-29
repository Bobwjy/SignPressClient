using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class HDJContract
    {
        private String m_name;
        public String Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        private String m_submitDate;
        public String SubmitDate
        {
            get { return this.m_submitDate; }
            set { this.m_submitDate = value; }
        }

        private Templete m_conTemp;     //  所对应的会签单模版的信息
        public Templete ConTemp
        {
            get { return this.m_conTemp; }
            set { this.m_conTemp = value; }
        }

        private String m_id;                    //  审批会签单编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private List<String> m_columnDatas;     //  存储会签单
        public List<String> ColumnDatas
        {
            get { return this.m_columnDatas; }
            set { this.m_columnDatas = value; }
        }

        private List<String> m_signRemarks;      //  每一个人的评论信息
        public List<String> SignRemarks
        {
            get { return this.m_signRemarks; }
            set { this.m_signRemarks = value; }
        }

        private Employee m_submitEmployee;
        public Employee SubmitEmployee
        {
            get { return this.m_submitEmployee; }
            set { this.m_submitEmployee = value; }
        }

        private List<int> m_signResults;            // 每个人的信息
        public List<int> SignResults
        {
            get { return this.m_signResults; }
            set { this.m_signResults = value; }
        }
    }
}
