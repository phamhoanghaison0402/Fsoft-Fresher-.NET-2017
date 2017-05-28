using System.Collections.Generic;

namespace IPM.Model.Models
{
    public class RoundProcess
    {
        public int ID { get; set; }

        public int InterviewRoundID { get; set; }

        public int InterviewProcessID { get; set; }

        public virtual InterviewProcess InterviewProcess { get; set; }

        public virtual InterviewRound InterviewRound { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }

        public int RoundOrder { get; set; }

        public bool Active { get; set; }

        public RoundProcess()
        {
            Active = true;
            Interviews = new HashSet<Interview>();
        }
    }
}
