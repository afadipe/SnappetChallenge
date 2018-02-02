using System;
using System.Collections.Generic;
using VATMVCAPPFramework.Data.EntityContract;

namespace VATMVCAPPFramework.Data.EntityBase
{
    public class Entity : IEntity
    {
        public Entity()
        {
            IsActive = true;
            IsDeleted = false;
            DateCreated = DateTime.Now;
        }

        public Int64 Id { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsTransient()
        {
            return EqualityComparer<Int64>.Default.Equals(Id, default(Int64));
        }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}