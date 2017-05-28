using System;
using System.Collections.Generic;
using IPM.Business;
using IPM.Model.Models;

namespace IPM.Service
{
    public interface IInterviewRoundService :IServiceBase<InterviewRound>
    {
        
        /// <summary>
        /// Lists the interview round.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        IEnumerable<InterviewRound> ListInterviewRound(int id);


        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Update_Status(int id);


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        IEnumerable<InterviewRound> GetList(string search);


        /// <summary>
        /// Adds the round.
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        void AddRound(InterviewRound interviewRound);
    }


    /// <summary>
    /// Implement Interface IInterviewRoundService
    /// </summary>
    /// <seealso cref="IPM.Service.IInterviewRoundService" />
    public class InterviewRoundService : ServiceBase<InterviewRound>,IInterviewRoundService
    {
        /// <summary>
        /// The interview round business
        /// </summary>
        private IInterviewRoundBusiness _interviewRoundBusiness;        


        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewRoundService"/> class.
        /// </summary>
        /// <param name="interviewRoundBusiness">The interview round business.</param>
        public InterviewRoundService(IInterviewRoundBusiness interviewRoundBusiness)
                :base(interviewRoundBusiness)
        {
            this._interviewRoundBusiness = interviewRoundBusiness;
        }            
        

        /// <summary>
        /// Inserts the specified interview round.
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        public void AddRound(InterviewRound interviewRound)
        {
            _interviewRoundBusiness.AddRound(interviewRound);
        }
                

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Update_Status(int id)
        {
            _interviewRoundBusiness.Update_Status(id);
        }


        /// <summary>
        /// Lists the interview round.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IEnumerable<InterviewRound> ListInterviewRound(int id)
        {
            return _interviewRoundBusiness.ListInterviewRound(id);
        }


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public IEnumerable<InterviewRound> GetList(string search)
        {
            return _interviewRoundBusiness.GetList(search);
        }

                                
    }
}
