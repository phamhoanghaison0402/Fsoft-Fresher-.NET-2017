using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;
using System;

namespace IPM.Service
{
    /// <summary>
    /// Interface of CatalogService
    /// </summary>
    public interface ICatalogService :IServiceBase<Catalog>
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
    /// Class CatalogService implement ServiceBase, ICatalogService
    /// </summary>
    public class CatalogService : ServiceBase<Catalog>, ICatalogService
    {
        private readonly ICatalogBusiness _catalogBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="catalogBusiness"></param>
        public CatalogService(ICatalogBusiness catalogBusiness) :base(catalogBusiness)
        {
            this._catalogBusiness = catalogBusiness;
        }

        /// <summary>
        /// Get all Question (ID) of Catalog via Catalog ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            return _catalogBusiness.GetAll(id);
        }


        public  IEnumerable<Catalog> GetAll()
        {
            return _catalogBusiness.GetAll();
        }

        public bool ChangeActive(int id)
        {
            return _catalogBusiness.ChangeActive(id);
        }

        public IEnumerable<CatalogQuestion> GetQuestion(int idcatalog)
        {
            return _catalogBusiness.GetQuestion(idcatalog);
        }

        public IEnumerable<Question> AvailableQuestion(int idcatalog)
        {
            return _catalogBusiness.AvailableQuestion(idcatalog);
        }
    }
}