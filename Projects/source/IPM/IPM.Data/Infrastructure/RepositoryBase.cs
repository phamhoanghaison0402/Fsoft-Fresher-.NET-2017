using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace IPM.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Marks an entity as new
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);

        /// <summary>
        /// Marks an entity as modified
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);

        /// <summary>
        /// Marks an entity to be removed
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Delete(T entity);

        /// <summary>
        /// Delete folowing id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Delete(int id);

        /// <summary>
        /// Delete multi records
        /// </summary>
        /// <param name="where"></param>
        void DeleteMulti(Expression<Func<T, bool>> where);

        /// <summary>
        /// Get an entity by int id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetSingleById(int id);

        /// <summary>
        /// Get Single By Condition
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(string[] includes = null);

        //IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        /// <summary>
        /// Get multi paging
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="total"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> where);

        /// <summary>
        /// Check Contains
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool CheckContains(Expression<Func<T, bool>> predicate);
    }

    /// <summary>
    /// RepositoryBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region Properties

        private IPMDbContext dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }

        protected IPMDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        #endregion Properties

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        #region Implementation

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Update(T entity)
        {
            var result = dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            return result;
        }

        /// <summary>
        /// Delete flowing entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete folowing id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Delete(int id)
        {
            var entity = dbSet.Find(id);
            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete multi
        /// </summary>
        /// <param name="where"></param>
        public virtual void DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Get Single By Condition
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Any())
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return dataContext.Set<T>().FirstOrDefault(expression);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Any())
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return dataContext.Set<T>().AsQueryable();
        }

        /// <summary>
        /// Get Multi
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        /// <summary>
        /// Get MultiPaging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="total"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 20, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Count(where);
        }

        /// <summary>
        /// Check Contains
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Count<T>(predicate) > 0;
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        #endregion Implementation
    }
}