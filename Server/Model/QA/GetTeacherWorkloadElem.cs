using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.QA
{
    public class GetTeacherWorkloadElem
    {
        public string year { get; set; }
        public string group_standard_name { get; set; }
        public string group_fullname { get; set; }
        public string discipline { get; set; }
        public string finance_form { get; set; }
        public int term_num { get; set; }
        public int sum { get; set; }
        public double in_week { get; set; }
        public int theory { get; set; }
        public int practice { get; set; }
        public int consultation { get; set; }
        public int course { get; set; }
        public int exam { get; set; }
    }
}
