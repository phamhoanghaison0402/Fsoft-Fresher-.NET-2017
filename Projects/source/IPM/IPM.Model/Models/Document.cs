namespace IPM.Model.Models
{
    public class Document
    {
        public int ID { set; get; }

        public int CandidateID { set; get; }

        public string Name { get; set; }

        public string Path { get; set; }

        public virtual Candidate Candidate { get; set; }

        public bool Active { get; set; }

        public Document()
        {
            Active = true;
        }
    }
}
