using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class FormReportViewModel
    {
        //Controller-View
        public int ID { get; set; }
        public string No { get; set; }
        public int Result { get; set; }
        public DateTime Date { get; set; }

        //Controller-View - View-Model
        public string Name { get; set; }
        public string Certificate { get; set; }

        //View-Model
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string InterviewResult { get; set; }

        public String Position { get; set; }
        public int PositionID { get; set; }
        public String Skills { get; set; }
        public DateTime StartDate { get; set; }
    }
}