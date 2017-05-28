using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class PositionCandidateViewModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DateTime Birthday { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string University { set; get; }
        public string PositionName { set; get; }
    }
}