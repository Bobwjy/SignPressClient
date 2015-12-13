using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    /*
     *  会签单栏目信息
     */
    public class ColumnTemplate
    {
        private String m_columnName;         //  栏目名称
        public String ColumnName
        {
            get { return this.m_columnName; }
            set { this.m_columnName = value; }
        }
        
        
        private String m_columnData;         //  栏目信息
        public String ColumnData
        {
            get { return this.m_columnData; }
            set { this.m_columnData = value; }
        }
    
    }
}
