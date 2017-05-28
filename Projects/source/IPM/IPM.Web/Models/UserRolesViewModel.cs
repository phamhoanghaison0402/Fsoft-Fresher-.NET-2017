using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class UserRolesViewModel
    {
        public string Account { get; set; }
       
        public int[] SelectRole { get; set; }

        public string RoleName { get; set; }

        public int RoleID { get; set; }
    }
}