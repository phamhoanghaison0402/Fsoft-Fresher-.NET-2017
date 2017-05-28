using IPM.Data.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    public interface IBusinessBase<T> where T : class 
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

    public abstract class BusinessBase<T> : IBusinessBase<T> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILog log;

        protected BusinessBase(IRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            log = LogManager.GetLogger(GetType().Name);
        }

        /// <summary>
        /// Save changes to database.
        /// </summary>
        public void Save()
        {
            log.Info("Save");

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                log.Error($"Commit error. {ex.Message}");
                //throw new CommitException("Commit error.", ex);
            }
        }

        /// <summary>
        /// Get all entity from database.
        /// </summary>
        /// <returns>List of entity</returns>
        public virtual IEnumerable<T> GetAll()
        {
            log.Info("GetAll");

            return _repository.Get();
        }

        /// <summary>
        /// Get one instance of entity by id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        public virtual T GetSingleById(int id)
        {
            log.Info("GetSingle");

            return _repository.GetSingleById(id);
        }

        /// <summary>
        /// Insert new entity into database.
        /// </summary>
        /// <param name="entity">New entity to be inserted</param>
        /// <returns>Entity</returns>
        public virtual T Insert(T entity)
        {
            log.Info("Insert");

            return _repository.Add(entity);
        }

        /// <summary>
        /// Update entity's information.
        /// </summary>
        /// <param name="entity">Entity with new information</param>
        public virtual void Update(T entity)
        {
            log.Info("Update");

            _repository.Update(entity);
        }

        /// <summary>
        /// Logical deleting entity, set active to false.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        public virtual T Delete(int id)
        {
            log.Info("Delete");

            return _repository.Delete(id);
        }
    }
}
