using IPM.Model.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPM.Web.Models
{
    public class QuestionViewModel
    {
        public int ID { set; get; }
        
        public int SkillID { get; set; }

        public string SkillName { get; set; }

        public string Content { get; set; }

        public string Answer { get; set; }

        public bool Active { get; set; }

       
    }
}