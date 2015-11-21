using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignPressServer.SignData
{
    public class Search
    {
        //enum SearchType
        //{ 
        //    GET_SDEAPRTMENT_CATEGORY_HDJCONTRACT_COUNT,
        //    GET_CATEGORY_HDJCONTRACT_COUNT,
        //    GET_
        //}
        // 模糊搜索的信息串
        public String ConId { get; set; }
        public String ProjectName { get; set; }
        
        //  查询日期的信息串
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        //  员工的ID
        public int EmployeeId { get; set; }

        //  会签单是否可以下载
        public int Downloadable { get; set; }
       
        //  会签单统计信息
        // 用户查询表中，{ 当前类别CategoryShortCall | 当前部门签署的SDepartmentShortlCall | 当前年份Year的} 会签单信息或者数目 
        public String CategoryShortCall { get; set; }
        public String SDepartmentShortlCall { get; set; }
        public int Year { get; set; }
        //  
    }
}
