using System;

namespace VATMVCAPPFramework.Data.EntityContract
{
    /// <summary>
    /// Defines interface for base entity type. All entities in the system must implement this interface.
    /// </summary>
    
    public interface IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        /// Int64
        Int64 Id { get; set; }

        DateTime DateCreated { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        bool IsTransient();

        /// <summary>
        /// To allow soft delete
        /// </summary>
        bool IsDeleted { get; set; }
        /// <summary>
        /// to mark entity as active or inactive
        /// </summary>
        bool IsActive { get; set; }

    }
}