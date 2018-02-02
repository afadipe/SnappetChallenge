using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class ApplicationTableConfig : EntityTypeConfiguration<Application>
    {

        public ApplicationTableConfig()
        {


            this.ToTable(tableName: "Application");
            this.Property(m => m.ApplicationName).IsOptional();
            this.Property(m => m.Description).IsOptional();
            //Entity
            this.Property(m => m.HasAdminUserConfigured).IsRequired();
            this.Property(m => m.Description).IsRequired();
            this.Property(m => m.IsActive).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.DateCreated).IsRequired();

        }
    }
}
