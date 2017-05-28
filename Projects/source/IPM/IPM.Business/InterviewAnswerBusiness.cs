using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    /// <summary>
    /// Interface of InterviewAnswerBusiness
    /// </summary>
    public interface IInterviewAnswerBusiness : IBusinessBase<InterviewAnswer>
    {
        /// <summary>
        /// Get Interview Answer List
        /// </summary>
        /// <param name="interviewId"></param>
        /// <returns></returns>
        ICollection<InterviewAnswer> GetInterviewAnswerList(int interviewId);
        /// <summary>
        /// Get Last Inserted Interview Answer ID
        /// </summary>
        /// <returns></returns>
        int GetLastInserted();
    }

    /// <summary>
    /// Class InterviewAnswerBusiness implement BusinessBass and IInterviewAnswerBusiness
    /// </summary>
    public class InterviewAnswerBusiness : BusinessBase<InterviewAnswer>, IInterviewAnswerBusiness
    {
        private readonly IInterviewAnswerRepository _interviewAnswerRepository;
        private ICandidateRepository _candidateRepository;
        private IInterviewRepository _interviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interviewAnswerRepository"></param>
        /// <param name="unitOfWork"></param>
        public InterviewAnswerBusiness(IInterviewAnswerRepository interviewAnswerRepository, IUnitOfWork unitOfWork)
            : base(interviewAnswerRepository, unitOfWork)
        {
            this._interviewAnswerRepository = interviewAnswerRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get All Interview Results of this Interview via Interview ID
        /// </summary>
        /// <param name="interviewId"></param>
        /// <returns></returns>
        public ICollection<InterviewAnswer> GetInterviewAnswerList(int interviewId)
        {
            var interviewAnswer = _interviewAnswerRepository.GetInterviewAnswerList(interviewId);

            if (interviewAnswer == null)
            {
                throw new Exception("InterviewAnswer not found.");
            }
            return interviewAnswer;
        }

        /// <summary>
        /// Get ID of Last Inserted Interview Result
        /// </summary>
        /// <returns></returns>
        public int GetLastInserted()
        {
            return _interviewAnswerRepository.GetLastId();           
        }
    }
}