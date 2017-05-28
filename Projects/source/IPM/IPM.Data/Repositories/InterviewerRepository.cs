using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface IInterviewerRepository : IRepository<User>
    {
    }
    class InterviewerRepository : RepositoryBase<User>, IInterviewerRepository
    {
        public InterviewerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
