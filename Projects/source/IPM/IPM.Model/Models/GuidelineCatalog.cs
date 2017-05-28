using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class GuidelineCatalog
    {
        [Key]
        public int ID { get; set; }

        public int GuidelineID { get; set; }

        public int CatalogID { get; set; }

        [ForeignKey("GuidelineID")]
        public virtual Guideline Guideline { get; set; }

        [ForeignKey("CatalogID")]
        public virtual Catalog Catalog { get; set; }

        public bool Active { get; set; }

        public GuidelineCatalog()
        {
            Active = true;
        }
    }
}
