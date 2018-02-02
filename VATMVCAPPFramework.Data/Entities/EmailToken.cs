using VATMVCAPPFramework.Data.EntityContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.Entities
{
   public class EmailToken : Entity
    {
        public string EmailCode { get; set; }
        public string Token { get; set; }
        public string PreviewText { get; set; }

       
    }
}
