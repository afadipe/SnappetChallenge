using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class ApplicationUserLoginTableConfig
        : EntityTypeConfiguration<ApplicationUserLogin>
    {
        public ApplicationUserLoginTableConfig()
        {
           // ToTable("AspNetUserLogins");
            this.ToTable(tableName: "AspNetUserLogin");
            // Primary Key
            HasKey(l => new {l.LoginProvider, l.ProviderKey, l.UserId});

           
            this.Property(e => e.UserId).IsRequired().HasColumnName("AspNetUserId");

           // Property(l => l.UserId).IsRequired();
            Property(l => l.LoginProvider)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("LoginProvider");
            Property(l => l.ProviderKey)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("ProviderKey");
        }
    }
}