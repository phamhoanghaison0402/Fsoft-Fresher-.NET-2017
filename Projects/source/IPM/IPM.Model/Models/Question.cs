using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int SkillID { get; set; }

        public string Content { get; set; }

        public string Answer { get; set; }

        [ForeignKey("SkillID")]
        public virtual Skill Skill { get; set; }

        public virtual ICollection<AnswerQuestion> AnswerQuestions { get; set; }

        public virtual ICollection<CatalogQuestion> CatalogQuestions { get; set; }

        public bool Active { get; set; }

        public Question()
        {
            Active = true;
            AnswerQuestions = new HashSet<AnswerQuestion>();
            CatalogQuestions = new HashSet<CatalogQuestion>();
        }
    }
}
