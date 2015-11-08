using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class UserHelper
    {
        //public UserHelper()
        //{ 
            
        //}
        public const string DEFAULT_SIGNATURE_PATH = "./signature/";
 
        public static Employee UserInfo { get; set; }

        public static List<Department> DepList { get; set; }
        public static List<SDepartment> SDepList { get; set; } 

        public static List<Employee> EmpList { get; set; }
        public static List<sEmployee> sEmpList { get; set; }

        public static List<Templete> TempList { get; set; }

        public static Templete SelectedTemp { get; set; }

        public static List<SHDJContract> ToDoList { get; set; }

        public static List<SHDJContract> DoneList { get; set; }

        public static List<SHDJContract> PenddingList { get; set; }

        public static List<SHDJContract> RefuseList { get; set; }

        public static List<SHDJContract> AgreeList { get; set; }
        public static List<SHDJContract> AgreeUndownList { get; set; }

        public static List<String> DepartmentShortCallList{ get; set;}          ///  存放部门简称
    

    }
}
