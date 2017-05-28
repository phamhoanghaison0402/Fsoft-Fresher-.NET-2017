using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IPM.Web.Models
{
    public class UserViewModel
    {


        public int[] SelectRoleID { get; set; }

        [Required(ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), ErrorMessageResourceName = "AccountRequired")]
        public string Account { get; set; }

        public string Username { get; set; }

        public string RoleName { get; set; }

        public bool Active { get; set; }

        public int Actives { get; set; }
    }
}