using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Model.Models;

namespace IPM.Data.ModelReport
{
    public class ReportInterviewers
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public int NumOfCandidates { get; set; }

        public string PassFail { get; set; }
                
        //public List<Candidate> Candidates { get; set; }        
    }
}
