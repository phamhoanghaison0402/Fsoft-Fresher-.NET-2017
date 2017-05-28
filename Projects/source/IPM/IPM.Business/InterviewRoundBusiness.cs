using System;
using System.Collections.Generic;
using IPM.Model.Models;
using IPM.Data.Repositories;
using IPM.Data.Infrastructure;
using log4net;

namespace IPM.Business
{
    /// <summary>
    /// Bussiness for InterviewRound
    /// </summary>
    public interface IInterviewRoundBusiness : IBusinessBase<InterviewRound>
    {


        /// <summary>
        /// Get all by  interview round ID
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        IEnumerable<InterviewRound> ListInterviewRound(int id);


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        IEnumerable<InterviewRound> GetList(string search);

        
        /// <summary>
        /// Adds the InterviewRound
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        void AddRound(InterviewRound interviewRound);


        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Update_Status(int id);

    }


    /// <summary>
    /// Implement Interface IInterviewRoundBusiness
    /// </summary>
    /// <seealso cref="IPM.Business.IInterviewRoundBusiness" />
    public class InterviewRoundBusiness : BusinessBase<InterviewRound>,IInterviewRoundBusiness 
    {


        private IInterviewRoundRepository _interviewRoundRepository;

        private IRoundProcessRepository _roundProcessRepository;

        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewRoundBusiness"/> class.
        /// </summary>
        /// <param name="interviewRoundRepository">The interview round repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="roundProcessRepository">The round process repository.</param>
        public InterviewRoundBusiness(IInterviewRoundRepository interviewRoundRepository, IUnitOfWork unitOfWork, IRoundProcessRepository roundProcessRepository)
                    :base(interviewRoundRepository,unitOfWork)
        {
            this._interviewRoundRepository = interviewRoundRepository;          

            this._roundProcessRepository = roundProcessRepository;

            this._unitOfWork = unitOfWork;
        }
                          

        /// <summary>
        /// Get all InterviewRound
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<InterviewRound> GetAll()
        {
            return _interviewRoundRepository.Get(r => r.Active == true, null, "Guideline, RoundProcesses");            
        }


        /// <summary>
        /// Get List Process for InterviewRound with ID
        /// </summary>
        /// <param name="id">The round identifier.</param>
        /// <returns></returns>
        public IEnumerable<InterviewRound> ListInterviewRound(int id)
        {
            return _interviewRoundRepository.Get(r=>r.ID==id, null, "GuideLine");
        }            


        /// <summary>
        /// Insert new InterviewRound
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        public void AddRound(InterviewRound interviewRound)
        {
            interviewRound.Active = true;
            _interviewRoundRepository.Add(interviewRound);
            Save();
            
        }


        /// <summary>
        /// Update InterviewRound
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        /// <exception cref="System.Exception">Interview Round not found</exception>
        public override void Update(InterviewRound interviewRound)
        {
            var interviewRoundDb = _interviewRoundRepository.GetSingleById(interviewRound.ID);

            if (interviewRoundDb == null)
            {
                log.Error("Interview Round not found");
                throw new Exception("Interview Round not found");
            }
            else
            {
                interviewRoundDb.RoundName = interviewRound.RoundName;

                interviewRoundDb.Description = interviewRound.Description;

                interviewRoundDb.GuidelineID = interviewRound.GuidelineID;

                _interviewRoundRepository.Update(interviewRoundDb);

                Save();
            }           

        }


        /// <summary>
        /// Update InterviewRound set Active = false
        /// </summary>
        /// <param name="id">InterviewRound.ID</param>
        /// <exception cref="System.Exception">InterviewRound not found.</exception>
        public void Update_Status(int id)
        {
            var interviewRound = _interviewRoundRepository.GetSingleById(id);

            if (interviewRound == null)
            {
                log.Error("InterviewRound not found.");
                throw new Exception("InterviewRound not found.");
            }
            else
            {
                interviewRound.Active = false;

                _interviewRoundRepository.Update(interviewRound);

                Save();
            }
           
        }


        /// <summary>
        /// Get List Process for InterviewRound
        /// </summary>
        /// <param name="RoundId">The round identifier.</param>
        /// <returns></returns>      
        public IEnumerable<RoundProcess> ListRoundProcess(int RoundId)
        {
            yield return _roundProcessRepository.GetSingleById(RoundId);
        }


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<InterviewRound> GetList(string search)
        {
            return _interviewRoundRepository.GetList(search);
        }
    }
}
