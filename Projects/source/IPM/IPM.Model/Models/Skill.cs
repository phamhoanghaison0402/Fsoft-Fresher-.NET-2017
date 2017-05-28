using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    /// <summary>
    /// Skill entity
    /// </summary>
    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<CandidateSkill> CandidateSkills { get; set; }

        public bool Active { get; set; }

        public Skill()
        {
            Active = true;
            Questions = new HashSet<Question>();
            CandidateSkills = new HashSet<CandidateSkill>();
        }
    }
}
