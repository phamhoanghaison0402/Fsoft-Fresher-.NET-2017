using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class Guideline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<InterviewRound> InterviewRounds { get; set; }

        public virtual ICollection<GuidelineCatalog> GuidelineCatalog { get; set; }

        public bool Active { get; set; }

        public Guideline()
        {
            Active = true;
            InterviewRounds = new HashSet<InterviewRound>();
            GuidelineCatalog = new HashSet<GuidelineCatalog>();
        }
    }
}
