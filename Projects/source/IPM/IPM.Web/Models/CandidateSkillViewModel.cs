using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IPM.Model.Models;

namespace IPM.Web.Models
{
    public class CandidateSkillViewModel
    {
        public virtual Candidate Candidate { get; set; }

        public virtual Skill Skill { get; set; }

        public bool Active { set; get; }

        public IEnumerable<int> SkillIds { get; set; }

        public IEnumerable<Skill> CandidateSkills { get; set; }
    }
}