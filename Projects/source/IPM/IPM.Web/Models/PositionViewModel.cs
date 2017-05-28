using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    /// <summary>
    /// class position view model
    /// </summary>
    public class PositionViewModel
    {
        public int ID { set; get; }

        [Display(Name = "PositionName", ResourceType = typeof(IPM.Web.MultiLanguage.Resource))]
        [Required(ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), ErrorMessageResourceName = "PositionNameRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), ErrorMessageResourceName = "NameRange")]

        public string Code { get; set; }
        public string Name { get; set; }
 

        public bool Active { get; set; }
    }
}