using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    /// <summary>
    /// Meeting Request
    /// </summary>
    public class MeetingRequest
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string InterviewerEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public int RoomID { get; set; }

        public string EmailContent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string InterviewAdmin { get; set; }

        public string AppointmentID { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room Room { set; get; }

        [Required]
        public bool Active { get; set; }

        public MeetingRequest()
        {
            Active = true;
        }
    }
}
