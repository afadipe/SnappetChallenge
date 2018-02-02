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
    public interface IRepositoryQuery<TEntity> where TEntity : class,IEntity
    {

        #region Select/Get/Query

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// <see>
        ///     <cref>UnitOfWorkAttribute</cref>
        /// </see>
        ///     attribute must be used to be able to call this method since this method
        /// returns IQueryable and it requires open database connection to use it.
        /// </summary>
        /// <returns>
        /// <see cref="IQueryable{TEntity}"/> to be used to select entities from database 
        /// 
        /// </returns>
        IQueryable<TEntity> GetAll();



        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>
        /// <see cref="List{TEntity}"/>
        /// List of all entities</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// Used to get all entities Async.
        /// </summary>
        /// <returns>List of all entities Async</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to run a query over entire entities.
        /// <see>
        ///     <cref>UnitOfWorkAttribute</cref>
        /// </see>
        ///     attribute is not always necessary (as opposite to <see cref="GetAll"/>)
        /// if <paramref name="queryMethod"/> finishes IQueryable with TEntityoList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="TEntity">TEntityype of return value of this method</typeparam>
        /// <param name="queryMethod">TEntityhis method is used to query over entities</param>
        /// <returns>Query result</returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity <see cref="TEntity"/></returns>
        TEntity Get(Int64 id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity <see cref="TEntityask{TEntity}"/></returns>
        Task<TEntity> GetAsync(Int64 id);
       
        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// TEntityhrows exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">Entity <see cref="TEntity"/></param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// TEntityhrows exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">Entity</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        TEntity FirstOrDefault(Int64 id);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(Int64 id);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Creates an entity with given primary key without database access.
        /// </summary>
        /// <param name="id">Primary key of the entity to load</param>
        /// <returns>Entity</returns>
        TEntity Load(Int64 id);

        #endregion
        #region Aggregates

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        int Count();

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greather than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>Count of entities</returns>
        long LongCount();

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region MyStoredProdureRegion
        DbRawSqlQuery<T> ExecuteStoredProdure<T>(string storedProcedureNameAndParameterPlaceholder, params object[] parameters);

        void ExecuteStoreprocedure(string storedProcedureNameAndParameterPlaceholder, params object[] parameters);
        DbRawSqlQuery<T> StoreprocedureQueryFor<T>(string storeprocedureName, params object[] parameters);

        DbRawSqlQuery<T> StoreprocedureQuery<T>(string storeprocedureName);
        #endregion

    }
}
