using System.Collections.Generic;

namespace IPM.Model.Models
{
    public class Role
    {
        public int ID { set; get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public bool Active { get; set; }

        public Role()
        {
            Active = true;
            UserRoles = new HashSet<UserRole>();
        }
    }
}
