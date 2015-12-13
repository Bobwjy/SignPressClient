using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    /*
     * 签字明细表信息
     * 
     * 对应每一次签字记录的信息
     * 存储了以下信息
     * 什么时间，什么人对哪张会签单，同意还是拒绝，有无备注信息
     * 描述如下
     * 时间SignDate时，签字人SignEmpId，
     * 对会签单SignConId进行了SignResult处理，
     * 备注信息m_signRemark
     */
    public class SignatureDetail
    {

        private String m_date;      //  签字的日期
        public String Date
        {
            get{return this.m_date;}
            set{this.m_date = value;}
        }


        private int m_empId;        // 签字人的编号
        public int EmpId
        {
            get { return this.m_empId; }
            set { this.m_empId = value; }
        }

        private String m_conId;
        public String ConId
        {
            get { return this.m_conId; }
            set { this.m_conId = value; }
        }

        private int m_result;       // 签字状态[-1拒绝，1同意，0未知]
        public int Result
        {
            get{return this.m_result; }
            set{this.m_result = value;}
        }

        private String m_remark;    // 签字的备注
        public String Remark
        {
            get{return this.m_remark;}
            set{this.m_remark = value;}
        }
        
    }
}
