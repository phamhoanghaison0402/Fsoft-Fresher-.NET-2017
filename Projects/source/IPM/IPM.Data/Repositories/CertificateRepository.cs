using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Data.Infrastructure;
using IPM.Model.Models;


namespace IPM.Data.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        
    }

    public class CertificateRepository : RepositoryBase<Certificate>, ICertificateRepository
    {
        public CertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
