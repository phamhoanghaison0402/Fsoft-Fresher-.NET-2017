using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// Interface of AnswerQuestionRepository
    /// </summary>
    public interface IAnswerQuestionRepository : IRepository<AnswerQuestion>
    {
    }

    /// <summary>
    /// Class AnswerQuestionResository implement RepositoryBase, IAnswerQuestionRepository
    /// </summary>
    public class AnswerQuestionRepository : RepositoryBase<AnswerQuestion>, IAnswerQuestionRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public AnswerQuestionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        
    }
}