using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class CandidateSkill
    {
        [Key]
        [Column(Order = 1)]
        public int CandidateID { set; get; }

        [Key]
        [Column(Order = 2)]
        public int SkillID { set; get; }

        public virtual Candidate Candidate { get; set; }

        public virtual Skill Skill { get; set; }

        public bool Active { set; get; }

        public CandidateSkill()
        {
            Active = true;
        }
    }
}
