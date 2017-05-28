using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// interface  of QuestionRepository.
    /// </summary>
    public interface IQuestionRepository : IRepository<Question>
    {
        /// <summary>
        ///  Get all Question by catalog id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetAll(int id);
    }

    /// <summary>
    /// class QuestionRepository implement repositorybase, iquestionrepository.
    /// </summary>
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {

        /// <summary>
        /// Constructor questionrepository.
        /// </summary>
        /// <param name="dbFactory"></param>
        public QuestionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Get all question of this catalog via catalog id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            var idlist = (from cq in DbContext.CatalogQuestions
                         where cq.CatalogID == id
                         select cq.QuestionID).ToList();
            return (List<int>)idlist;
        }
    }
}