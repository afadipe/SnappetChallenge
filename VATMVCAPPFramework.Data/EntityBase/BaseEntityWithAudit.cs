using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VATMVCAPPFramework.Data.EntityContract;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityBase
{
    public class BaseEntityWithAudit : IAduit
    {
        public BaseEntityWithAudit()
        {
            IsActive = true;
            DateCreated=DateTime.Now;
            IsDeleted = false;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Int64 CreatedBy { get; set; }
        public Int64? UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; } 

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public bool IsTransient()
        {
            return EqualityComparer<Int64>.Default.Equals(Id, default(Int64));
            
        }
    }
}