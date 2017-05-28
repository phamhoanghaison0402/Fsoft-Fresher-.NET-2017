using IPM.Data.Infrastructure;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IPM.Common.CustomExceptions;

namespace IPM.Data.Repositories
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
        Candidate GetCandidateProfile(int candidateId);
        void AddDocument(Document document);
        List<Candidate> Search(string searchData, string concidentData);
        bool DeleteCandidate(int id);
       
    }
    public class CandidateRepository : RepositoryBase<Candidate>, ICandidateRepository
    {
        private readonly ILog _log = LogManager.GetLogger("log");
        public CandidateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        
        /// <summary>
        /// Get candidate profile information
        /// </summary>
        /// <param name="candidateId">Candidate id</param>
        /// <returns>Candidate</returns>
        public Candidate GetCandidateProfile(int candidateId)
        {
            return DbContext.Candidates
                .Include(c => c.CandidateSkills.Select(cs => cs.Skill))
                .Include(c => c.CandidateCertificates.Select(cg => cg.Certificate))
                .Include(c => c.InterviewAdmin)
                .Include(c => c.Position)
                .Include(c => c.Documents)
                .FirstOrDefault(c => c.ID == candidateId);
        }

        /// <summary>
        /// Add new document of candidate into database
        /// </summary>
        /// <param name="document">New document</param>
        public void AddDocument(Document document)
        {
            DbContext.Documents.Add(document);
        }

        /// <summary>
        /// Search candidate
        /// </summary>
        /// <param name="searchData">search Data of candidate</param>
        /// <param name="concidentData">concidentData of candidate</param>
        /// <returns>List candidate</returns>
        public List<Candidate> Search(string searchData, string concidentData)
        {
            List<Candidate> lstCandidate = new List<Candidate>();
            try
            {
                lstCandidate = DbContext.Candidates.Where(c => c.Active
                                                               && (c.Name.Contains(searchData) 
                                                                   || c.Birthdate.ToString().Contains(searchData)
                                                                   || c.Email.Contains(searchData) 
                                                                   || c.Phone.Contains(searchData)
                                                                   || c.University.Contains(searchData) 
                                                                   || c.Position.Name.Contains(searchData)))
                                                                    .ToList();

                if (concidentData != null)
                {
                    List<Candidate> lstConcident = new List<Candidate>();
                    foreach (var item in concidentData.Split
                        (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
                    {

                        var temp = lstCandidate.Where(c => c.ConcidentStatus == int.Parse(item)).ToList();
                        lstConcident.AddRange(temp);

                    }

                    return lstConcident;
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Search candidate ({0} - {1}) fail: ", searchData, concidentData), e);
            }

            return lstCandidate;
        }

        /// <summary>
        /// Delete candidate: not delete out of database, update active = false
        /// </summary>
        /// <param name="id">Candidate id</param>
        /// <returns>true | false</returns>
        public bool DeleteCandidate(int id)
        {
            try
            {
                var candidate = DbContext.Candidates.SingleOrDefault(c => c.ID == id);
                if (candidate != null)
                {
                    candidate.Active = false;
                    return true;
                }
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Delete candidate id {0} fail: ", id), e);
            }
            return false;
        }
        
    }
}