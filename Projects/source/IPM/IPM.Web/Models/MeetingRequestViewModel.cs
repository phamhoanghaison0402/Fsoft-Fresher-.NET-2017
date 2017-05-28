using IPM.Model.Models;
using System;

namespace IPM.Web.Models
{
    /// <summary>
    /// Meeting Request View Model
    /// </summary>
    public class MeetingRequestViewModel
    {
        public int ID { get; set; }
        public string InterviewerEmail { get; set; }
        public string Subject { get; set; }
        public int RoomID { get; set; }
        public string EmailContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String InterviewAdmin { get; set; }
        public String AppointmentID { get; set; }
        public bool Active { get; set; }
        public virtual Room Room { set; get; }
    }
}