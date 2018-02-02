using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
   
    public partial class EmailAttachment : Entity
    {

        public long EmailLogID { get; set; }

        [Required]
        public string FilePath { get; set; }

        [StringLength(50)]
        public string FileTitle { get; set; }

        public virtual EmailLog EmailLog { get; set; }
    }
}
