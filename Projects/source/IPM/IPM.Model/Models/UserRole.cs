using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPM.Model.Models
{
    public class UserRole
    {
        [Key]
        [Column(Order = 1)]
        public string Account { set; get; }

        [Key]
        [Column(Order = 2)]
        public int RoleID { get; set; }

        public virtual Role Role { set; get; }

        public virtual User User { get; set; }
    }
}
