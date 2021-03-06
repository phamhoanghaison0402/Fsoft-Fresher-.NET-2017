﻿using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface ICandidateCertificateRepository : IRepository<CandidateCertificate>
    {

    }
    public class CandidateCertificateRepository : RepositoryBase<CandidateCertificate>, ICandidateCertificateRepository
    {
        public CandidateCertificateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // specific
    }
}
