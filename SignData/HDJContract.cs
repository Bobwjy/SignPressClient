using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    /*
     * 合同会签单的信息
     */
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


        private String m_id;                    //  审批会签单编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private List<String> m_columnDatas;     //  存储会签单的前5项栏目信息
        public List<String> ColumnDatas
        {
            get { return this.m_columnDatas; }
            set { this.m_columnDatas = value; }
        }

        private Employee m_submitEmployee;      //  提交人的信息
        public Employee SubmitEmployee
        {
            get { return this.m_submitEmployee; }
            set { this.m_submitEmployee = value; }
        }

        private ContractTemplate m_conTemp;     //  所对应的会签单模版的信息
        public ContractTemplate ConTemp
        {
            get { return this.m_conTemp; }
            set { this.m_conTemp = value; }
        }

        private List<String> m_signRemark;      //  每一个人的批注信息
        public List<String> SginRemark
        {
            get { return this.m_signRemark; }
            set { this.m_signRemark = value; }
        }

        private int m_currLevel;                //  当前进度节点
        public int CurrLevel
        {
            get { return this.m_currLevel; }
            set { this.m_currLevel = value; }
        }

        private int m_maxLevel;                 //  最大节点信息
        public int MaxLevel
        {
            get { return this.m_maxLevel; }
            set { this.m_maxLevel = value; }
        }
    }
}
