using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// interface of InterviewAnswerRepository
    /// </summary>
    public interface IInterviewAnswerRepository : IRepository<InterviewAnswer>
    {
        /// <summary>
        /// get all Interview Answer by Interview ID
        /// </summary>
        /// <param name="interviewId"></param>
        /// <returns></returns>
        ICollection<InterviewAnswer> GetInterviewAnswerList(int interviewId);
        /// <summary>
        /// Get last inserted interview answer ID
        /// </summary>
        /// <returns></returns>
        int GetLastId();
    }

    /// <summary>
    /// class InterviewAnswerRepository implement RepositoryBase, InterviewAnswerRepository
    /// </summary>
    public class InterviewAnswerRepository : RepositoryBase<InterviewAnswer>, IInterviewAnswerRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public InterviewAnswerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
       
        /// <summary>
        /// Get InterviewAnswer List information
        /// </summary>
        /// <param name="interviewId">InterviewAnswer id</param>
        /// <returns>InterviewAnswer</returns>
        public ICollection<InterviewAnswer> GetInterviewAnswerList(int interviewId)
        {
            var listAnswers = DbContext.InterviewAnswers.Include(c => c.Catalog).Include(c=>c.AnswerQuestions.Select(x=>x.Question)).Where(c => c.InterviewID == interviewId);
            ICollection<InterviewAnswer> listInterviewAnswers = new List<InterviewAnswer>();
            foreach (var item in listAnswers)
            {
                listInterviewAnswers.Add(item);
            }
            return listInterviewAnswers;
        }

        /// <summary>
        /// Get Last Inserted Interview Result
        /// </summary>
        /// <returns></returns>
        public int GetLastId()
        {
            return DbContext.InterviewAnswers.OrderByDescending(x=>x.ID).Take(1).Single().ID;
        }
    }
}