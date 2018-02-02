using System.Threading.Tasks;
using System;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Data.Entities;
using VATMVCAPPFramework.Repository.CoreRepositories;

namespace VATMVCAPPFramework.Repository
{
    public interface IActivityLogRepositoryCommand : IAutoDependencyRegister 
    {
        //: IRepository<ActivityLog>
        Task CreateActivityLogAsync(string descriptn, string controllerName, string actionName, Int64 userid, object record);
        void CreateActivityLog(string descriptn, string controllerName, string actionNAme, Int64 userid, object record);
    }
}
