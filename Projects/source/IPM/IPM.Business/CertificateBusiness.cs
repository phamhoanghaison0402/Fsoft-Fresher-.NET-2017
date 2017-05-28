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
    public interface ICertificateBusiness : IBusinessBase<Certificate>
    {
        
    }

    public class CertificateBusiness : BusinessBase<Certificate>, ICertificateBusiness
    {
        private ICertificateRepository _certificateRepository;
        private IUnitOfWork _unitOfWork;
        public CertificateBusiness(ICertificateRepository certificateRepository, IUnitOfWork unitOfWork)
            : base(certificateRepository, unitOfWork)
        {
            this._certificateRepository = certificateRepository;
            this._unitOfWork = unitOfWork;
        }
    }
}
