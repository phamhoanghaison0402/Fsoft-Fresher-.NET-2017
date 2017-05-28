using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class SkillPageViewModel
    {
        public IPagedList<SkillViewModel> SkillList { get; set; }
        public SkillViewModel Skill { get; set; }
    }
}