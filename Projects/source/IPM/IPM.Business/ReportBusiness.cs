using IPM.Data.Infrastructure;
using IPM.Common.CustomExceptions;
using System;
using System.Collections.Generic;
using IPM.Model.Models;
using IPM.Data.Repositories;
using IPM.Data.ModelReport;
using System.Linq;
using log4net;

namespace IPM.Business
{
    /// <summary>
    /// Interface Report Business Layer
    /// </summary>
    public interface IReportBusiness
    {
        /// <summary>
        /// Get Data report Candiddates with 4 params
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="interviewResult"></param>
        /// <returns></returns>
        IEnumerable<ReportCandidates> GetReportCandidates(string fromDate, string toDate, string name, string interviewResult);

        IEnumerable<ReportInterviewers> GetReportInterviewers(DateTime? FromDate, DateTime? ToDate, string Name, int? InterviewResult);

        /// <summary>
        /// Get list report skill and position.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        List<ReportSkillAndPosition> GetReportSkillAndPosition(string fromDate, string toDate, string name,
            string position);

        /// <summary>
        /// Get Data report Candiddates GST with 4 params
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="interviewResult"></param>
        /// <returns></returns>
        IEnumerable<ReportCandidates> GetReportCandidatesGst(string fromDate, string toDate, string name,
            string interviewResult);
    }

    /// <summary>
    /// Class Report Business Layer
    /// </summary>
    public class ReportBusiness : IReportBusiness
    {
        protected readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IReportRepositoty _reportRepository;

        /// <summary>
        /// Iniatialize Class Report Business
        /// </summary>
        /// <param name="reportRepository"></param>
        public ReportBusiness(IReportRepositoty reportRepository)
        {
            this._reportRepository = reportRepository;
        }

        /// <summary>
        /// Get Data report Candiddates with 4 params
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="interviewResult"></param>
        /// <returns></returns>
        public IEnumerable<ReportCandidates> GetReportCandidates(string fromDate, string toDate, string name, string interviewResult)
        {
            IEnumerable<ReportCandidates> listDataCandidates = null;

            try
            {
                if (interviewResult != null)
                {
                    log.Info("Report with full 3 result: Pass,Fail,None");
                    int result = Convert.ToInt32(interviewResult);
                    listDataCandidates = _reportRepository.GetReportCandidates(fromDate, toDate, name, interviewResult)
                        .Where(i => i.Result.Equals(result))
                        .ToList();
                }
                else
                {
                    log.Info("Report with a result: Pass or Fail or None");
                    listDataCandidates = _reportRepository.GetReportCandidates(fromDate, toDate, name, interviewResult)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not get data report candidates from database. " + ex.Message);
                throw new DatabaseException("Could not get data report candidates rom database.", ex);
            }

            return listDataCandidates;
        }

        public IEnumerable<ReportInterviewers> GetReportInterviewers(DateTime? FromDate, DateTime? ToDate, string Name, int? InterviewResult)
        {
            return _reportRepository.GetReportInterviewers(FromDate, ToDate, Name, InterviewResult);
        }

        /// <summary>
        /// Get data for report Skill and Position
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<ReportSkillAndPosition> GetReportSkillAndPosition(string fromDate, string toDate, string name,
            string position)
        {
            var listReportSkillAndPosition = _reportRepository.GetReportSkillAndPosition(fromDate, toDate, name, position);
            return listReportSkillAndPosition;
        }

        /// <summary>
        /// Get Data report Candiddates GST with 4 params
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="interviewResult"></param>
        /// <returns></returns>
        public IEnumerable<ReportCandidates> GetReportCandidatesGst(string fromDate, string toDate, string name, string interviewResult)
        {
            IEnumerable<ReportCandidates> listDataCandidates = null;

            try
            {
                if (interviewResult != null)
                {
                    log.Info("Report with full 3 result: Pass,Fail,None");
                    int result = Convert.ToInt32(interviewResult);
                    listDataCandidates = _reportRepository.GetReportCandidatesGst(fromDate, toDate, name, interviewResult)
                        .Where(i => i.Result.Equals(result))
                        .ToList();
                }
                else
                {
                    log.Info("Report with a result: Pass or Fail or None");
                    listDataCandidates = _reportRepository.GetReportCandidatesGst(fromDate, toDate, name, interviewResult)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Could not get data report candidates GST from database. " + ex.Message);
                throw new DatabaseException("Could not get data report candidates GST from database.", ex);
            }

            return listDataCandidates;
        }
    }
}
