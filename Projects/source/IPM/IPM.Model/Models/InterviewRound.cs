using System.Collections.Generic;

namespace IPM.Model.Models
{

    /// <summary>
    /// InterviewRound model for module Management InterviewRound
    /// </summary>
    public class InterviewRound
    {
        public int ID
        {
            get;

            set;
        }


        public string RoundName
        {
            get;

            set;
        }


        public string Description
        {
            get;

            set;
        }


        public int GuidelineID
        {
            get;

            set;
        }


        public virtual Guideline Guideline
        {
            get;

            set;
        }


        public virtual ICollection<RoundProcess> RoundProcesses
        {
            get;

            set;
        }


        public bool Active
        {
            get;

            set;
        }


        public InterviewRound()
        {
            Active = true;
            RoundProcesses = new HashSet<RoundProcess>();
        }
    }
}
