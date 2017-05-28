using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    /// <summary>
    /// class position
    /// </summary>
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public string Name { get; set; }

        public string Code { get; set; }

        public virtual ICollection<InterviewProcess> InterviewProcesses { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }

        public bool Active { get; set; }

        public Position()
        {
            Active = true;
            InterviewProcesses = new HashSet<InterviewProcess>();
            Candidates = new HashSet<Candidate>();
        }
    }
}
