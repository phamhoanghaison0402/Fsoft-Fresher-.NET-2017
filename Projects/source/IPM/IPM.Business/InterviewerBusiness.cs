using System;
using System.Collections.Generic;
using System.Linq;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;

namespace IPM.Business
{
    public interface IInterviewerBusiness
    {
        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        IEnumerable<User> GetAll(string searchString);
    }

    /// <summary>
    /// </summary>
    /// <seealso cref="IPM.Business.IInterviewerBusiness" />
    public class InterviewerBusiness : IInterviewerBusiness
    {
        private const int IntInterviewerRoleId = 3; //Interviewer role id
        private readonly ILog _log = LogManager.GetLogger("IPM.Business.InterviewerBusiness.cs"); //_log for log4net
        private readonly IInterviewerRepository _interviewerRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InterviewerBusiness" /> class.
        /// </summary>
        /// <param name="interviewerRepository">The interviewer repository.</param>
        /// <param name="userRoleRepository"></param>
        public InterviewerBusiness(IInterviewerRepository interviewerRepository, IUserRoleRepository userRoleRepository)
        {
            _interviewerRepository = interviewerRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        public IEnumerable<User> GetAll(string searchString)
        {
            try
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    var listUsers = _interviewerRepository.Get(); // get list all user
                    var listRoles = _userRoleRepository.Get(); // get list all role
                    var listInterviewers = from users in listUsers.ToList()
                                           from roles in listRoles.ToList()
                                           where users.Account == roles.Account && roles.RoleID == IntInterviewerRoleId
                                           select users; // get list all interviwer in list user
                    _log.Info("Get All Interviewer Success");

                    return listInterviewers;
                }

                return _interviewerRepository.Get(x => x.Account.Contains(searchString));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _log.Error(ex.Message + " " + ex.InnerException.Message);
                }
                else
                {
                    throw new Exception("Server is overload!");
                }
            }

            return null; // list null, do not view for user
        }
    }
}
