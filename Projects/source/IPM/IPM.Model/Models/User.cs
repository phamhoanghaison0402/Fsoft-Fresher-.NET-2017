using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPM.Model.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Account { get; set; }

        public string Username { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Interview> InterviewAdminsInterviews { get; set; }

        public virtual ICollection<Interview> InterviewersInterviews { get; set; }

        public virtual ICollection<MeetingRequest> MeetingRequests { set; get; }

        public bool Active { get; set; }

        public User()
        {
            Active = true;
            UserRoles = new HashSet<UserRole>();
            InterviewAdminsInterviews = new HashSet<Interview>();
            InterviewersInterviews = new HashSet<Interview>();
            MeetingRequests = new HashSet<MeetingRequest>();
        }
    }
}
