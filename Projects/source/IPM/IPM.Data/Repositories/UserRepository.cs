using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
    public class UserRepository:RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // specific
    }
}
