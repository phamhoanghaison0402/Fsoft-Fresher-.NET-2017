using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IPM.Model.Models;

namespace IPM.Web.Models
{
    public class CertificateViewModel
    {
        public int ID { set; get; }

        public string Name { get; set; }

        public bool Active { get; set; }

    }
}