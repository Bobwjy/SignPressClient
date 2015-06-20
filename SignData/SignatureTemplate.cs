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

        private int m_signId;           //  签字人编号
        public int  SignId
        {
            get { return this.m_signId; }
            set { this.m_signId = value; }
        }

        private int m_signlevel;        //  签字人签字顺序级别
        public int SignLevel
        {
            get { return this.m_signlevel; }
            set { this.m_signlevel = value; }
        }

        private int m_signPosX;         // 签字人坐标X
        public int SignPosX
        {
            get{ return this.m_signPosX; }
            set{ this.m_signPosX = value; }
        }
    
        private int m_signposY;         // 签字人坐标Y
        public int SignPosY
        {
            get { return this.m_signposY; }
            set { this.m_signposY = value; }
        }


    
    }
}
