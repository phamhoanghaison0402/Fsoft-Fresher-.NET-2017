using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// Declare interface of CatalogQuestionRepository.
    /// </summary>
    public interface ICatalogQuestionRepository : IRepository<CatalogQuestion>
    {
    }

    /// <summary>
    /// class CatalogQuestionRepository implement RepositoryBase, ICatalogQuestionRepository.
    /// </summary>
    public class CatalogQuestionRepository : RepositoryBase<CatalogQuestion>, ICatalogQuestionRepository
    {
        /// <summary>
        /// Constructor CatalogQuestionRepository.
        /// </summary>
        /// <param name="dbFactory"></param>
        public CatalogQuestionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}