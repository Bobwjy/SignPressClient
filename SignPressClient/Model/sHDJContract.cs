using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    /*
* 会签单的简单信息
*/
    public class SHDJContract
    {
        private String m_id;                    //  审批会签单编号
        public String Id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        private String m_name;              // 方案名称
        public String Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        private String m_projectName;       // 工程名称
        public String ProjectName
        {
            get { return this.m_projectName; }
            set { this.m_projectName = value; }
        }

        private String m_submitDate;        // 提交日期
        public String SubmitDate
        {
            get { return this.m_submitDate; }
            set { this.m_submitDate = value; }
        }

        private String m_submitEmployeeName;      //  提交人的信息
        public String SubmitEmployeeName
        {
            get { return this.m_submitEmployeeName; }
            set { this.m_submitEmployeeName = value; }
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

        private String m_signDate;          //  签字日期
        public String SignDate
        {
            get { return this.m_signDate; }
            set { this.m_signDate = value; }
        }

        private String m_signRemark;        // 本人签字备注
        public String SignRemark
        {
            get { return this.m_signRemark; }
            set { this.m_signRemark = value; }
        }

        private String m_signResult;        //  本人签字结果
        public String SignResult
        {
            get { return this.m_signResult; }
            set { this.m_signResult = value; }
        }
    }

}
