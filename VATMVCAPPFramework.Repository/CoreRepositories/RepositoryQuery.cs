using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VATMVCAPPFramework.Data;
using VATMVCAPPFramework.Data.EntityContract;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Repository.CoreRepositories
{
    public class RepositoryQuery<TEntity> : IRepositoryQuery<TEntity> where TEntity : class, IEntity
    {
        private APPContext _context;
        public RepositoryQuery(APPContext context)
        {
            this._context = context;
        }

        public RepositoryQuery()
        {
            this._context = new APPContext();
        }
        /// <summary>
        /// get the current Entity
        /// <returns><see cref="DbSet{TEntity}"/></returns>
        /// </summary>
        public virtual DbSet<TEntity> Table
        {
            get { return _context.Set<TEntity>(); }
        }

        /// <summary>
        /// get the entity <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <returns></returns>
        public  IQueryable<TEntity> GetAll()
        {
            return Table.Where(x => !x.IsDeleted);
           // return Table.Where(x => !x.IsDeleted && x.IsActive);
        }



        public virtual List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }


        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(GetAll());
        }

        public virtual TEntity Get(Int64 id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                throw new Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(Int64 id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return entity;
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity FirstOrDefault(Int64 id)
        {
            return GetAll().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Int64 id)
        {
            return Task.FromResult(FirstOrDefault(id));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }
        public virtual TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LastOrDefault(predicate);
        }

        public virtual Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LastOrDefault(predicate));
        }
        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual TEntity Load(Int64 id)
        {
            return Get(id);
        }


        public virtual int Count()
        {
            return GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }

        public virtual long LongCount()
        {
            return GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(LongCount(predicate));
        }
 
        #region MyStoredProdureRegion
        public DbRawSqlQuery<T> ExecuteStoredProdure<T>(string storedProcedureNameAndParameterPlaceholder, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>("EXEC " + storedProcedureNameAndParameterPlaceholder, parameters);
        }

        public void ExecuteStoreprocedure(string storedProcedureNameAndParameterPlaceholder, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand("EXEC " + storedProcedureNameAndParameterPlaceholder, parameters);
        }

        public DbRawSqlQuery<T> StoreprocedureQueryFor<T>(string storeprocedureName, params object[] parameters)
        {
            return _context.Database.SqlQuery<T>("EXEC " + storeprocedureName, parameters);
        }

        public DbRawSqlQuery<T> StoreprocedureQuery<T>(string storeprocedureName)
        {
            return _context.Database.SqlQuery<T>("EXEC " + storeprocedureName);
        }
        #endregion

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(Int64 id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(Int64))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

    }
}