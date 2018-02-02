using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VATMVCAPPFramework.ViewModel
{
    public class CurrentUserViewModel
    {
        public long? AspNetUserId { get; set; }

        public string FullName { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }

        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }


    }
}