using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class AnswerQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int QuestionID { get; set; }

        public int InterviewAnswerID { get; set; }

        public string CandidateAnswer { get; set; }

        public string Comment { get; set; }

        [ForeignKey("InterviewAnswerID")]
        public virtual InterviewAnswer InterviewAnswer { get; set; }
        
        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        public bool Active { get; set; }

        public AnswerQuestion()
        {
            Active = true;
        }
    }
}
