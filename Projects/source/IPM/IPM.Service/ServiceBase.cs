using IPM.Business;
using log4net;
using System.Collections.Generic;

namespace IPM.Service
{
    public interface IServiceBase<T> where T : class
    {
        /// <summary>
        /// Get all entity from database.
        /// </summary>
        /// <returns>List of entity</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get one instance of entity by id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        T GetSingleById(int id);

        /// <summary>
        /// Insert new entity into database.
        /// </summary>
        /// <param name="entity">New entity to be inserted</param>
        /// <returns>Entity</returns>
        T Insert(T entity);

        /// <summary>
        /// Update entity's information.
        /// </summary>
        /// <param name="entity">Entity with new information</param>
        void Update(T entity);

        /// <summary>
        /// Logical deleting entity, set active to false.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        T Delete(int id);

        /// <summary>
        /// Save changes to database.
        /// </summary>
        void Save();
    }

    public class ServiceBase<T> : IServiceBase<T> where T : class 
    {
        private readonly IBusinessBase<T> _business;
        protected readonly ILog log;

        protected ServiceBase(IBusinessBase<T> business)
        {
            _business = business;
            log = LogManager.GetLogger(GetType().Name);
        }

        /// <summary>
        /// Save changes to database.
        /// </summary>
        public void Save()
        {
            log.Info("Save");

            _business.Save();
        }

        /// <summary>
        /// Get all entity from database.
        /// </summary>
        /// <returns>List of entity</returns>
        public virtual IEnumerable<T> GetAll()
        {
            log.Info("GetAll");

            return _business.GetAll();
        }

        /// <summary>
        /// Get one instance of entity by id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        public virtual T GetSingleById(int id)
        {
            log.Info("GetSingle");

            return _business.GetSingleById(id);
        }

        /// <summary>
        /// Insert new entity into database.
        /// </summary>
        /// <param name="entity">New entity to be inserted</param>
        /// <returns>Entity</returns>
        public virtual T Insert(T entity)
        {
            log.Info("Insert");

            return _business.Insert(entity);
        }

        /// <summary>
        /// Update entity's information.
        /// </summary>
        /// <param name="entity">Entity with new information</param>
        public virtual void Update(T entity)
        {
            log.Info("Update");

            _business.Update(entity);
        }

        /// <summary>
        /// Logical deleting entity, set active to false.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        public virtual T Delete(int id)
        {
            log.Info("Delete");

            return _business.Delete(id);
        }
    }
}
