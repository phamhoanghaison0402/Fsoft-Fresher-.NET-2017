using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Business
{
    
    public interface IRoundProcessBusiness : IBusinessBase<RoundProcess>
    {
        /// <summary>
        /// Get roundProcess by processId and roundId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        RoundProcess GetByRoundIdProcessId(int processId, int roundId);
        /// <summary>
        /// Get list by process id
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
        /// Get list by round ID
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        IEnumerable<InterviewProcess> GetByRoundId(int roundId);
        
    }
    /// <summary>
    /// Round process business
    /// </summary>
    public class RoundProcessBusiness : BusinessBase<RoundProcess>, IRoundProcessBusiness
    {
        private IRoundProcessRepository _roundProcessRepository;
        private IUnitOfWork _unitOfWork;
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IInterviewProcessRepository _interviewProcessRepository;

        public RoundProcessBusiness(IUnitOfWork unitOfWork, IRoundProcessRepository roundProcessRepository,IInterviewProcessRepository interviewProcessRepository)
            :base(roundProcessRepository, unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._roundProcessRepository = roundProcessRepository;
            this._interviewProcessRepository = interviewProcessRepository;
        }
        

        /// <summary>
        /// Get round process by processID and roundID
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public RoundProcess GetByRoundIdProcessId(int processId, int roundId)
        {
            try
            {
                return _roundProcessRepository.GetByRoundIdProcessId(processId, roundId);
            }
            catch (Exception exc)
            {
                log.Error(exc.Message + " " + exc.InnerException.Message);
                throw new Exception("Round Process not found");
            }
        }


        /// <summary>
        /// Get list interviewProcess by ProcessID
        /// </summary>
        /// <param name="processId">ProcessID</param>
        /// <returns></returns>
        public IEnumerable<RoundProcess> GetByProcessId(int processId)
        {
            try
            {
                return _roundProcessRepository.GetByProcessId(processId);
            }
            catch (Exception exc)
            {
                log.Error(exc.Message + " " + exc.InnerException.Message);
                throw new Exception("Round Process not found");
            }
        }


        /// <summary>
        /// Delete round process
        /// </summary>
        /// <param name="roundProcess"></param>
        /// <returns></returns>
        public RoundProcess Delete(RoundProcess roundProcess)
        {
            try
            {
                return _roundProcessRepository.Delete(roundProcess);
            }
            catch (Exception exc)
            {
                log.Error(exc.Message + " " + exc.InnerException.Message);
                throw new Exception("Round Process not found");
            }
        }


        /// <summary>
        /// Get list round process by round id
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public IEnumerable<InterviewProcess> GetByRoundId(int roundId)
        {
            var interviewPrcess = _interviewProcessRepository.Get();
            var roundPorcess = _roundProcessRepository.GetByRoundId(roundId);

            var tempData = from t1 in interviewPrcess
                           join t2 in roundPorcess
                           on t1.ID equals t2.InterviewProcessID
                           // where t2.InterviewRoundID == roundId
                           select new InterviewProcess()
                           {
                               ID = t1.ID,
                               PositionID = t1.PositionID,
                               StartDate = t1.StartDate,
                               Active = t1.Active,
                               ProcessName = t1.ProcessName,
                           };

            return tempData;
        }
    }
}
