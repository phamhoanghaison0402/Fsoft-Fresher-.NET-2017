using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class CalendarEventItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Location { get; set; }
        public string Body { get; set; }
        public string Address { get; set; }
    }
}