using System;
using System.ComponentModel.DataAnnotations;

namespace VATMVCAPPFramework.ViewModel
{
    public class PermissionViewModel
    {
        public Int64 PermissionId { get; set; }

        [Display(Name = "Permission Name")]
        public string PermissionName { get; set; }


        [Display(Name = "Permission Code")]
        public string PermissionCode { get; set; }
    }
}