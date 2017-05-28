using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    public interface IMeetingRequestRepository : IRepository<MeetingRequest>
    {
        
    }

    /// <summary>
    /// Meeting Request repository
    /// </summary>
    public class MeetingRequestRepository : RepositoryBase<MeetingRequest>, IMeetingRequestRepository
    {
        public MeetingRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
