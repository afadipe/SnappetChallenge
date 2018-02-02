using VATMVCAPPFramework.Data.EntityContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
    public  class EmailTemplate : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Body { get; set; }
 
        
    }
}
