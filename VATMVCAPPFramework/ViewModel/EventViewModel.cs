using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VATMVCAPPFramework.ViewModel
{
    public class EventViewModel
    {

        public string title { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public DateTime StartDate2 { get; set; }

        public DateTime EndDate2 { get; set; }

        public string Venue { get; set; }

        public string Description { get; set; }

        [Display(Name= "Session")]
        public long SessionID { get; set; }

        public string SessionName { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Sessions { get; set; }

        [Display(Name = "Term")]
        public long TermID { get; set; }

        public string TermName { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> Terms { get; set; }
    }



}