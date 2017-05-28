using System;
using System.Collections.Generic;

namespace IPM.Model.Models
{
    public class InterviewProcess
    {
        public int ID { get; set; }

        public string ProcessName { get; set; }

        public int PositionID { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual Position Position { get; set; }

        public virtual ICollection<RoundProcess> RoundProcesses { get; set; }

        public bool Active { get; set; }

        public InterviewProcess()
        {
            Active = true;
            RoundProcesses = new HashSet<RoundProcess>();
        }
    }
}
