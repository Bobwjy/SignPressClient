using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressServer.SignData
{

    /*  
     * 
     *  会签单模版类ContractTemplate
     *  对用数据库中的ConTemp
     */
    class ContractTemplate
    {
        private int m_id;   //  会签单模版的编号
        public  int Id
        {
            get{ return this.m_id;  }
            set{ this.m_id = value; }
        }

        private String m_name;     //  会签单名称
        public String Name
        {
            get{ return this.m_name;}
            set{ this.m_name = value;}
        }

        #region   column栏目信息[5个栏目]
        
        private int m_columnCount;      // 目前此接口无用，因为数据栏目定死是5个
        public int ColumnCount
        {
            get{ return this.m_columnCount; }
            set{ this.m_columnCount = value; }
        }

        private List<String> m_columnData;     //  存储5个栏目项的信息
        public List<String> ColumnData
        {
            get { return this.m_columnData; }
            set { this.m_columnData = value; }
        }
        
        #endregion


        #region  signinfo签字人信息[8个签字人]
        
        
        private int m_signCount;            //  签字人人数  此接口暂时无用，供扩展哟个，因为暂时签字人就是6+2=8个
        public int SignCount
        {
            get { return this.m_signCount; }
            set { this.m_signCount = value; }
        }


        private List<SignatureTemplate> m_signData;     //  签字人信息
        public List<SignatureTemplate> SignData
        {
            get { return this.m_signData; }
            set { this.m_signData = value; }
        }
        
        
        #endregion
    }
}
