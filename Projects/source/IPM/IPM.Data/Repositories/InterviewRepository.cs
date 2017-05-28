using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// interface of InterviewRepository
    /// </summary>
    public interface IInterviewRepository : IRepository<Interview>
    {
        /// <summary>
        /// Get Interview ADmin by Role ID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewAdminById(int roleId);

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate candidate);
    }

    /// <summary>
    /// class InterviewRepository implement RepositoryBase, IInterviewRepository
    /// </summary>
    public class InterviewRepository : RepositoryBase<Interview>, IInterviewRepository
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbFactory"></param>
        public InterviewRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Get Interview Admin By ID
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewAdminById(int candidateId)
        {
            var interview =  from can in DbContext.Candidates
                                join inte in DbContext.Interviews
                                on can.ID equals inte.CandidateID
                                where can.ID == candidateId
                                select inte;

            return interview;
        }

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate candidate)
        {
            var interviews = DbContext.Interviews.Where(i => i.CandidateID == candidate.ID 
                                                        && i.RoundProcess.InterviewProcess.PositionID == candidate.PositionID)
                                .OrderBy(i => i.RoundProcess.RoundOrder);

            return interviews;
        }
    }
}