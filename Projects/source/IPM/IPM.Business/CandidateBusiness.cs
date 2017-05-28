using IPM.Common.CustomExceptions;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IPM.Business
{
    public interface ICandidateBusiness : IBusinessBase<Candidate>
    {
        Candidate GetCandidateProfile(int candidateId);
        List<Candidate> Search(string searchData, string concidentData);
        bool DeleteCandidate(int id);
        void EditCandidate(Candidate candidate);
      
        void InsertCandidate(Candidate candidate, HttpPostedFileBase[] uploadedFiles,string uploadPath);

        //Son - Load skill
        IEnumerable<Skill> ListSkill(int id);
        IEnumerable<Skill> GetListSkillByCandidateID(int candidateID);

        //Load certificate
        IEnumerable<Certificate> ListCertificate(int id);
        IEnumerable<Certificate> GetCertificateByCandidateID(int candidateID);
    }

    public class CandidateBusiness : BusinessBase<Candidate>, ICandidateBusiness
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICandidateSkillRepository _candidateSkillRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly ICandidateCertificateRepository _candidateCertificateRepository;

        public CandidateBusiness(
            ICandidateRepository candidateRepository, 
            ICandidateSkillRepository candidateSkillRepository, 
            IUnitOfWork unitOfWork, ISkillRepository skillRepository,
            ICertificateRepository certificateRepository,
            ICandidateCertificateRepository candidateCertificateRepository
            )
            : base(candidateRepository, unitOfWork)
        {
            this._candidateRepository = candidateRepository;
            this._candidateSkillRepository = candidateSkillRepository;
            this._skillRepository = skillRepository;
            this._certificateRepository = certificateRepository;
            this._candidateCertificateRepository = candidateCertificateRepository;
        }

        /// <summary>
        /// Get candidate profile information.
        /// </summary>
        /// <param name="candidateId">Candidate id</param>
        /// <returns></returns>
        public Candidate GetCandidateProfile(int candidateId)
        {
            try
            {
                var candidate = _candidateRepository.GetCandidateProfile(candidateId);

                if (candidate != null)
                {
                    return candidate;
                }

                log.Info("Candidate not found.");
                throw new EntityNotFoundException("Candidate not found.");
            }
            catch (Exception ex)
            {
                log.Error("Could not get candidate profile from database. " + ex.Message);
                throw new DatabaseException("Could not get candidate profile from database.", ex);
            }
        }

        /// <summary>
        /// Search candidate.
        /// </summary>
        /// <param name="searchData">search Data of candidate</param>
        /// <param name="concidentData">concidentData of candidate</param>
        /// <returns>List candidate</returns>
        public List<Candidate> Search(string searchData, string concidentData)
        {     
            var lstCandidate = _candidateRepository.Search(searchData, concidentData);

            if (lstCandidate == null)
            {
                 throw new Exception(string.Format("candidates not found: {0} - {1}",searchData,concidentData));
            }

            return lstCandidate;          
        }

        /// <summary>
        /// Delete candidate.
        /// </summary>
        /// <param name="id">Candidate id</param>
        /// <returns>true | false</returns>
        public bool DeleteCandidate(int id)
        {
            return _candidateRepository.DeleteCandidate(id);     
        }

        /// <summary>
        /// GetAll List Candidate with active = true.
        /// </summary>
        /// <returns>IEnumerable Candidate</returns>
        public override IEnumerable<Candidate> GetAll()
        {
            var lstCandidate = _candidateRepository.Get(c => c.Active == true, null, "Position");

            if(lstCandidate == null)
            {
                throw new Exception("List candidate is null");
            }

            return lstCandidate;
        }

        /// <summary>
        /// Map Candidate's data from Business layer to Repository layer.
        /// </summary>
        /// <param name="candidate">Candidate Model</param>
        public void EditCandidate(Candidate candidate)
        {
            
                var candidateDb = _candidateRepository.GetSingleById(candidate.ID);

                if (candidateDb == null)
                {
                    throw new EntityNotFoundException("Candidate not found.");
                }

                candidateDb.Name = candidate.Name;
                candidateDb.Active = true;
                candidateDb.PositionID = candidate.PositionID;
                candidateDb.Birthdate = candidate.Birthdate;
                candidateDb.Email = candidate.Email;
                candidateDb.Phone = candidate.Phone;
                candidateDb.University = candidate.University;
                candidateDb.Major = candidate.Major;
                candidateDb.Certificate = candidate.Certificate;
                candidateDb.Address = candidate.Address;
                candidateDb.GPA = candidate.GPA;
                candidateDb.InterviewAdminID = candidate.InterviewAdmin.ID;
                candidateDb.IDCard = candidate.IDCard;
                _candidateRepository.Update(candidateDb);
        }       

        /// <summary>
        /// Insert new candidate with file upload.
        /// </summary>
        /// <param name="candidate">Candidate model</param>
        /// <param name="uploadedFiles">Files uploaded</param>
        /// <param name="uploadPath">Upload file path</param>
        public void InsertCandidate(Candidate candidate, HttpPostedFileBase[] uploadedFiles, string uploadPath)
        {
            _candidateRepository.Add(candidate);

            if (uploadedFiles.Length > 0 && uploadedFiles[0] != null)
            {
                SaveFiles(uploadedFiles, candidate, uploadPath);
            }
        }

        /// <summary>
        /// Save candidate's documents.
        /// </summary>
        /// <param name="uploadedFiles">List of document</param>
        /// <param name="candidate">Candidate object</param>
        /// <param name="uploadPath">Upload path</param>
        private void SaveFiles(IEnumerable<HttpPostedFileBase> uploadedFiles, Candidate candidate, string uploadPath)
        {
            try
            {
                // Create folder by candidate id.
                var savePath = Path.Combine(uploadPath, candidate.Name);
                Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath));

                foreach (var file in uploadedFiles)
                {
                    if (file.ContentLength > 0)
                    {
                        var document = new Document
                        {
                            Name = file.FileName,
                            Path = Path.Combine(savePath, file.FileName),
                            CandidateID = candidate.ID
                        };

                        _candidateRepository.AddDocument(document);

                        file.SaveAs(System.Web.Hosting.HostingEnvironment.MapPath("~/" + document.Path));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Save file failed. " + ex.Message);
                throw new FileUploadErrorException("Save file failed.", ex);
            }
        }

        /// <summary>
        /// Load list of skills by Candidate Id.
        /// </summary>
        /// <param name="candidateID">Candidate.ID</param>
        /// <returns></returns>
        public IEnumerable<Skill> GetListSkillByCandidateID(int candidateID)
        {
            var skill = _skillRepository.Get();
            var candidateSkill = _candidateSkillRepository.Get(x => x.CandidateID == candidateID);

            var listSkill = from skillTable in skill
                join candidateSkillTable in candidateSkill
                on skillTable.ID equals candidateSkillTable.SkillID
                select new Skill
                {
                    ID = skillTable.ID,
                    Name = skillTable.Name,
                };

            return listSkill;
        }

        /// <summary>
        /// Load list of skills.
        /// </summary>
        /// <param name="id">Candidate.ID</param>
        /// <returns></returns>
        public IEnumerable<Skill> ListSkill(int id)
        {
            var skill = _skillRepository.Get();
            var candidateSkill = _candidateSkillRepository.Get(x => x.CandidateID == id);
            var results = from t1 in skill
                          where !(from t2 in candidateSkill
                                  select t2.SkillID).Contains(t1.ID)
                select t1;
            return results.ToList();
        }


        /// <summary>
        /// Load list of certificates to view.
        /// </summary>
        /// <param name="candidateID">Candidate.ID</param>
        /// <returns></returns>
        public IEnumerable<Certificate> GetCertificateByCandidateID(int candidateID)
        {
            var certificate = _certificateRepository.Get();
            var candidateCertificate = _candidateCertificateRepository.Get(x => x.CandidateID == candidateID);
            var listCertificates = from certificateTable in certificate
                            join candidateCertificateTable in candidateCertificate
                            on certificateTable.ID equals candidateCertificateTable.CertificateID
                select new Certificate
                {
                    ID = certificateTable.ID,
                    Name = certificateTable.Name,
                };

            return listCertificates;
        }

        /// <summary>
        /// Load list of certificate id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Certificate> ListCertificate(int id)
        {
            var certificate = _certificateRepository.Get();
            var candidateCertificate = _candidateCertificateRepository.Get(x => x.CandidateID == id);
            var results = from t1 in certificate
                          where !(from t2 in candidateCertificate
                                  select t2.CertificateID).Contains(t1.ID)
                select t1;

            return results.ToList();
        }
    }
}