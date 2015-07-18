using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{
    /*
     * 
     *  签字信息模版类SignatureTemplate
     *  数据库中的signaturelevel表就是在此数据结构的基础上，进行的一次封装
     *  包括了一下信息
     *  签字人的职位信息m_signinfo，此字段不同于员工表中的职位，而是如表格中的申请单位项目负责人（签字）栏目的字段信息
     *  签字人的编号m_signposX, 直接对应该位置的签字人信息
     *  签字人的签字位置X坐标和Y坐标，m_signPosX和m_signPosY
     *  
     */
    public class SignatureTemplate
    {
        private String  m_signInfo;      // 签字人职位信息
        public String   SignInfo
        {
            get{ return this.m_signInfo; }
            set{ this.m_signInfo = value; }
        }

        //private int m_signId;           //  签字人编号
        //public int  SignId
        //{
        //    get { return this.m_signId; }
        //    set { this.m_signId = value; }
        //}

        private Employee m_signEmployee;        //  签字人的信息
        public Employee SignEmployee
        { 
            get{return this.m_signEmployee; }
            set{this.m_signEmployee = value;}
        }

        private int m_signlevel;        //  签字人签字顺序级别
        public int SignLevel
        {
            get { return this.m_signlevel; }
            set { this.m_signlevel = value; }

        }


        private int m_canView;            //  用户是否有查看会签单审核状态的权限
        public int CanView
        {
            get { return this.m_canView; }
            set { this.m_canView = value; }
        }

        private int m_canDownload;              //  当前签字人是否有下载会签单的权限
        public int CanDownload
        {
            get { return this.m_canDownload; }
            set { this.m_canDownload = value; }
        }


    
    }
}
