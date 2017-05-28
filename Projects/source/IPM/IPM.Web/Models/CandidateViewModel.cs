using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IPM.Web.Models
{
    public class CandidateViewModel
    {
        public int ID { set; get; }

        public int? InterviewAdminID { get; set; }

        [Required(ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidateNameRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateNameLengh")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidateEmailRequired")]
        [RegularExpression(@"^[a-z0-9](\.?[a-z0-9_-]){0,}@[a-z0-9-]+\.([a-z]{1,6}\.)?[a-z]{2,6}$", 
            ErrorMessageResourceType = typeof(IPM.Web.MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidateEmailInvalid")]
        [StringLength(50, ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateEmailLengh")]
        public string Email { get; set; }

        public DateTime Birthdate { get; set; }

        [RegularExpression(@"^[0-9\-\+]{9}$", ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateIdCardInvalid")]
        public string IDCard { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateAddressLengh")]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidatePhoneRequired")]
        [RegularExpression(@"^[0-9\-\+]{10,11}$", ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidatePhoneInvalid")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidateUniversityRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateUniversityLengh")]
        public string University { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(MultiLanguage.Resource),
            ErrorMessageResourceName = "CandidateMajorLengh")]
        public string Major { get; set; }

        public string GPA { get; set; }

       // public string Certificate { get; set; }

        public int ConcidentStatus { get; set; } = 0;

        [Required(ErrorMessageResourceType = typeof(MultiLanguage.Resource), 
            ErrorMessageResourceName = "CandidatePositionRequired")]
        public int PositionID { get; set; }

        public Position Position { get; set; }

        public User InterviewAdmin { get; set; }

        public bool Active { get; set; } = true;

        public bool Result { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        public IEnumerable<Document> Documents { get; set; }

        public IEnumerable<User> InterviewAdmins { get; set; }

        public IEnumerable<Skill> ListSkill { get; set; }

        public Skill Skill { get; set; }

        public IEnumerable<int> SelectSkillID { get; set; }

        public IEnumerable<int> SelectCertificateId { get; set; }

        public string SkillName { get; set; }

        public IEnumerable<Position> Positions { get; set; }

        public HttpPostedFileBase[] UploadedFiles { get; set; }

    }
}