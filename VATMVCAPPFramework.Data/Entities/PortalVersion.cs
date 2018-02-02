using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
    public class PortalVersion : Entity
    {
        public string FrameworkName { get; set; }
        public string FrameworkVersion { get; set; }

        public string FrameworkDescription { get; set; }

        public string TargetServer { get; set; }

        public string DefaultDatabaseEngine { get; set; }

        public string PackagesUsed { get; set; }

        public string DevelopedBy { get; set; }

        public string UX { get; set; }

        public string IOC { get; set; }
        
    }
}
