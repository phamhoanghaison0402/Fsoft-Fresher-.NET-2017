using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPM.Business
{
    public interface IInterviewAdminBusiness : IBusinessBase<User>
    {
        /// <summary>
        /// Get list interview admin by user role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IEnumerable<User> GetAllByRoleId(int roleId, string search);

        /// <summary>
        /// Update candidate (Transmit candidate).
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="interviewAdminID"></param>
        /// <returns></returns>
        int UpdateCandidate(Candidate candidate, int interviewAdminID);

        /// <summary>
        /// Get list candidate by InterviewAdminID (candidate of this interview admin manage).
        /// </summary>
        /// <param name="interviewAdminID"></param>
        /// <returns></returns>
        IEnumerable<Candidate> GetCandidateByInterViewAdminID(int interviewAdminID);

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="camdidate"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate camdidate);
    }

    /// <summary>
    /// Class interview admin business.
    /// </summary>
    public class InterviewAdminBusiness : BusinessBase<User>, IInterviewAdminBusiness
    {
        
        /// <summary>
        /// Object interface interview admin repository.
        /// </summary>
        private IInterviewAdminRepository _interviewAdminRepository;

        /// <summary>
        /// Object interface candidate repository
        /// </summary>
        private ICandidateRepository _candidateRepository;

        /// <summary>
        /// Object interface interview repository.
        /// </summary>
        private IInterviewRepository _interviewRepository;

        /// <summary>
        /// Object interface unit of work.
        /// </summary>
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Result update success.
        /// </summary>
        private const int INT_UPDTAE_SUCCESS = 1;

        /// <summary>
        /// Result cannot update because not select other interview admin.
        /// </summary>
        private const int INT_CANNOT_UPDATE = 0;

        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="interviewAdminRepository"></param>
        /// <param name="candidateRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="userRoleRepository"></param>
        public InterviewAdminBusiness(IInterviewAdminRepository interviewAdminRepository,ICandidateRepository candidateRepository, IUnitOfWork unitOfWork,
            IInterviewRepository interviewRepository) : base(interviewAdminRepository, unitOfWork)
        {
            this._interviewAdminRepository = interviewAdminRepository;
            this._unitOfWork = unitOfWork;
            this._candidateRepository = candidateRepository;
            this._interviewRepository = interviewRepository;
        }

        /// <summary>
        /// Get list interview admin by user role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAllByRoleId(int roleId, string search)
        {
            return _interviewAdminRepository.GetInterviewAdminByRole(roleId, search);
        }

        /// <summary>
        /// Get list candidate by InterviewAdminID (candidate of this interview admin manage).
        /// </summary>
        /// <param name="interviewAdminID"></param>
        /// <returns></returns>
        public IEnumerable<Candidate> GetCandidateByInterViewAdminID(int interviewAdminID)
        {
            return _candidateRepository.Get(x => x.InterviewAdminID == interviewAdminID, null, "Position");
        }

        /// <summary>
        /// Update candidate (Transmit candidate).
        /// </summary>
        /// <param name="candidateModel"></param>
        /// <param name="interviewAdminID"></param>
        /// <returns></returns>
        public int UpdateCandidate(Candidate candidateModel, int interviewAdminID)
        {

            try
            {
                var candidate = _candidateRepository.GetSingleById(candidateModel.ID);

                if (candidate == null)
                {
                    throw new Exception("Candidate not found with candidate id: " + candidateModel.ID);
                }
                if (candidate.InterviewAdminID != interviewAdminID)
                {
                    candidate.InterviewAdminID = interviewAdminID;
                    var result = _candidateRepository.Update(candidate);

                    return INT_UPDTAE_SUCCESS;
                }
                else
                {
                    return INT_CANNOT_UPDATE;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Update candidate error with candidate id: " + candidateModel.ID + "----" + ex.Message);
            }
        }

        /// <summary>
        /// Get interviews by interview admin and candidate.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewByInterviewAdmin(Candidate candidate)
        {
            return _interviewRepository.GetInterviewByInterviewAdmin(candidate);
        }
    }
}
