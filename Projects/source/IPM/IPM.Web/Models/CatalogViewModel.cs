using IPM.Model.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPM.Web.Models
{
    public class CatalogViewModel
    {
        public int ID { set; get; }
        public string Name { get; set; }
        public int MaxPoint { get; set; }
        public string GuidelineName { get; set; }
        public bool Active { get; set; }
        public List<GuidelineCatalog> GuidelineCatalogs { get; set; }
    }
}