using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Data.ModelReport
{
    public class ReportCandidates
    {
        public int ID { get; set; }

        public string No { get; set; }

        public string Name { get; set; }

        public int InterviewAdminID { get; set; }

        public int PositionID { get; set; }

        public string FullName { get; set; }

        public DateTime Birthdate { get; set; }

        public string IDCard { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string University { get; set; }

        public string Major { get; set; }

        public string GPA { get; set; }

        public int ConcidentStatus { get; set; }

        public bool Active { get; set; }

        public string Certificate { get; set; }

        public List<CandidateCertificate> CandidateGSTs { get; set; }

        public List<Interview> Interviews { get; set; }

        public User InterviewAdmin { get; set; }

        public Position Position { get; set; }

        public List<CandidateSkill> CandidateSkills { get; set; }

        public List<Document> Documents { get; set; }

        public int Result { get; set; }

        public DateTime Date { get; set; }
    }
}
