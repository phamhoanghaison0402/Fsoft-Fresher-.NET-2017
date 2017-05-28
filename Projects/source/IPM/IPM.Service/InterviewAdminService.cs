using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Business;

namespace IPM.Service
{
    public interface IInterviewAdminService : IServiceBase<User>
    {
        /// <summary>
        /// Get list interview admin by user role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IEnumerable<User> GetAllByRoleId(int roleId, string search);

        /// <summary>
        /// Get list candidate by InterviewAdminID (candidate of this interview admin manage).
        /// </summary>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        IEnumerable<Candidate> GetCandidateByInterViewAdminID(int interviewAdminId);

        /// <summary>
        /// Update candidate (Transmit candidate).
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        int UpdateCandidate(Candidate candidate, int interviewAdminId);

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate candidate);
    }

    /// <summary>
    /// Class interview admin service
    /// </summary>
    class InterviewAdminService : ServiceBase<User>, IInterviewAdminService
    {
        /// <summary>
        /// Object IInterviewAdminBusiness interface.
        /// </summary>
        private IInterviewAdminBusiness _interviewAdminBusiness;

        /// <summary>
        /// Constructor with param interview admin business.
        /// </summary>
        /// <param name="interviewAdminBusiness"></param>
        public InterviewAdminService(IInterviewAdminBusiness interviewAdminBusiness) : base(interviewAdminBusiness)
        {
            this._interviewAdminBusiness = interviewAdminBusiness;
        }

        /// <summary>
        /// Get list interview admin by user role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAllByRoleId(int roleId, string search)
        {
            return _interviewAdminBusiness.GetAllByRoleId(roleId, search);
        }

        /// <summary>
        /// Get list candidate by InterviewAdminID (candidate of this interview admin manage).
        /// </summary>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        public IEnumerable<Candidate> GetCandidateByInterViewAdminID(int interviewAdminId)
        {
            return _interviewAdminBusiness.GetCandidateByInterViewAdminID(interviewAdminId);
        }

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="interviewAdminId"></param>
        /// <param name="roleId"></param>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate candidate)
        {
            return _interviewAdminBusiness.GetInterviewByInterviewAdmin(candidate);
        }

        /// <summary>
        /// Update candidate (Transmit candidate).
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        public int UpdateCandidate(Candidate candidate, int interviewAdminId)
        {
            return _interviewAdminBusiness.UpdateCandidate(candidate, interviewAdminId);
        }
    }
}
