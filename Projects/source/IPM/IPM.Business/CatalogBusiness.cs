using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    /// <summary>
    /// Interface of Catalog
    /// </summary>
    public interface ICatalogBusiness : IBusinessBase<Catalog>
    {
        /// <summary>
        /// Get all Catalog by Guideline ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetAll(int id);

        bool ChangeActive(int id);

        IEnumerable<CatalogQuestion> GetQuestion(int idcatalog);

        IEnumerable<Question> AvailableQuestion(int idcatalog);
    }

    /// <summary>
    ///Class of Catalog implement BusinessBase and ICatalogBusiness
    /// </summary>
    public class CatalogBusiness : BusinessBase<Catalog>, ICatalogBusiness
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///Constructor
        /// </summary>
        /// <param name="catalogRepository"></param>
        /// <param name="unitOfWork"></param>
        public CatalogBusiness(ICatalogRepository catalogRepository, IUnitOfWork unitOfWork) : base(catalogRepository,unitOfWork)
        {
            this._catalogRepository = catalogRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all catalog by guideline ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            return _catalogRepository.GetAll(id);
        }

        public override IEnumerable<Catalog> GetAll()
        {
            return _catalogRepository.Get(x => x.Active == true);
        }

        public bool ChangeActive(int id)
        {
            var catalog = _catalogRepository.GetSingleById(id);
            catalog.Active = false;
            _catalogRepository.Update(catalog);
            return true;
        }

        public IEnumerable<CatalogQuestion> GetQuestion(int idcatalog)
        {
            return _catalogRepository.GetQuestion(idcatalog);
        }

        public IEnumerable<Question> AvailableQuestion(int idcatalog)
        {
            return _catalogRepository.AvailableQuestion(idcatalog);
        }
    }
}