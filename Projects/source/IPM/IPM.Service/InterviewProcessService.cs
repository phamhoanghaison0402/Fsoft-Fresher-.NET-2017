using IPM.Business;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Model;

namespace IPM.Service
{
    public interface IInterviewProcessService : IServiceBase<InterviewProcess>
    {
        /// <summary>
        /// Add interview process
        /// </summary>
        /// <param name="process">interview process</param>
        /// <param name="listRound">list interview round in process</param>
        /// <returns>interview process id or exception throw from business layer</returns>
        int AddProcess(InterviewProcess process, List<RoundProcess> listRound);
        /// <summary>
        /// Get interview process by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>interview process or exception throw from business layer</returns>
        InterviewProcess GetProcessById(int id);
        /// <summary>
        /// Edit interview round
        /// </summary>
        /// <param name="process">interview process</param>
        /// <param name="listRound">list interview round in process</param>
        /// <returns>interview process or exception throw from business layer</returns>
        InterviewProcess Edit(InterviewProcess process, List<RoundProcess> listRound);
        /// <summary>
        /// Search interview process in multi field
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <returns>
        /// List search result or exception throw from business layer
        /// </returns>
        IEnumerable<InterviewProcess> Search(string searchString);
    }
    /// <summary>
    /// Interview process service
    /// </summary>
    public class InterviewProcessService : ServiceBase<InterviewProcess>, IInterviewProcessService
    {
        private readonly IInterviewProcessBusiness _interviewProcessBusiness;
        /// <summary>
        /// interview process service contructor
        /// </summary>
        /// <param name="interviewProcessBusiness">interview process business</param>
        public InterviewProcessService(IInterviewProcessBusiness interviewProcessBusiness)
            : base(interviewProcessBusiness)
        {
            this._interviewProcessBusiness = interviewProcessBusiness;
        }
        /// <summary>
        /// Add interview process
        /// </summary>
        /// <param name="process">interview process</param>
        /// <param name="listRound">list interview round in process</param>
        /// <returns>interview process id or exception throw from business layer</returns>
        public int AddProcess(InterviewProcess process, List<RoundProcess> listRound)
        {
            log.Info(String.Format("Function: AddProcess - process: {0} - listRound: {1}", process, listRound));
            return _interviewProcessBusiness.AddProcess(process, listRound);
        }
        /// <summary>
        /// Get interview process by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>interview process or exception throw from business layer</returns>
        public InterviewProcess GetProcessById(int id)
        {
            log.Info(String.Format("Function: GetProcessById - id: {0}", id));
            return _interviewProcessBusiness.GetProcessById(id);
        }
        /// <summary>
        /// Edit interview round
        /// </summary>
        /// <param name="process">interview process</param>
        /// <param name="listRound">list interview round in process</param>
        /// <returns>interview process or exception throw from business layer</returns>
        public InterviewProcess Edit(InterviewProcess process, List<RoundProcess> listRound)
        {
            log.Info(String.Format("Function: Edit - process: {0} - listRound: {1}", process, listRound));
            return _interviewProcessBusiness.Edit(process, listRound);
        }
        /// <summary>
        /// Search interview process in multi field
        /// </summary>
        /// <param name="searchString">search string</param>
        /// <returns>
        /// List search result or exception throw from business layer
        /// </returns>
        public IEnumerable<InterviewProcess> Search(string searchString)
        {
            log.Info(String.Format("Function: Search - searchString: {0}", searchString));
            return _interviewProcessBusiness.Search(searchString);
        }
    }
}
