using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;

namespace IPM.Service
{
    /// <summary>
    /// interface of InterviewAnswersService
    /// </summary>
    public interface IInterviewAnswersService : IServiceBase<InterviewAnswer>
    {
        /// <summary>
        /// Get all Interview Answer by Interview ID
        /// </summary>
        /// <param name="interviewId"></param>
        /// <returns></returns>
        ICollection<InterviewAnswer> GetInterviewAnswersList(int interviewId);
        /// <summary>
        /// Get last inserted interview Answer ID
        /// </summary>
        /// <returns></returns>
        int GetLastInserted();
    }

    /// <summary>
    /// class InterviewAnswersService implement ServiceBase, IInterviewAnswersService
    /// </summary>
    public class InterviewAnswerService : ServiceBase<InterviewAnswer>, IInterviewAnswersService
    {
        private readonly IInterviewAnswerBusiness _interviewAnswersBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interviewAnswersBusiness"></param>
        public InterviewAnswerService(IInterviewAnswerBusiness interviewAnswersBusiness)
            : base(interviewAnswersBusiness)
        {
            this._interviewAnswersBusiness = interviewAnswersBusiness;
        }

        /// <summary>
        /// Get all Interview Result of this Interview by Interview ID
        /// </summary>
        /// <param name="interviewId"></param>
        /// <returns></returns>
        public ICollection<InterviewAnswer> GetInterviewAnswersList(int interviewId)
        {
            return _interviewAnswersBusiness.GetInterviewAnswerList(interviewId);
        }

        /// <summary>
        /// Get Last Inserted Interview Result
        /// </summary>
        /// <returns></returns>
        public int GetLastInserted()
        {
            return _interviewAnswersBusiness.GetLastInserted();
        }
    }
}