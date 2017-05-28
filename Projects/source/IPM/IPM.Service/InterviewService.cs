using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;
using System;

namespace IPM.Service
{
    /// <summary>
    /// interface  of InterviewService
    /// </summary>
    public interface IInterviewService : IServiceBase<Interview>
    {
        /// <summary>
        /// Get interview
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewList(int id);
        /// <summary>
        /// Get Interview Results
        /// </summary>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewResultList();
        /// <summary>
        /// Get Interview Info of Candidate
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewListofCandidate(int candidateID);

        /// <summary>
        /// Update interview admin in interview.
        /// </summary>
        /// <param name="interviewId"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        int UpdateInterview(int interviewId, int interviewAdminId);

        IEnumerable<Interview> GetInterviewResultofCandidate(int candidateID);
    }
    /// <summary>
    /// class InterviewService implement ServiceBase, IInterviewService
    /// </summary>
    public class InterviewService : ServiceBase<Interview>, IInterviewService
    {
        private readonly IInterviewBusiness _interviewBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interviewBusiness"></param>
        public InterviewService(IInterviewBusiness interviewBusiness)
            : base(interviewBusiness)
        {
            this._interviewBusiness = interviewBusiness;
        }

        /// <summary>
        /// Get Interview
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewList(int id)
        {
            return _interviewBusiness.GetInterviewList(id);
        }

        /// <summary>
        /// Get Interview Result List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewResultList()
        {
            return _interviewBusiness.GetInterviewResultList();
        }

        /// <summary>
        /// Get Interview List of Candidate
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewListofCandidate(int candidateID)
        {
            return _interviewBusiness.GetInterviewListofCandidate(candidateID);
        }

        /// <summary>
        /// Update interview admin in interview.
        /// </summary>
        /// <param name="interviewId"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        public int UpdateInterview(int interviewId, int interviewAdminId)
        {
            return _interviewBusiness.UpdateInterview(interviewId, interviewAdminId);
        }

        /// <summary>
        /// Get Interview List of Candidate
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewResultofCandidate(int candidateID)
        {
            return _interviewBusiness.GetInterviewResultofCandidate(candidateID);
        }

    }
}