using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.ViewModel
{
    public class EmailViewModel
    {
        public long EmailID { get; set; }

        [Display(Name = "Email Subject")]
        [Required]
        public string EmailSubject { get; set; }

        [Display(Name = "Email Code")]
        [Required]
        public string EmailCode { get; set; }

        [Display(Name = "Email Body")]
        [Required(ErrorMessage = "Enter email body")]
        [AllowHtml]
        public string EmailText { get; set; }

        public List<EmailToken> EmailToken { get; set; }
    }
}