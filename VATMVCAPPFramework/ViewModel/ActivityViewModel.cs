using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VATMVCAPPFramework.ViewModel
{
    public class ActivitlogSearchInfo
    {
        [Display(Name = "Controller")]
        public string SelectedController { get; set; }
      
        public IEnumerable<System.Web.Mvc.SelectListItem> Contollers { get; set; }

        [Display(Name = "Start Date")]
        public string SelectedStartDate { get; set; }

        [Display(Name = "End Date")]
        public string SelectedEndDate { get; set; }

        [Display(Name = "User")]
        public long? SelectedUser { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Users { get; set; }
    }
  
}
