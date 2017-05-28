using IPM.Business;
using IPM.Model.Models;

namespace IPM.Service
{
    /// <summary>
    /// interface of GuidelineCatalogService
    /// </summary>
    public interface IGuidelineCatalogService : IServiceBase<GuidelineCatalog>
    {

    }

    /// <summary>
    /// class GuidelineCatalogService implement ServiceBase, IGuidelineCatalogService
    /// </summary>
    public class GuidelineCatalogService : ServiceBase<GuidelineCatalog>, IGuidelineCatalogService
    {
        private readonly IGuidelineCatalogBusiness _guidelineCatalogBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guidelineCatalogBusiness"></param>
        public GuidelineCatalogService(IGuidelineCatalogBusiness guidelineCatalogBusiness)
            : base(guidelineCatalogBusiness)
        {
            this._guidelineCatalogBusiness = guidelineCatalogBusiness;
        }

    }
}