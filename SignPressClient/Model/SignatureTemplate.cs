using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class SignatureTemplate
    {
        private String m_signInfo;      // 签字人职位信息
        public String SignInfo
        {
            get { return this.m_signInfo; }
            set { this.m_signInfo = value; }
        }

        /*private int m_signId;           //  签字人编号
        public int SignId
        {
            get { return this.m_signId; }
            set { this.m_signId = value; }
        }*/
        private Employee m_signEmployee;
        public Employee SignEmployee
        {
            get { return this.m_signEmployee; }
            set { this.m_signEmployee = value; }
        }

        private int m_signPosX;         // 签字人坐标X
        public int SignPosX
        {
            get { return this.m_signPosX; }
            set { this.m_signPosX = value; }
        }

        private int m_signposY;         // 签字人坐标Y
        public int SignPosY
        {
            get { return this.m_signposY; }
            set { this.m_signposY = value; }
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
