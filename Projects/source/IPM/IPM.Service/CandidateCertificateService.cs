using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Business;
using IPM.Model.Models;

namespace IPM.Service
{
    public interface ICandidateCertificateService : IServiceBase<CandidateCertificate>
    {
        IEnumerable<Certificate> GetListCertificateByCandidateId(int candidateId);

        void Update(int candidateId, IEnumerable<int> selectCertificateId); 
    }

    public class CandidateCertificateService : ServiceBase<CandidateCertificate>, ICandidateCertificateService
    {
        private readonly ICandidateCertificateBusiness _candidateCertificateBusiness;

        public CandidateCertificateService(ICandidateCertificateBusiness candidateCertificateBusiness) 
            : base(candidateCertificateBusiness)
        {
            this._candidateCertificateBusiness = candidateCertificateBusiness;
        }

        public IEnumerable<Certificate> GetListCertificateByCandidateId(int candidateId)
        {
            return _candidateCertificateBusiness.GetListCertificateByCandidateId(candidateId);
        }

        public void Update(int candidateId, IEnumerable<int> selectCertificateId)
        {
            _candidateCertificateBusiness.Update(candidateId, selectCertificateId);
        }
    }
}
