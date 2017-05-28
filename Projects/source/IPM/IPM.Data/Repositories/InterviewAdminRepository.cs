using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPM.Data.Repositories
{
    public interface IInterviewAdminRepository : IRepository<User>
    {

        /// <summary>
        /// Get interview admin by user role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IEnumerable<User> GetInterviewAdminByRole(int roleId, string search);
    }

    /// <summary>
    /// Class interview admin repository.
    /// </summary>
    public class InterviewAdminRepository : RepositoryBase<User>, IInterviewAdminRepository
    {

        private IEnumerable<User> interviewAdmin;

        /// <summary>
        /// Constructor auto call of interview admin repository
        /// </summary>
        /// <param name="dbFactory"></param>
        public InterviewAdminRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        /// <summary>
        /// Get interview admin by user role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<User> GetInterviewAdminByRole(int roleId, string search)
        {
            var account = DbContext.UserRoles.Where(u => u.RoleID == roleId).Select(u => u.Account);
            
            if (!String.IsNullOrEmpty(search))
            {
                interviewAdmin = from admin in DbContext.Users
                                 where account.Contains(admin.Account)
                                        && (admin.Username.Contains(search)
                                            || admin.Account.Contains(search))
                                 select admin;
            }
            else
            {
                interviewAdmin = DbContext.Users.Where(u => account.Contains(u.Account)).ToList();
            }

            return interviewAdmin;
        }
    }
}