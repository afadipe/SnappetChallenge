using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{

    public class PermissionTableConfig : EntityTypeConfiguration<Permission>
    {

        public PermissionTableConfig() {


            this.ToTable(tableName: "Permission");
            this.Property(m => m.Name).IsRequired();
            this.Property(m => m.Code).IsRequired();
            //Basentity
            this.Property(m => m.CreatedBy).IsRequired();
            this.Property(m => m.UpdatedBy).IsOptional();
            this.Property(m => m.IsActive).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.DateCreated).IsRequired();
            this.Property(m => m.DateUpdated).IsOptional();
            this.Property(m => m.RowVersion).IsRequired();

            //HasRequired(t => t.ApplicationUserCreated).WithMany(t => t.PermissionCreatedBy).HasForeignKey(d => d.CreatedBy);
            //HasOptional(t => t.ApplicationUserModified).WithMany(t => t.PermissionModifiedBy).HasForeignKey(d => d.UpdatedBy);

        }
    }
}
