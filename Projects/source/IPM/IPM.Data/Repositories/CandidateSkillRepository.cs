using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface ICandidateSkillRepository : IRepository<CandidateSkill>
    {

    }
    public class CandidateSkillRepository : RepositoryBase<CandidateSkill>, ICandidateSkillRepository
    {
        public CandidateSkillRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // specific
    }
}
