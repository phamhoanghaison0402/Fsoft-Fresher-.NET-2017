using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {

    }
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // specific
    }
}
