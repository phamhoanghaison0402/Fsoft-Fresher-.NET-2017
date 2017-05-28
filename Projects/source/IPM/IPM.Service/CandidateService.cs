using IPM.Business;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace IPM.Service
{
	public interface ICandidateService : IServiceBase<Candidate>
	{
		Candidate GetCandidateProfile(int candidateId);
		void InsertCandidate(Candidate candidate, HttpPostedFileBase[] uploadedFiles, string uploadPath);
		List<Candidate> Search(string searchData, string concidentData);
		
		void EditCandidate(Candidate candidate);
		bool DeleteCandidate(int id);

        //Son - Load skill
        IEnumerable<Skill> ListSkill(int id);
	    IEnumerable<Skill> GetListSkillByCandidateID(int candidateID);

        //Son - Load certificate
        IEnumerable<Certificate> ListCertificate(int id);
	    IEnumerable<Certificate> GetCertificateByCandidateID(int candidateID);

    }

	public class CandidateService : ServiceBase<Candidate>, ICandidateService
	{
		private readonly ICandidateBusiness _candidateBusiness;

		public CandidateService(ICandidateBusiness candidateBusiness)
			: base(candidateBusiness)
		{
			this._candidateBusiness = candidateBusiness;
		}

		/// <summary>
		/// Get candidate profile information.
		/// </summary>
		/// <param name="candidateId">Candidate id</param>
		/// <returns>Candidate</returns>
		public Candidate GetCandidateProfile(int candidateId)
		{
			return _candidateBusiness.GetCandidateProfile(candidateId);
		}

		/// <summary>
		/// Add new candidate, include uploaded documents.
		/// </summary>
		/// <param name="candidate"></param>
		/// <param name="uploadedFiles"></param>
		/// <param name="uploadPath"></param>
		public void InsertCandidate(Candidate candidate, HttpPostedFileBase[] uploadedFiles, string uploadPath)
		{
			_candidateBusiness.InsertCandidate(candidate, uploadedFiles, uploadPath);
		}

		/// <summary>Get skill's name</summary>
		/// <param name="skillName"></param>
		/// <returns></returns>
		public int GetSkillByName(string skillName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Search candidate.
		/// </summary>
		/// <param name="searchData">search Data of candidate</param>
		/// <param name="concidentData">concidentData of candidate</param>
		/// <returns>List candidate</returns>
		public List<Candidate> Search(string searchData, string concidentData)
		{
			return _candidateBusiness.Search(searchData, concidentData);
		}

		/// <summary>
		/// Delete candidate.
		/// </summary>
		/// <param name="id">Candidate id</param>
		/// <returns>true | false</returns>
		public bool DeleteCandidate(int id)
		{
			return _candidateBusiness.DeleteCandidate(id);
		}		

		/// <summary>
		/// Edit Candidate.
		/// </summary>
		/// <param name="candidate">Candidate Model</param>
		public void EditCandidate(Candidate candidate)
		{
			_candidateBusiness.EditCandidate(candidate);
		}

		/// <summary>
		/// Get list of skills by candidate's Id.
		/// </summary>
		/// <param name="candidateID">Candidate.ID</param>
		/// <returns></returns>
		public IEnumerable<Skill> GetListSkillByCandidateID(int candidateID)
		{
			return _candidateBusiness.GetListSkillByCandidateID(candidateID);
		}

        public IEnumerable<Skill> ListSkill(int id)
        {
            return _candidateBusiness.ListSkill(id);
        }

        /// <summary>
        /// Load list of certificates by Id.
        /// </summary>
        /// <param name="candidateID">Candidate.ID</param>
        /// <returns></returns>
	    public IEnumerable<Certificate> GetCertificateByCandidateID(int candidateID)
	    {
	        return _candidateBusiness.GetCertificateByCandidateID(candidateID);
	    }

        /// <summary>
        /// List of certificates.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
	    public IEnumerable<Certificate> ListCertificate(int id)
	    {
	        return _candidateBusiness.ListCertificate(id);
	    }
    }		
}