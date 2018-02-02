using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class PortalVersionTableConfig:EntityTypeConfiguration<PortalVersion>
    {
        public PortalVersionTableConfig()
        {


            this.ToTable(tableName: "PortalVersion");
            this.Property(m => m.DateCreated).IsRequired();
            this.Property(m => m.FrameworkDescription).IsRequired();
            //Entity
            this.Property(m => m.FrameworkName).IsRequired();
            this.Property(m => m.FrameworkVersion).IsRequired();
            this.Property(m => m.IOC).IsRequired();
            this.Property(m => m.TargetServer).IsRequired();
            this.Property(m => m.DefaultDatabaseEngine).IsRequired();
            this.Property(m => m.PackagesUsed).IsRequired();
            this.Property(m => m.DevelopedBy).IsRequired();
            this.Property(m => m.UX).IsRequired();
            this.Property(m => m.IsActive).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.DateCreated).IsRequired();

        }
    }
}
