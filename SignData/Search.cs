using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    public class Search
    {
        // 模糊搜索的信息串
        public String ConId { get; set; }
        public String ProjectName { get; set; }
        
        //  日期的信息串
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        //  员工的ID
        public int EmployeeId { get; set; }    

       
    }
}
