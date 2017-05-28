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
    public  interface ICandidateSkillBusiness : IBusinessBase<CandidateSkill>
    {
        IEnumerable<Skill> GetListSkillByCandidateId(int candidateId);

        void Update(int candidateId, IEnumerable<int> selectSkillId);

        void UpdateCandidateCertificate(int candidateId, IEnumerable<int> selectCertificateId);
    }
    public class CandidateSkillBusiness : BusinessBase<CandidateSkill>, ICandidateSkillBusiness
    {
        private readonly ICandidateSkillRepository _candidateSkillRepository;
        private readonly ICandidateCertificateRepository _candidateCertificateRepository;
        private readonly ISkillRepository _skillRepository;

        public CandidateSkillBusiness(ICandidateSkillRepository candidateSkillRepository, 
            ISkillRepository skillRepository, 
            IUnitOfWork unitOfWork, 
            ICandidateCertificateRepository candidateCertificateRepository
            ) : base(candidateSkillRepository, unitOfWork)
        {
            this._skillRepository = skillRepository;
            this._candidateSkillRepository = candidateSkillRepository;
            this._candidateCertificateRepository = candidateCertificateRepository;
        }
        public IEnumerable<Skill> GetListSkillByCandidateId(int candidateId)
        {
            var skill = _skillRepository.Get();
            var candidateSkill = _candidateSkillRepository.Get(x => x.CandidateID == candidateId);

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

        //Update into table candidate skill
        public void Update(int candidateId, IEnumerable<int> selectSkillId)
        {
            var candidateSkill = _candidateSkillRepository.Get(x => x.CandidateID == candidateId);
            var results = from candidateSkillTable in candidateSkill
                where !(from SelectSkillIdTable in selectSkillId
                        select SelectSkillIdTable).Contains(candidateSkillTable.SkillID)
                select candidateSkillTable.SkillID;

            if (results.Count() > 0)
            {
                foreach (var skill in results)
                {
                    var candidateSkillDB = _candidateSkillRepository.GetSingleByCondition(x => x.CandidateID == candidateId && x.SkillID == skill);
                    if (candidateSkillDB != null)
                    {
                        _candidateSkillRepository.Delete(candidateSkillDB);
                    }
                }
            }

            foreach (var i in selectSkillId)
            {
                var candidateSkillDB = _candidateSkillRepository.GetSingleByCondition(x => x.CandidateID == candidateId && x.SkillID == i);

                if (candidateSkillDB == null)
                {
                    var newCandidateSkill = new CandidateSkill();
                    newCandidateSkill.SkillID = i;
                    newCandidateSkill.CandidateID = candidateId;
                    newCandidateSkill.Active = true;
                    _candidateSkillRepository.Add(newCandidateSkill);
                }
            }
        }

        //Update into table candidate certificate
        public void UpdateCandidateCertificate(int candidateId, IEnumerable<int> selectCertificateId)
        {
            var candidateCertificate = _candidateCertificateRepository.Get(x => x.CandidateID == candidateId);
            var results = from candidateCertificateTable in candidateCertificate
                          where !(from SelectCertificateIdTable in selectCertificateId
                                  select SelectCertificateIdTable).Contains(candidateCertificateTable.CertificateID)
                select candidateCertificateTable.CertificateID;

            if (results.Count() > 0)
            {
                foreach (var certificate in results)
                {
                    var candidateCertificateDB = _candidateCertificateRepository.GetSingleByCondition(
                        x => x.CandidateID == candidateId && x.CertificateID == certificate);
                    if (candidateCertificateDB != null)
                    {
                        _candidateCertificateRepository.Delete(candidateCertificateDB);
                    }
                }
            }

            foreach (var i in selectCertificateId)
            {
                var candidateCertificateDB = _candidateCertificateRepository.GetSingleByCondition(
                    x => x.CandidateID == candidateId && x.CertificateID == i);
                if (candidateCertificateDB == null)
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
