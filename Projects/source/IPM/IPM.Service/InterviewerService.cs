using System.Collections.Generic;
using IPM.Business;
using IPM.Model.Models;

namespace IPM.Service
{
    /// <summary>
    /// </summary>
    public interface IInterviewerService
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        IEnumerable<User> GetAll(string searchString);
    }

    /// <summary>
    /// </summary>
    /// <seealso cref="IPM.Service.IInterviewerService" />
    public class InterviewerService : IInterviewerService
    {
        private readonly IInterviewerBusiness _interviewerBusiness;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InterviewerService" /> class.
        /// </summary>
        /// <param name="interviewerBusiness">The interviewer business.</param>
        public InterviewerService(IInterviewerBusiness interviewerBusiness)
        {
            _interviewerBusiness = interviewerBusiness;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public IEnumerable<User> GetAll(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _interviewerBusiness.GetAll(searchString);
            }
            return _interviewerBusiness.GetAll(searchString);
        }
    }
}
