using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;
using System;

namespace IPM.Service
{
    /// <summary>
    /// Interface of GuidelineService
    /// </summary>
    public interface IGuidelineService : IServiceBase<Guideline>
    {
        

        bool ChangeActive(int id);

        IEnumerable<GuidelineCatalog> GetCatalog(int idguideline);


        IEnumerable<Catalog> AvailableCatalog(int idguideline);
    }

    /// <summary>
    /// Class GuidelineService implement ServiceBase, IGuidelineService
    /// </summary>
    public class GuidelineService : ServiceBase<Guideline>, IGuidelineService
    {
        private readonly IGuidelineBusiness _catalogBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="catalogBusiness"></param>
        public GuidelineService(IGuidelineBusiness catalogBusiness) : base(catalogBusiness)
        {
            this._catalogBusiness = catalogBusiness;
        }

        public IEnumerable<Guideline> GetAll()
        {
            return _catalogBusiness.GetAll();
        }

        public bool ChangeActive(int id)
        {
            return _catalogBusiness.ChangeActive(id);
        }

        public IEnumerable<GuidelineCatalog> GetCatalog(int idguideline)
        {
            return _catalogBusiness.GetCatalog(idguideline);
        }

        public IEnumerable<Catalog> AvailableCatalog(int idguideline)
        {
            return _catalogBusiness.AvailableCatalog(idguideline);
        }
    }
}