using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Business;
using IPM.Data.ModelReport;

namespace IPM.Service
{
    /// <summary>
    /// Interface Report Service Layer
    /// </summary>
    public interface IReportService
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
        /// Get data for report Skill and Position.
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
    public class ReportService : IReportService
    {
        private readonly IReportBusiness _reportBusiness;

        /// <summary>
        /// Iniatialize Class Report Service
        /// </summary>
        /// <param name="reportBusiness"></param>
        public ReportService(IReportBusiness reportBusiness)
        {
            this._reportBusiness = reportBusiness;
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
            return _reportBusiness.GetReportCandidates(fromDate, toDate, name, interviewResult);
        }

        public IEnumerable<ReportInterviewers> GetReportInterviewers(DateTime? FromDate, DateTime? ToDate, string Name, int? InterviewResult)
        {
            return _reportBusiness.GetReportInterviewers(FromDate, ToDate, Name, InterviewResult);
        }

        /// <summary>
        /// Get data for report Skill and Position
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<ReportSkillAndPosition> GetReportSkillAndPosition(string fromDate, string toDate, string name, string position)
        {
            return _reportBusiness.GetReportSkillAndPosition(fromDate, toDate, name, position);
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
            return _reportBusiness.GetReportCandidatesGst(fromDate, toDate, name, interviewResult);
        }
    }
}
