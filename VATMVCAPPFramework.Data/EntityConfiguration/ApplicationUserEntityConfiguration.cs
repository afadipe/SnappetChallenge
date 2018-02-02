using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VATMVCAPPFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{
    public class ApplicationUserEntityConfiguration :EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserEntityConfiguration()
        {
           this.ToTable(tableName: "AspNetUser");
           this.Property(e => e.Id).HasColumnName("AspNetUserId");
           this.Property(m => m.MiddleName).IsOptional();
           //this.Property(m => m.DOB).IsOptional();
           this.Property(m => m.PhoneNumber).IsOptional();
           this.Property(m => m.DateUpdated).IsOptional();
        }
    }
    public class ApplicationRoleEntityConfiguration : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleEntityConfiguration()
        {
            this.ToTable(tableName: "AspNetRole");
            this.Property(e => e.Id).HasColumnName("AspNetRoleId");
        }
    }

    public class ApplicationUserClaimEntityConfiguration : EntityTypeConfiguration<ApplicationUserClaim>
    {
        public ApplicationUserClaimEntityConfiguration()
        {
            this.ToTable(tableName: "AspNetUserClaim");
            this.Property(e => e.UserId).HasColumnName("AspNetUserId");
             this.Property(e => e.Id).HasColumnName("AspNetUserClaimId");
        }
    }

    public class ApplicationUserLoginEntityConfiguration : EntityTypeConfiguration<ApplicationUserLogin>
    {
        public ApplicationUserLoginEntityConfiguration()
        {
            this.ToTable(tableName: "AspNetUserLogin");
            this.Property(e => e.UserId).HasColumnName("AspNetUserId");
        }
    }

    public class ApplicationUserRoleEntityConfiguration : EntityTypeConfiguration<ApplicationUserRole>
    {
        public ApplicationUserRoleEntityConfiguration()
        {
            this.ToTable(tableName: "AspNetUserRole");
            this.Property(e => e.UserId).HasColumnName("AspNetUserId");
            this.Property(e => e.RoleId).HasColumnName("AspNetRoleId");
        }
    }



}