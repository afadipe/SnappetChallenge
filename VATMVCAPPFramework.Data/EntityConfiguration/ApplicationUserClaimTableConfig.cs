using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class ApplicationUserClaimTableConfig : EntityTypeConfiguration<ApplicationUserClaim>
    {
        public ApplicationUserClaimTableConfig()
        {
           // ToTable("AspNetUserClaims");
            this.ToTable(tableName: "AspNetUserClaim");
            this.Property(e => e.UserId).IsRequired().HasColumnName("AspNetUserId");
            this.Property(e => e.Id).IsRequired().HasColumnName("AspNetUserClaimId");

            // Primary Key
            HasKey(c => new {c.Id, c.UserId});

            //Property(c => c.Id).IsRequired().HasColumnName("Id");
            //Property(c => c.UserId).IsRequired().HasColumnName("UserId");
            Property(c => c.ClaimType).HasColumnName("ClaimType");
            Property(c => c.ClaimValue).HasColumnName("ClaimValue");
        }
    }
}