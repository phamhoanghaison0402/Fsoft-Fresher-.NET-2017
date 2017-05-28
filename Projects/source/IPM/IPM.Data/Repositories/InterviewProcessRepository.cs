using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace IPM.Data.Repositories
{
    public interface IInterviewProcessRepository : IRepository<InterviewProcess>
    {
        /// <summary>
        /// Get interview process include RoundProcess and InterviewRound
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InterviewProcess GetProcessById(int id);
    }
    /// <summary>
    /// Interview process repository
    /// </summary>
    public class InterviewProcessRepository : RepositoryBase<InterviewProcess>, IInterviewProcessRepository
    {
        public InterviewProcessRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        /// <summary>
        /// Get process by Id, include RoundProcess and InterviewRound
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InterviewProcess GetProcessById(int id)
        {
            return DbContext.InterviewProcesses.Include(ip => ip.RoundProcesses.Select(x=>x.InterviewRound)).Include(i=>i.Position).FirstOrDefault(x => x.ID == id);
        }

    }
}
