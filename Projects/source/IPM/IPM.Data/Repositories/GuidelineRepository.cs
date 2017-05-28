using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// Interface of Catalogrepository.
    /// </summary>
    public interface IGuidelineRepository : IRepository<Guideline>
    {

        /// <summary>
        /// Declare function GetQuestion by catalog id.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        IEnumerable<GuidelineCatalog> GetCatalog(int idguideline);

        /// <summary>
        /// Declare function AvailableQuestion.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        IEnumerable<Catalog> AvailableCatalog(int idguideline);
    }

    /// <summary>
    /// Declare class CatalogRepository implement RepositoryBase, ICatalogRepository.
    /// </summary>
    public class GuidelineRepository : RepositoryBase<Guideline>, IGuidelineRepository
    {
        /// <summary>
        /// Constructor CatalogRepository.
        /// </summary>
        /// <param name="dbFactory"></param>
        public GuidelineRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Implement function get GetQuestion by catalog id.
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        public IEnumerable<GuidelineCatalog> GetCatalog(int idguideline)
        {
            var query = (from guidelineCatalog in DbContext.GuidelineCatalogs
                where guidelineCatalog.GuidelineID == idguideline
                select guidelineCatalog).OrderBy(x => x.ID);

            return query;
        }

        /// <summary>
        /// Implement function get AvailableQuestion. 
        /// </summary>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        public IEnumerable<Catalog> AvailableCatalog(int idGuideline)
        {
            var querycatalog = from catalog in DbContext.Catalogs
                join guidelineCatalog in DbContext.GuidelineCatalogs
                on catalog.ID equals guidelineCatalog.CatalogID
                where guidelineCatalog.GuidelineID == idGuideline
                select catalog;

            var listCatalog = (from catalog in DbContext.Catalogs
                where !(querycatalog.Any(x => x.ID == catalog.ID))
                select catalog).OrderBy(x => x.ID);

            return listCatalog;
        }

    }
}