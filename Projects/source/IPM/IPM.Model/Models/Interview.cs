using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class Interview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime StartTime { get; set; }

        public int RoundProcessID { get; set; }

        public int CandidateID { get; set; }

        public bool? Result { get; set; }

        public string Record { get; set; }

        public int? InterviewerID { get; set; }

        public int? InterviewAdminID { get; set; }

        [ForeignKey("RoundProcessID")]
        public virtual RoundProcess RoundProcess { get; set; }

        [ForeignKey("CandidateID")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("InterviewerID")]
        [InverseProperty("InterviewersInterviews")]
        public virtual User Interviewer { get; set; }

        [ForeignKey("InterviewAdminID")]
        [InverseProperty("InterviewAdminsInterviews")]
        public virtual User InterviewAdmin { get; set; }

        public virtual ICollection<InterviewAnswer> InterviewAnswers { get; set; }

        public bool Active { get; set; }

        public Interview()
        {
            Active = true;
            InterviewAnswers = new HashSet<InterviewAnswer>();
        }
    }
}
