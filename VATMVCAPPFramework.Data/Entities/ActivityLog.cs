using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
    public class ActivityLog : Entity
    {

        public Int64? UserId { get; set; }

        public string ModuleName { get; set; }
        public string ModuleAction { get; set; }

        public string Description { get; set; }

        public string Record { get; set; }

    }
}
