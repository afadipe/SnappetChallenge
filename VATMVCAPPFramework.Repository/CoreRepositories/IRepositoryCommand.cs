using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using VATMVCAPPFramework.Data.EntityContract;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Repository.CoreRepositories
{
    public interface IRepositoryCommand<TEntity> where TEntity : class, IEntity
    {
        #region SaveChanges
        void Save();
        int SaveChanges();
        Task<int> SaveChangesAsync();

        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        /// Int64
        Int64 InsertAndGetId(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Task<Int64> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Int64 InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// Inserts or updates given entity depending on Id's value.
        /// Also returns Id of the entity.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Task<Int64> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        TEntity Update(Int64 id, Action<TEntity> updateAction);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(Int64 id, Func<TEntity, Task> updateAction);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        Task DeleteAsync(TEntity entity);

        ///// <summary>
        ///// Deletes an entity by primary key.
        ///// </summary>
        ///// <param name="id">Primary key of the entity</param>
        //void Delete(TPrimaryKey id);

        ///// <summary>
        ///// Deletes an entity by primary key.
        ///// </summary>
        ///// <param name="id">Primary key of the entity</param>
        //Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Int64 id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(Int64 id);

        #endregion

        #region MyCommandRegion
        Task<TEntity> InsertOrUpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entity);
        #endregion

    }
}
