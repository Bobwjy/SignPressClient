using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class SignatureStatus
    {
        private String m_id;            // 签字状态表的编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private String m_conId;        //  处理的会签单的编号
        public String ConId
        {
            get { return this.m_conId; }
            set { this.m_conId = value; }
        }

        private List<String> m_results;  //  签字状态表
        public List<String> Results
        {
            get { return this.m_results; }
            set { this.m_results = value; }
        }

        private int m_agreeCount;            //  同意人数计数器
        public int AgreeCount
        {
            get { return this.m_agreeCount; }
            set { this.m_agreeCount = value; }
        }

        private int m_refuseCount;              // 拒绝人数计数器
        public int RefuseCount
        {
            get { return this.m_refuseCount; }
            set { this.m_refuseCount = value; }
        }
    }
}
