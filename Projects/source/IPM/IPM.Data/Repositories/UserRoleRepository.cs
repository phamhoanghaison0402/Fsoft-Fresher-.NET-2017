using IPM.Data.Infrastructure;
using IPM.Model.Models;

namespace IPM.Data.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {

    }
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        // specific
    }
}
