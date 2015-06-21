using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{

    /*
     * 签字状态表的确定
     * 
     * 存储了针对某一张会签单所有签字人的签字状态
     * 
     * 本表对应用程序来说是一张只读表
     * 
     */
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
