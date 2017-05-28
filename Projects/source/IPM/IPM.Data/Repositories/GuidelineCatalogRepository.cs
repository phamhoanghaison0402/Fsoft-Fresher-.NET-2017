using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// interface of GuidelineCatalogRepository.
    /// </summary>
    public interface IGuidelineCatalogRepository : IRepository<GuidelineCatalog>
    {
    }

    /// <summary>
    /// Declare class GuidelineCatalogRepository implement RepositoryBase, IGuidelineCatalogRepository.
    /// </summary>
    public class GuidelineCatalogRepository : RepositoryBase<GuidelineCatalog>, IGuidelineCatalogRepository
    {
        /// <summary>
        /// Constructor GuidelineCatalogRepository.
        /// </summary>
        /// <param name="dbFactory"></param>
        public GuidelineCatalogRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        
    }
}