using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class Catalog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public int MaxPoint { get; set; }

        public virtual ICollection<GuidelineCatalog> GuidelineCatalogs { get; set; }

        public virtual ICollection<CatalogQuestion> CatalogQuestions { get; set; }

        public bool Active { get; set; }

        public Catalog()
        {
            Active = true;
            GuidelineCatalogs = new HashSet<GuidelineCatalog>();
            CatalogQuestions = new HashSet<CatalogQuestion>();
        }
    }
}
