using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class CatalogQuestion
    {
        [Key]
        public int ID { get; set; }

        public int CatalogID { get; set; }

        public int QuestionID { get; set; }

        [ForeignKey("CatalogID")]
        public virtual Catalog Catalog { get; set; }

        [ForeignKey("QuestionID")]
        public virtual Question Question { get; set; }

        public bool Active { get; set; }

        public CatalogQuestion()
        {
            Active = true;
        }
    }
}
