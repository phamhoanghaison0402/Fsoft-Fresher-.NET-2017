using System.ComponentModel.DataAnnotations;

namespace IPM.Web.Models
{
    public class GuidelineViewModel
    {
        public int ID { set; get; }

        [Display(Name = "GuidelineName", ResourceType = typeof(IPM.Web.MultiLanguage.Resource))]
        [Required(ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), ErrorMessageResourceName = "GuidelineNameRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), ErrorMessageResourceName = "NameRange")]
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}