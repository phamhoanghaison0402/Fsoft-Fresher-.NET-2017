using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IPM.Model.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        public virtual ICollection<MeetingRequest> MeetingRequests { set; get; }

        public bool Active { get; set; }

        public Room()
        {
            Active = true;
            MeetingRequests = new HashSet<MeetingRequest>();
        }
    }
}
