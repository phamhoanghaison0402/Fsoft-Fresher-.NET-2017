using System.ComponentModel.DataAnnotations;

namespace IPM.Web.Models
{
    public class SkillViewModel
    {
        public int ID { set; get; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}