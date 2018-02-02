using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.Data.EntityConfiguration
{

    public class ActivityLogTableConfig : EntityTypeConfiguration<ActivityLog>
    {

        public ActivityLogTableConfig()
        {


            this.ToTable(tableName: "ActivityLog");
            this.Property(m => m.UserId).IsOptional();
            this.Property(m => m.Record).IsOptional();
            //Entity
            this.Property(m => m.ModuleAction).IsRequired();
            this.Property(m => m.ModuleName).IsRequired();
            this.Property(m => m.Description).IsRequired();
            this.Property(m => m.IsActive).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.DateCreated).IsRequired();

        }
    }
}
