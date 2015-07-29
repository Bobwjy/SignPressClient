using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignPressClient.Model
{
    public class Search
    {
        public int EmployeeId { get; set; }
        public string ConId { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int Downloadable { get; set; }
    }
}
