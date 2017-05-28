using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    /// <summary>
    /// Interview process view model
    /// </summary>
    public class InterviewProcessViewModel
    {
        public int ID { get; set; }
        public string ProcessName { get; set; }
        public int PositionID { get; set; }
        public bool Active { get; set; }
        public DateTime StartDate { get; set; }

    }
}