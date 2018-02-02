using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
     public class RolePermissionTableConfig : EntityTypeConfiguration<RolePermission>
    {

        public RolePermissionTableConfig()
        {
            this.ToTable(tableName: "RolePermission");
            this.Property(m => m.RoleId).IsRequired();
            this.Property(m => m.PermissionId).IsRequired();
            //Basentity
            this.Property(m => m.CreatedBy).IsRequired();
            this.Property(m => m.UpdatedBy).IsOptional();
            this.Property(m => m.IsActive).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.DateCreated).IsRequired();
            this.Property(m => m.DateUpdated).IsOptional();
            this.Property(m => m.RowVersion).IsRequired();

            //HasRequired(t => t.ApplicationUserCreated).WithMany(t => t.RolePermissionCreatedBy).HasForeignKey(d => d.CreatedBy);
            //HasOptional(t => t.ApplicationUserModified).WithMany(t => t.RolePermissionModifiedBy).HasForeignKey(d => d.UpdatedBy);

            //HasOptional(t => t.ApplicationRole).WithMany(t => t.RolePermissions).HasForeignKey(d => d.RoleId);
            //HasOptional(t => t.Permission).WithMany(t => t.RolePermissions).HasForeignKey(d => d.PermissionId);
        }
    }
}
