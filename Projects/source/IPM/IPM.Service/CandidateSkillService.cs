using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Business;
using IPM.Model.Models;

namespace IPM.Service
{
    public interface ICandidateSkillService : IServiceBase<CandidateSkill>
    {
        IEnumerable<Skill> GetListSkillByCandidateId(int candidateId);

        void Update(int candidateId, IEnumerable<int> selectSkillId);

        void UpdateCandidateCertificate(int candidateId, IEnumerable<int> selectCertificateId);
    }

    public class CandidateSkillService : ServiceBase<CandidateSkill>, ICandidateSkillService
    {
        private readonly ICandidateSkillBusiness _candidateSkillBusiness;

        public CandidateSkillService(ICandidateSkillBusiness candidateSkillBusiness) 
            : base(candidateSkillBusiness)
        {
            this._candidateSkillBusiness = candidateSkillBusiness;
        }

        public IEnumerable<Skill> GetListSkillByCandidateId(int candidateId)
        {
            return _candidateSkillBusiness.GetListSkillByCandidateId(candidateId);
        }

        public void Update(int candidateId, IEnumerable<int> selectSkillId)
        {
            _candidateSkillBusiness.Update(candidateId, selectSkillId);
        }

        public void UpdateCandidateCertificate(int candidateId, IEnumerable<int> selectCertificateId)
        {
            _candidateSkillBusiness.UpdateCandidateCertificate(candidateId, selectCertificateId);
        }
    }
}
