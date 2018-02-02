using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
    public partial class EmailLog : Entity
    {
        public EmailLog()
        {
            EmailAttachments = new HashSet<EmailAttachment>();
        }
        [Required]
        [StringLength(1000)]
        public string Sender { get; set; }

        [Required]
        [StringLength(1000)]
        public string Receiver { get; set; }
        
        [StringLength(1000)]
        public string CC { get; set; }
        
        [StringLength(1000)]
        public string BCC { get; set; }

        [Required]
        [StringLength(1000)]
        public string Subject { get; set; }

        [Required]
        public string MessageBody { get; set; }

        public bool HasAttachment { get; set; }

        public bool IsSent { get; set; }

        public int Retires { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime DateToSend { get; set; }

        public virtual ICollection<EmailAttachment> EmailAttachments { get; set; }
    }
}
