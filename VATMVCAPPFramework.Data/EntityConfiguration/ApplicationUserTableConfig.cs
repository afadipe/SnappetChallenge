using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class ApplicationUserTableConfig
        : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserTableConfig()
        {
            ToTable("AspNetUsers");

            // Primary Key
            HasKey(t => t.Id);

           // Property(u => u.Id).IsRequired().HasColumnName("Id");
            this.Property(e => e.Id).IsRequired().HasColumnName("AspNetUserId");
            this.Property(m => m.FirstName).IsRequired();
            this.Property(m => m.MiddleName).IsOptional();
            this.Property(m => m.LastName).IsRequired();
            //this.Property(m => m.DOB).IsOptional();
            this.Property(m => m.MobileNumber).IsOptional();
            //this.Property(m => m.Address).IsOptional();
            this.Property(m => m.IsFirstLogin).IsRequired();
            this.Property(m => m.DateUpdated).IsOptional();
            this.Property(m => m.DateCreated).IsRequired();

            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("UserName")
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UserNameIndex")
                    {
                        IsUnique = true
                    }));
            Property(u => u.Email).IsRequired().HasMaxLength(256).HasColumnName("Email");
            Property(u => u.EmailConfirmed).IsRequired().HasColumnName("EmailConfirmed");
            Property(u => u.PasswordHash).HasColumnName("PasswordHash");
            Property(u => u.SecurityStamp).HasColumnName("SecurityStamp");
            Property(u => u.PhoneNumber).HasColumnName("PhoneNumber");
            Property(u => u.PhoneNumberConfirmed)
                .IsRequired()
                .HasColumnName("PhoneNumberConfirmed");
            Property(u => u.TwoFactorEnabled)
                .IsRequired()
                .HasColumnName("TwoFactorEnabled");
            Property(u => u.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            Property(u => u.LockoutEnabled).IsRequired().HasColumnName("LockoutEnabled");
            Property(u => u.AccessFailedCount)
                .IsRequired()
                .HasColumnName("AccessFailedCount");
            //AuditEntity
            Property(u => u.CreatedBy).IsOptional();
            //Property(u => u.DeletedBy).IsOptional();
            Property(u => u.UpdatedBy).IsOptional();
            Property(u => u.IsDeleted).IsRequired();
            Property(u => u.IsActive).IsRequired();
            Property(u => u.RowVersion).IsRequired();
            // Relationships
            HasMany(u => u.Roles)
                .WithRequired()
                .HasForeignKey(ur => ur.UserId)
                .WillCascadeOnDelete();
            HasMany(u => u.Claims)
                .WithRequired()
                .HasForeignKey(uc => uc.UserId)
                .WillCascadeOnDelete();
            HasMany(u => u.Logins)
                .WithRequired()
                .HasForeignKey(ul => ul.UserId)
                .WillCascadeOnDelete();
        }
    }
}