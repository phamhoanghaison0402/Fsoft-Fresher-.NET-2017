using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class CandidateCertificate
    {
        [Key]
        [Column(Order = 1)]
        public int CandidateID { set; get; }

        [Key]
        [Column(Order = 2)]
        public int CertificateID { get; set; }

        public virtual Certificate Certificate { get; set; }

        public virtual Candidate Candidate { get; set; }

        public bool Active { get; set; }

        public CandidateCertificate()
        {
            Active = true;
        }
    }
}
