using IPM.Business;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Service
{
    public interface IRoundProcessService : IServiceBase<RoundProcess>
    {
        /// <summary>
        /// Get roundProcess by processId and roundId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        RoundProcess GetById(int processId, int roundId);
        /// <summary>
        /// Get list process id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        IEnumerable<RoundProcess> GetByProcessId(int processId);
        /// <summary>
        /// Delete roundProcess
        /// </summary>
        /// <param name="roundProcess"></param>
        /// <returns></returns>
        RoundProcess Delete(RoundProcess roundProcess);
        /// <summary>
        /// Get list by round id
        /// </summary>
        /// <param name="roundId">round id</param>
        /// <returns>list interview round</returns>
        IEnumerable<InterviewProcess> GetByRoundId(int roundId);
    }
    /// <summary>
    /// Round process service
    /// </summary>
    public class RoundProcessService : ServiceBase<RoundProcess>, IRoundProcessService
    {
        private IRoundProcessBusiness _roundProcessBusiness;

        public RoundProcessService(IRoundProcessBusiness roundProcessBusiness)
            : base(roundProcessBusiness)
        {
            this._roundProcessBusiness = roundProcessBusiness;
        }


        /// <summary>
        /// Get roundProcess by processId and roundId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public RoundProcess GetById(int processId, int roundId)
        {
            return _roundProcessBusiness.GetByRoundIdProcessId(processId, roundId);
        }


        /// <summary>
        /// Get list process id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public IEnumerable<RoundProcess> GetByProcessId(int processId)
        {
            return _roundProcessBusiness.GetByProcessId(processId);
        }


        /// <summary>
        /// Delete roundProcess
        /// </summary>
        /// <param name="roundProcess"></param>
        /// <returns></returns>
        public RoundProcess Delete(RoundProcess roundProcess)
        {
            return _roundProcessBusiness.Delete(roundProcess);
        }


        /// <summary>
        /// Get list by round id
        /// </summary>
        /// <param name="roundId">round id</param>
        /// <returns>list interview round</returns>
        public IEnumerable<InterviewProcess> GetByRoundId(int roundId)
        {
            return _roundProcessBusiness.GetByRoundId(roundId);
        }
    }
}
