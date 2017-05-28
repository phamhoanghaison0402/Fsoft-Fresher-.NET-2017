using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{

    /// <summary>
    /// Repository for InterviewRound
    /// </summary>
    /// <seealso cref="IPM.Data.Infrastructure.IRepository{IPM.Model.Models.InterviewRound}" />
    public interface IInterviewRoundRepository : IRepository<InterviewRound>
    {

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        IEnumerable<InterviewRound> GetList(string search);
    }


    /// <summary>
    /// Interview round repository
    /// </summary>
    public class InterviewRoundRepository : RepositoryBase<InterviewRound>, IInterviewRoundRepository
    {
        
        /// <summary>
        /// The interview rounds
        /// </summary>
        private IEnumerable<InterviewRound> interviewRounds;


        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewRoundRepository"/> class.
        /// </summary>
        /// <param name="dbFactory">The database factory.</param>
        public InterviewRoundRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public IEnumerable<InterviewRound> GetList(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                interviewRounds = from interviewRound in DbContext.InterviewRounds
                                  where interviewRound.Active == true
                                        && (interviewRound.RoundName.Contains(search)
                                        || interviewRound.Description.Contains(search)
                                        || interviewRound.Guideline.Name.Contains(search))
                                  select interviewRound;
            }
            else
            {
                interviewRounds = from interviewRound in DbContext.InterviewRounds
                                  where interviewRound.Active == true
                                  select interviewRound;
            }
            return interviewRounds.OrderByDescending(x => x.ID);
        }       
    }
}