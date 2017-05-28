using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Data.Infrastructure;
using IPM.Model.Models;
using IPM.Data.ModelReport;
using System.Data.SqlClient;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// Interface ReportRepository
    /// </summary>
    public interface IReportRepositoty : IRepository<ReportCandidates>
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

        /// <summary>
        /// Get result interview of candidates
        /// </summary>
        /// <param name="listCandidates"></param>
        /// <returns></returns>
        List<ReportCandidates> GetResultCandidate(List<ReportCandidates> listCandidates);

        /// <summary>
        /// Get Report Interviewers
        /// </summary>
        /// <returns></returns>
        IEnumerable<ReportInterviewers> GetReportInterviewers(DateTime? FromDate, DateTime? ToDate, string Name, int? InterviewResult);

        /// <summary>
        /// Get list Report skill and Position
        /// </summary>
        /// <returns></returns>
        List<ReportSkillAndPosition> GetReportSkillAndPosition();

        /// <summary>
        /// Get list Report skill and Position 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        List<ReportSkillAndPosition> GetReportSkillAndPosition(string fromDate, string toDate, string name,
            string position);

        /// <summary>
        /// Get Data report Candiddates with 4 params
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
    /// Class Report Repository
    /// </summary>
    public class ReportRepository : RepositoryBase<ReportCandidates>, IReportRepositoty
    {
        /// <summary>
        /// Initialize class Report Repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public ReportRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Get result interview of candidates
        /// </summary>
        /// <param name="listCandidates"></param>
        /// <returns></returns>
        public List<ReportCandidates> GetResultCandidate(List<ReportCandidates> listCandidates)
        {
            var listTemp = listCandidates;
            var listCandidatesId = listCandidates.Select(i => i.ID).ToList();

            //select all interview of candidates
            var listInterview = DbContext.Interviews.Where(i => listCandidatesId
            .Contains(i.CandidateID)).Select(i => new { ID = i.ID, Result = i.Result, CandidateID = i.CandidateID })
            .ToList();

            foreach (var item in listTemp)
            {
                var candidateResults = listInterview.Where(i => i.CandidateID.Equals(item.ID)).ToList();
                int tempResult = 0;

                ////number of round interview of candidate
                //var count_round = candidateResults.Count();
                var countResultFalse = candidateResults.Where(i => i.Result.Equals(false))
                    .GroupBy(i => i.CandidateID)
                    .Select(g => new { Count = g.Count() }).FirstOrDefault();

                var countResultNull = candidateResults.Where(i => i.Result.Equals(null))
                    .GroupBy(i => i.CandidateID)
                    .Select(g => new { Count = g.Count() }).FirstOrDefault();

                //Candidate fail interview
                if (countResultFalse != null)
                {
                    // 0 : Fail
                    tempResult = 0;
                }

                //Candidate haven't completed interview
                if (countResultNull != null)
                {
                    // 2 : None
                    tempResult = 2;
                }

                //Candidate pass interview
                if (countResultFalse == null && countResultNull == null)
                {
                    // 1 : Pass
                    tempResult = 1;
                }

                item.Result = tempResult;
            }

            return listTemp;
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
            List<ReportCandidates> listDataCandidates = new List<ReportCandidates>();

            if (name == null)
            {
                name = "";
            }

            var candidates = from candidate in DbContext.Candidates
                             join ip in DbContext.InterviewProcesses on candidate.PositionID equals ip.PositionID
                             where candidate.Name.Contains(name)
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = ip.StartDate
                             };

            if (fromDate != null && toDate != null)
            {
                DateTime fd = DateTime.Parse(fromDate);
                DateTime td = DateTime.Parse(toDate);

                candidates = from candidate in candidates
                             where candidate.Date >= fd && candidate.Date <= td
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };

            }

            else if (fromDate != null)
            {
                DateTime fd = DateTime.Parse(fromDate);

                candidates = from candidate in candidates
                             where candidate.Date >= fd
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };
            }

            else if (toDate != null)
            {
                DateTime td = DateTime.Parse(toDate);

                candidates = from candidate in candidates
                             where candidate.Date <= td
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };
            }

            listDataCandidates = GetResultCandidate(candidates.Select(i => new ReportCandidates()
            {
                ID = i.ID,
                Name = i.Name,
                InterviewAdminID = i.InterviewAdminID.Value,
                PositionID = i.PositionID,
                Birthdate = i.Birthdate,
                InterviewAdmin = i.InterviewAdmin,
                Position = i.Position,
                Date = i.Date
            }).ToList());

            return listDataCandidates;
        }

        /// <summary>
        /// Get Interviewer Report
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReportInterviewers> GetReportInterviewers(DateTime? FromDate, DateTime? ToDate, string Name, int? InterviewResult)
        {            
            var result = (from interview in DbContext.Interviews
                          join user in DbContext.Users on interview.InterviewerID equals user.ID 
                          join candidate in DbContext.Candidates on interview.CandidateID equals candidate.ID                          
                          where user.Username == null ? true : user.Username.Contains(Name)
                          select new { interview, user, candidate });
            //where (InterviewResult == null) ? true : (interview.Result == InterviewResult)
            //(DbContext.InterviewProcesses.Where(e => ((FromDate == null) ? true : (e.StartDate >= FromDate) && (ToDate == null) ? true : (e.StartDate <= ToDate)) && e.PositionID == candidate.Position.ID).Select(e => e.PositionID).Distinct()).Contains(candidate.Position.ID)
            //where (candidate.Position.InterviewProcesses.Where(e => ((FromDate == null) ? true : (e.StartDate >= FromDate) && (ToDate == null) ? true : (e.StartDate <= ToDate))                            

            var resultByDateFilter = (from position in DbContext.Positions
                                      join re in result on position.ID equals re.candidate.PositionID
                                      join interviewprocess in DbContext.InterviewProcesses on position.ID equals interviewprocess.PositionID
                                      where ((FromDate == null) ? true : (interviewprocess.StartDate >= FromDate) && (ToDate == null) ? true : (interviewprocess.StartDate <= ToDate))
                                      select new { interview = re.interview, user = re.user, candidate = re.candidate}
                                      ); 
             var interviewersList = resultByDateFilter.Select(e => e.interview.InterviewerID).Distinct().ToList();            
            List<ReportInterviewers> listReportInterviewer = new List<ReportInterviewers>();
            for (int i = 0; i < interviewersList.Count(); i++)
            {                
                var interviewerID = interviewersList[i];
                var interviewerReportTemp = resultByDateFilter.Where(e => (e.interview.InterviewerID == interviewerID)).Select(e=>new { e.interview, e.user, e.candidate });
                var totalcandidates =  interviewerReportTemp.Select(e => new { e.interview.CandidateID }).Distinct().ToList(); //, e.interview.InterviewerID });
                int numofcandidates = totalcandidates.Count();
                int numofpasscandidates = 0;
                int numoffailcandidates = 0;
                int numofnonepassnonefail = 0;
                bool breakIf = false;
                for (int j = 0; j < numofcandidates; j++)
                {
                    var candidate = totalcandidates[j]; 
                    foreach (var record in resultByDateFilter.Where(e => e.interview.CandidateID == candidate.CandidateID))
                    {
                        if(record.interview.Result == false)
                        {
                            numoffailcandidates++;
                            breakIf = true;
                            break;
                        }
                        if (record.interview.Result == null)
                        {
                            numofnonepassnonefail++;
                            breakIf = true;
                            break; //dot not count null result
                        }                        
                    }
                    if(!breakIf) numofpasscandidates++;
                    breakIf = false;
                }
                //var passcandidates = interviewerReportTemp.Where(e=>(bool)e.interview.Result == true).GroupBy(e => new { e.interview.CandidateID, e.interview.InterviewerID, e.interview.Result });
                //var failcandidates = interviewerReportTemp.Where(e=>(bool)e.interview.Result == false).GroupBy(e => new { e.interview.CandidateID, e.interview.InterviewerID, e.interview.Result});                                

                var interviewerReport = interviewerReportTemp.Select(e => new { e.interview.InterviewerID, e.user.Username, Account = e.user.Account, NumOfCandidates = numofcandidates, PassFail = numofpasscandidates.ToString() + "/" + numoffailcandidates.ToString() + "/" + numofnonepassnonefail.ToString()}).Distinct();
                //neu co chon filter Pass/Fail/None, thi phai loc ben duoi
                if(InterviewResult == 1 && numofpasscandidates == 0)
                {
                    //Neu numofpasscandidate == 0 thi ko add                    
                    continue;
                }
                if(InterviewResult == 0 && numoffailcandidates == 0)
                {
                    continue;
                }
                if (InterviewResult == 2 && numofnonepassnonefail == 0)
                {
                    continue;
                }
                listReportInterviewer.Add(new ReportInterviewers()
                {
                    ID = (int)interviewerReport.Select(e=>e.InterviewerID).SingleOrDefault(),
                    Name = interviewerReport.Select(e => e.Username).SingleOrDefault(),
                    Account = interviewerReport.Select(e => e.Account).SingleOrDefault(),
                    NumOfCandidates = numofcandidates,
                    PassFail = numofpasscandidates.ToString() + "/" + numoffailcandidates.ToString() + "/" + numofnonepassnonefail.ToString()
                });
            }
            
            return listReportInterviewer;
        }

        public List<ReportSkillAndPosition> GetReportSkillAndPosition()
        {
            List<ReportSkillAndPosition> list = new List<ReportSkillAndPosition>();
            var listSkill = from candidateSkill in DbContext.CandidateSkills
                            join skill in DbContext.Skills on candidateSkill.SkillID equals skill.ID
                            select new
                            {
                                CandidateID = candidateSkill.CandidateID,
                                SkillName = skill.Name
                            };
            var listCandidate = from candidate in DbContext.Candidates
                                join position in DbContext.Positions on candidate.PositionID equals position.ID
                                join interviewProcess in DbContext.InterviewProcesses on position.ID equals interviewProcess.PositionID
                                select new
                                {
                                    CandidateID = candidate.ID,
                                    CandidateName = candidate.Name,
                                    StartDate = interviewProcess.StartDate,
                                    PositionName = position.Name,
                                    PositionID = position.ID
                                };

            foreach (var item in listCandidate)
            {
                ReportSkillAndPosition reportSkillAndPosition = new ReportSkillAndPosition();
                reportSkillAndPosition.Name = item.CandidateName;
                reportSkillAndPosition.StartDate = item.StartDate;
                reportSkillAndPosition.Position = item.PositionName;
                reportSkillAndPosition.PositionID = item.PositionID;
                var listSkillFlowCandidateId = listSkill.Where(x => x.CandidateID == item.CandidateID);
                string name = "";
                if (listSkillFlowCandidateId.Count() != 0)
                {
                    foreach (var item1 in listSkillFlowCandidateId)
                    {
                        name = name + item1.SkillName + ",";
                    }
                    name = name.Substring(0, name.Length - 1);
                }

                reportSkillAndPosition.Skills = name;
                list.Add(reportSkillAndPosition);
            }

            return list;
        }

        public List<ReportSkillAndPosition> GetReportSkillAndPosition(string fromDate, string toDate, string name, String position)
        {
            List<ReportSkillAndPosition> list = new List<ReportSkillAndPosition>();
            List<ReportSkillAndPosition> listFilter = new List<ReportSkillAndPosition>();
            list = GetReportSkillAndPosition();
            if (position != null)
            {
                int positionId = int.Parse(position);

                list = (from mt in list
                        where (mt.PositionID == positionId)
                        select mt).ToList();
            }


            if (fromDate != null && toDate == null)
            {
                DateTime fd = DateTime.Parse(fromDate);
                listFilter = (from mt in list
                              where (mt.Name == name || name == null) && (mt.StartDate >= fd)
                              select mt).ToList();
            }
            else if (fromDate == null && toDate != null)
            {
                DateTime td = DateTime.Parse(toDate);
                listFilter = (from mt in list
                              where (mt.Name == name || name == null) && (mt.StartDate <= td)
                              select mt).ToList();
            }
            else if (fromDate != null && toDate != null)
            {
                DateTime fd = DateTime.Parse(fromDate);
                DateTime td = DateTime.Parse(toDate);
                listFilter = (from mt in list
                              where (mt.Name == name || name == null) && (mt.StartDate <= td && mt.StartDate >= fd)
                              select mt).ToList();
            }
            else
            {
                listFilter = (from mt in list
                              where (mt.Name == name || name == null)
                              select mt).ToList();
            }

            return listFilter;
        }

        /// <summary>
        /// Get Candidates have Certificate
        /// </summary>
        /// <param name="listCandidates"></param>
        /// <returns></returns>
        public List<ReportCandidates> GetCandidateGst(List<ReportCandidates> listCandidates)
        {
            var listTemp = listCandidates;
            var listCandidatesId = listCandidates.Select(i => i.ID).ToList();

            var candidatesGst = DbContext.CandidateCertificates.Join(DbContext.Certificates,
                    cc => cc.CertificateID, cer => cer.ID, (cc, cer) => new
                    {
                        CandidateCertificates = cc,
                        Certificates = cer
                    })
                    .Where(i => listCandidatesId.Contains(i.CandidateCertificates.CandidateID))
                    .Select(i => new
                    {
                        ID = i.CandidateCertificates.CandidateID,
                        certificateName = i.Certificates.Name,
                    }).ToList();

            foreach (var item in listTemp)
            {
                string certificateName = "";
                var certificate = candidatesGst.Where(i => i.ID.Equals(item.ID)).ToList();
                foreach (var i in certificate)
                {
                    certificateName = certificateName + i.certificateName + ",";
                }

                if (certificateName != "")
                {
                    item.Certificate = certificateName.Substring(0, certificateName.Length - 1);
                }
                else
                {
                    item.Certificate = certificateName;
                }
            }

            return listTemp;
        }

        /// <summary>
        /// Get Data report Candiddates with 4 params
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="name"></param>
        /// <param name="interviewResult"></param>
        /// <returns></returns>
        public IEnumerable<ReportCandidates> GetReportCandidatesGst(string fromDate, string toDate, string name, string interviewResult)
        {
            List<ReportCandidates> listDataCandidates = new List<ReportCandidates>();

            if (name == null)
            {
                name = "";
            }

            var candidates = from candidate in DbContext.Candidates
                             join ip in DbContext.InterviewProcesses on candidate.PositionID equals ip.PositionID
                             where candidate.Name.Contains(name)
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = ip.StartDate
                             };

            if (fromDate != null && toDate != null)
            {
                DateTime fd = DateTime.Parse(fromDate);
                DateTime td = DateTime.Parse(toDate);

                candidates = from candidate in candidates
                             where candidate.Date >= fd && candidate.Date <= td
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };

            }

            else if (fromDate != null)
            {
                DateTime fd = DateTime.Parse(fromDate);

                candidates = from candidate in candidates
                             where candidate.Date >= fd
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };
            }

            else if (toDate != null)
            {
                DateTime td = DateTime.Parse(toDate);

                candidates = from candidate in candidates
                             where candidate.Date <= td
                             select new
                             {
                                 ID = candidate.ID,
                                 Name = candidate.Name,
                                 InterviewAdminID = candidate.InterviewAdminID,
                                 PositionID = candidate.PositionID,
                                 Birthdate = candidate.Birthdate,
                                 InterviewAdmin = candidate.InterviewAdmin,
                                 Position = candidate.Position,
                                 Date = candidate.Date
                             };
            }

            //Check candidates Gst
            listDataCandidates = GetCandidateGst(candidates.Select(i => new ReportCandidates()
            {
                ID = i.ID,
                Name = i.Name,
                InterviewAdminID = i.InterviewAdminID.Value,
                PositionID = i.PositionID,
                Birthdate = i.Birthdate,
                InterviewAdmin = i.InterviewAdmin,
                Position = i.Position,
                Date = i.Date
            }).ToList());

            //Get result each candidate
            listDataCandidates = GetResultCandidate(listDataCandidates);

            return listDataCandidates;
        }

    }
}
