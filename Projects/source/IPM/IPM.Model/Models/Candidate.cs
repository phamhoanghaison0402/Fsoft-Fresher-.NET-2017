using System;
using System.Collections.Generic;

namespace IPM.Model.Models
{
    public class Candidate
    {
        public int ID { get; set; }

        public int? InterviewAdminID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime Birthdate { get; set; }

        public string IDCard { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string University { get; set; }

        public string Major { get; set; }

        public string GPA { get; set; }

        public string Certificate { get; set; }

        public int ConcidentStatus { get; set; }

        public int PositionID { get; set; }

        public virtual Position Position { get; set; }

        public virtual User InterviewAdmin { get; set; }

        public virtual ICollection<CandidateCertificate> CandidateCertificates { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; }

        public bool Active { get; set; }

        public Candidate()
        {
            Active = true;
            CandidateCertificates = new HashSet<CandidateCertificate>();
            Interviews = new HashSet<Interview>();
            Documents = new HashSet<Document>();
            CandidateSkills = new HashSet<CandidateSkill>();
        }
    }
}
