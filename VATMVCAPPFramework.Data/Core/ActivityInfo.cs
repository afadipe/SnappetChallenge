using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Data.Core
{

    public enum SelectSessionEnum
    {
        [Display(Name = "Apply To All Programme")]
        ApplyToAllProgramme = 1,
        [Display(Name = "Apply To Sepcify Programme")]
        ApplyToSepcifyProgramme
    }
    public class ActivityInfo
    {
        public Int64 Id { get; set; }
        public string FullName { get; set; }
        public Int64? UserID { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }
        public string Record { get; set; }
    }
}
