using System.Collections.Generic;

namespace IPM.Model.Models
{
    public class Certificate
    {
        public int ID { set; get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<CandidateCertificate> CandidateCertificates { get; set; }

        public bool Active { get; set; }

        public Certificate()
        {
            Active = true;
            CandidateCertificates = new HashSet<CandidateCertificate>();
        }
    }
}
