using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Business;
using IPM.Model.Models;
using log4net;

namespace IPM.Service
{
    public interface ICertificateService : IServiceBase<Certificate>
    {
        
    }

    public class CertificateService : ServiceBase<Certificate>, ICertificateService
    {
        private ICertificateBusiness _certificateBusiness;
        
        public CertificateService(ICertificateBusiness certificateBusiness) : base(certificateBusiness)
        {
            this._certificateBusiness = certificateBusiness;
        }
    }
}
