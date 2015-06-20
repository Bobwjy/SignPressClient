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
        private ContractTemplate m_conTemp;     //  所对应的会签单模版的信息
        public ContractTemplate ConTemp
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
            get { return this.ColumnDatas; }
            set { this.m_columnDatas = value; }
        }

        private List<String> m_signRemark;      //  每一个人的评论信息
        public List<String> SginRemark
        {
            get { return this.m_signRemark; }
            set { this.m_signRemark = value; }
        }
    }
}
