using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;


namespace IPM.Business
{
    public  interface ICandidateCertificateBusiness : IBusinessBase<CandidateCertificate>
    {
        IEnumerable<Certificate> GetListCertificateByCandidateId(int candidateId);

        void Update(int candidateId, IEnumerable<int> selectCertificateId);
    }
    public class CandidateCertificateBusiness : BusinessBase<CandidateCertificate>, ICandidateCertificateBusiness
    {
        private readonly ICandidateCertificateRepository _candidateCertificateRepository;
        private readonly ICertificateRepository _certificateRepository;

        public CandidateCertificateBusiness(ICandidateCertificateRepository candidateCertificateRepository, 
            ICertificateRepository certificateRepository, IUnitOfWork unitOfWork)
            : base(candidateCertificateRepository, unitOfWork)
        {
            this._certificateRepository = certificateRepository;
            this._candidateCertificateRepository = candidateCertificateRepository;
        }
        public IEnumerable<Certificate> GetListCertificateByCandidateId(int candidateId)
        {
            var certificates = _certificateRepository.Get();
            var candidateCertificates = _candidateCertificateRepository.Get(x => x.CandidateID == candidateId);

            var listCertificate = from certificateTable in certificates
                                  join candidateCertificateTable in candidateCertificates
                on certificateTable.ID equals candidateCertificateTable.CertificateID
                select new Certificate
                {
                    ID = certificateTable.ID,
                    Name = certificateTable.Name,
                };

            return listCertificate;
        }

        public void Update(int candidateId, IEnumerable<int> selectSkillId)
        {
            var candidateCertificates = _candidateCertificateRepository.Get(x => x.CandidateID == candidateId);
            var results = from candidateCertificateTable in candidateCertificates
                          where !(from SelectCertificateIdTable in selectSkillId
                        select SelectCertificateIdTable).Contains(candidateCertificateTable.CertificateID)
                select candidateCertificateTable.CertificateID;

            if (results.Count() > 0)
            {
                foreach (var skill in results)
                {
                    var candidateCertificateDB = _candidateCertificateRepository.GetSingleByCondition(
                        x => x.CandidateID == candidateId && x.CertificateID == skill);
                    if (candidateCertificateDB != null)
                    {
                        _candidateCertificateRepository.Delete(candidateCertificateDB);
                    }
                }
            }

            foreach (var i in selectSkillId)
            {
                var candidateSkillDB = _candidateCertificateRepository.GetSingleByCondition(
                    x => x.CandidateID == candidateId && x.CertificateID == i);
                if (candidateSkillDB == null)
                {
                    var newCandidateCertificate = new CandidateCertificate();
                    newCandidateCertificate.CertificateID = i;
                    newCandidateCertificate.CandidateID = candidateId;
                    newCandidateCertificate.Active = true;
                    _candidateCertificateRepository.Add(newCandidateCertificate);
                }

            }
        }
    }
}
