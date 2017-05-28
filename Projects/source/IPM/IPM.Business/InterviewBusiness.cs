using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPM.Business
{
    /// <summary>
    /// Interface for InterviewBusiness
    /// </summary>
    public interface IInterviewBusiness : IBusinessBase<Interview>
    {
        /// <summary>
        /// Get all Interview List by Interviewer ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewList(int id);
        /// <summary>
        /// Get all Interview Results
        /// </summary>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewResultList();
        /// <summary>
        /// Get all interview info of Canddiate
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        IEnumerable<Interview> GetInterviewListofCandidate(int candidateID);

        /// <summary>
        /// Update interview admin in interview
        /// </summary>
        /// <param name="interviewId"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        int UpdateInterview(int interviewId, int interviewAdminId);

        IEnumerable<Interview> GetInterviewResultofCandidate(int candidateID);
    }

    /// <summary>
    /// Class InterviewBusiness implement BusinessBass and IInterviewBusiness
    /// </summary>
    public class InterviewBusiness : BusinessBase<Interview>, IInterviewBusiness
    {

        /// <summary>
        /// Object interview repository
        /// </summary>
        private readonly IInterviewRepository _interviewRepository;

        /// <summary>
        /// Object interface unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Return result update success.
        /// </summary>
        private const int INT_UPDTAE_SUCCESS = 1;

        /// <summary>
        /// [TienTD1] return result cannot update because not select other interview admin
        /// </summary>
        private const int INT_CANNOT_UPDATE = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interviewRepository"></param>
        /// <param name="unitOfWork"></param>
        public InterviewBusiness(IInterviewRepository interviewRepository, IUnitOfWork unitOfWork) :base(interviewRepository, unitOfWork)
        {
            this._interviewRepository = interviewRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get Interview Schedule for this Interviewer via Interviewer ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewList(int id)
        {
            try
            {
                return _interviewRepository.Get(x => x.InterviewerID == id && x.Result == null && x.RoundProcess.Active == true, null, "Candidate,Interviewer,RoundProcess.InterviewRound");
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get Interview Result List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewResultList()
        {
            try
            {
                return _interviewRepository.Get(x => x.Result != null, null, "Candidate,Interviewer,RoundProcess.InterviewRound");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get All Interview of this Candidata vie Candidate ID
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewListofCandidate(int candidateID)
        {
            try
            {
                return _interviewRepository.Get(x => x.CandidateID == candidateID, null, "Candidate,Interviewer,RoundProcess.InterviewRound, Candidate.CandidateCertificates.Certificate, Candidate.CandidateSkills.Skill");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get All Interview of this Candidata vie Candidate ID
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public IEnumerable<Interview> GetInterviewResultofCandidate(int candidateID)
        {
            try
            {
                return GetInterviewListofCandidate(candidateID).Where(x => x.Result != null);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Override GetSingleByID, Get Info of candidate and interviewer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Interview GetSingleById(int id)
        {
            try
            {
                return _interviewRepository.GetSingleByCondition(x => x.ID == id, new string[] { "Candidate", "Interviewer" });
            }
            catch (Exception ex)
            {
                log.Error(String.Format("Interview ID: {0} - Message: {1}", id, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update interview admin in interview.
        /// </summary>
        /// <param name="interviewId"></param>
        /// <param name="interviewAdminId"></param>
        /// <returns></returns>
        public int UpdateInterview(int interviewId, int interviewAdminId)
        {
            try
            {
                var interview = _interviewRepository.GetSingleById(interviewId);
                if (interview == null)
                {
                    throw new Exception("interview not found with interview id = " + interviewId);
                }
                if (interview.InterviewAdminID != interviewAdminId)
                {
                    interview.InterviewAdminID = interviewAdminId;
                    _interviewRepository.Update(interview);
                    
                    return INT_UPDTAE_SUCCESS;
                }
                else
                {
                    return INT_CANNOT_UPDATE;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Update interview error with interview id = " + interviewId + "----" + ex.Message); 
            }
        }
    }
}