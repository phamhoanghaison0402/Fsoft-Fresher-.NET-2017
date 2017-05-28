using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class InterviewAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int InterviewID { get; set; }

        public int CatalogID { get; set; }

        public int Mark { get; set; }

        [ForeignKey("InterviewID")]
        public virtual Interview Interview { get; set; }

        [ForeignKey("CatalogID")]
        public virtual Catalog Catalog { get; set; }

        public virtual ICollection<AnswerQuestion> AnswerQuestions { get; set; }

        public bool Active { get; set; }

        public InterviewAnswer()
        {
            Mark = 1;
            Active = true;
            AnswerQuestions = new HashSet<AnswerQuestion>();
        }
    }
}
