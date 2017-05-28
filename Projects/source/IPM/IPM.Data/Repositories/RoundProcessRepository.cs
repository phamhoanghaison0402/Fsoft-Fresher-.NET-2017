using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface IRoundProcessRepository : IRepository<RoundProcess>
    {
        /// <summary>
        /// Get roundProcess by processId and roundId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        RoundProcess GetByRoundIdProcessId(int processId, int roundId);
        /// <summary>
        /// Get list by process Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        IEnumerable<RoundProcess> GetByProcessId(int processId);
        /// <summary>
        /// Get list by round id
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        IEnumerable<RoundProcess> GetByRoundId(int roundId);
    }
    /// <summary>
    /// Round process repository
    /// </summary>
    public class RoundProcessRepository: RepositoryBase<RoundProcess>, IRoundProcessRepository
    {
        public RoundProcessRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }


        /// <summary>
        /// Get roundProcess by processId and roundId
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public RoundProcess GetByRoundIdProcessId(int processId, int roundId)
        {
            return DbContext.RoundProcesses.FirstOrDefault(r => r.InterviewProcessID == processId && r.InterviewRoundID == roundId);
        }


        /// <summary>
        /// Get list by process Id
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public IEnumerable<RoundProcess> GetByProcessId(int processId)
        {
            return DbContext.RoundProcesses.Where(x => x.InterviewProcessID == processId);
        }


        /// <summary>
        /// Get list by round id
        /// </summary>
        /// <param name="roundId"></param>
        /// <returns></returns>
        public IEnumerable<RoundProcess> GetByRoundId(int roundId)
        {
            return DbContext.RoundProcesses.Where(x => x.InterviewRoundID == roundId);
        }
    }
}