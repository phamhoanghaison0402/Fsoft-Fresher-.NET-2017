using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System.Collections.Generic;
using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace IPM.Business
{
    /// <summary>
    ///Initializes Interface IUserBusiness
    /// </summary>
    public interface IUserBusiness
    {  
        bool CheckLogin(string account, string password);
        bool CheckExistAccount(string account);
        bool CheckActive(string account);
        string GetFullName(string account);
        bool Insert(User user, List<int> role);
        bool Edit(User user, List<int> role);
        bool CheckAccountFpt(string account);
        List<InfoUser> GetUser();
        InfoUser GetUserByAccount(string account);
        int GetIDbyAccount(string account);
        List<InfoUser> GetListUserByAccount(string searchString);
    }

    /// <summary>
    /// Initializes Class InfoUser
    /// </summary>
    public class InfoUser
    {
        private string _account;
        private string _fullName;
        private bool _active;
        private List<Roles> _roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoUser"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="roles">The roles.</param>
        public InfoUser(string account, List<Roles> roles)
        {
            _account = account;
            _roles = roles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoUser"/> class.
        /// </summary>
        public InfoUser()
        {
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return _fullName;
        }

        /// <summary>
        /// Sets the full name.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        public void SetFullName(string fullName)
        {
            _fullName = fullName;
        }

        /// <summary>
        /// Gets the active.
        /// </summary>
        /// <returns></returns>
        public bool GetActive()
        {
            return _active;
        }

        /// <summary>
        /// Sets the active.
        /// </summary>
        /// <param name="active">if set to <c>true</c> [active].</param>
        public void SetActive(bool active)
        {
            _active = active;
        }

        /// <summary>
        /// Gets the account.
        /// </summary>
        /// <returns></returns>
        public string GetAccount()
        {
            return _account;
        }

        /// <summary>
        /// Sets the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void SetAccount(string account)
        {
            _account = account;
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns></returns>
        public List<Roles> GetRoles()
        {
            return _roles;
        }

        /// <summary>
        /// Sets the roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public void SetRoles(List<Roles> roles)
        {
            _roles = roles;
        }
    }

    /// <summary>
    ///  Initializes a new instance of the <see cref="Roles"/> class.
    /// </summary>
    public class Roles
    {
        private string _roles;

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <returns></returns>
        public string GetRoles()
        {
            return _roles;
        }

        /// <summary>
        /// Sets the roles.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public void SetRoles(string roles)
        {
            _roles = roles;
        }
    }

    /// <summary>
    ///  // Init class UserBussiness
    /// </summary>
    /// <seealso cref="IPM.Business.IUserBusiness" />
    public class UserBusiness :IUserBusiness
    {
        public const string StrDomainName = "fsoft.fpt.vn";
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IUserRoleRepository _userRoleRepository;
        private IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBusiness"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userRoleRepository">The user role repository.</param>
        /// <param name="roleRepository">The role repository.</param>
        public UserBusiness(IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }

        // Get infor user
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't load list user: " + ex.Message</exception>
        public List<InfoUser> GetUser()
        {
            // Initializes a new List InfoUser
            List<InfoUser> infoUser = new List<InfoUser>();

            try
            {
                IEnumerable<User> user = _userRepository.Get();
                IEnumerable<UserRole> userRole = _userRoleRepository.Get();
                IEnumerable<Role> role = _roleRepository.Get();

                // Get information User include : Account and Name RoleID
                var userRoles = from ur in userRole
                                from r in role
                                where ur.RoleID == r.ID
                                select new { ur.Account, r.Name };
                // Add information user into List InfoUser
                foreach (var item in user)
                {
                    InfoUser infouser = new InfoUser();
                    List<Roles> listroles = new List<Roles>();

                    infouser.SetAccount(item.Account);
                    infouser.SetFullName(item.Username);
                    infouser.SetActive(item.Active);                  
                    foreach (var item1 in userRoles)
                    {
                        if (item.Account == item1.Account)
                        {
                            Roles role1 = new Roles();

                            role1.SetRoles(item1.Name);
                            listroles.Add(role1);
                        }
                    }
                    infouser.SetRoles(listroles);
                    infoUser.Add(infouser);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't load list user: " + ex.Message);
            }

            return infoUser;
        }

        /// <summary>
        /// Checks the login.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool CheckLogin(string account, string password)
        {
            bool checkEa = CheckExistAccount(account);

            if (checkEa)
            {
                if (CheckActive(account))
                {
                    return IsValidateLogin(account, password);
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks the active.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't not check active account: " + ex.Message</exception>
        public bool CheckActive(string account)
        {
            bool checkActive=false;

            try
            {
                User user = _userRepository.Get(x => x.Account == account).SingleOrDefault();

                if (user != null)
                {
                    checkActive = user.Active;
                }                  
            }
            catch (Exception ex)
            {
                throw new Exception("Can't not check active account: " + ex.Message);     
            }

            return checkActive;
        }

        //check exist Account in database
        /// <summary>
        /// Checks the exist account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't check exist account: " +  ex.Message</exception>
        public bool CheckExistAccount(string account)
        {
            bool checkExistAccount = false;

            try
            {
                var acc = _userRepository.Get(x => x.Account == account).Count();
                if (acc != 0)
                {
                    checkExistAccount = true;
                }
            }
            catch (Exception ex)
            {
               throw new Exception("Can't check exist account: " +  ex.Message);     
            }

            return checkExistAccount;
        }

        // get full name by account
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <param name="strAccount">The string account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public string GetFullName(string strAccount)
        {
            string strFullName = "";
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, StrDomainName);

            try
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, strAccount);

                if (up != null) strFullName = up.DisplayName;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not get full name of account {0}: {1}",strAccount, ex.Message));      
            }

            return strFullName;
        }

        /// <summary>
        /// Determines whether [is validate login] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is validate login] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.Exception">Server busy</exception>
        private static bool IsValidateLogin(string userName, string password)
        {
            bool flagLogin=false;
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, StrDomainName);

            try
            {
                flagLogin = pc.ValidateCredentials(userName, password, ContextOptions.Negotiate);
            }
            catch (Exception ex)
            {
                throw new Exception("Server busy: "+ ex.Message);
            }

            return flagLogin;
        }

        /// <summary>
        /// Inserts the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't insert account: " +  user.Account + ": " + ex.Message</exception>
        public bool Insert(User user, List<int> role)
        {
            bool checkInsert=false;

            try
            {
                var newUser = new User
                {
                    Account = user.Account,
                    Username = GetFullName(user.Account),
                    Active = true
                };

                if (newUser.Account != null)
                {
                    _userRepository.Add(newUser);
                    if (role.Count() != 0)
                    {
                        foreach (var roleId in role)
                        {
                            var newUserRole = new UserRole
                            {
                                RoleID = roleId,
                                Account = user.Account
                            };

                            _userRoleRepository.Add(newUserRole);
                        }
                    }
                    _unitOfWork.Commit();
                    checkInsert = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't insert account: " +  user.Account + ": " + ex.Message);      
            }

            return checkInsert;
        }

        /// <summary>
        /// Checks the exist account FPT.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public bool CheckExistAccountFpt(string account)
        {
            {
                using (var domainContext = new PrincipalContext(ContextType.Domain, StrDomainName))
                {
                    using (var foundUser =
                        UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, account))
                    {
                        return foundUser != null;
                    }
                }
            }     
        }

        /// <summary>
        /// Checks the account FPT.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public bool CheckAccountFpt(string account)
        {
            return CheckExistAccountFpt(account);
        }

        /// <summary>
        /// Gets the user by account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't get user by account: " + account + ": " + ex.Message</exception>
        public InfoUser GetUserByAccount(string account)
        {
            //  Initializes a new InfoUser
            InfoUser infoUser = new InfoUser();

            try
            {
                //get Account, list Role by string account
                User user = _userRepository.Get(x => x.Account == account).SingleOrDefault();
                IEnumerable<UserRole> userRole = _userRoleRepository.Get(x => x.Account == account);
                IEnumerable<Role> role = _roleRepository.Get();

                if (user != null)
                {
                    List<Roles> roles = new List<Roles>();
                
                    infoUser.SetAccount(user.Account);
                    infoUser.SetFullName(user.Username);
                    infoUser.SetActive(user.Active);
                    var userRoles = from ur in userRole
                        from r in role
                        where ur.RoleID == r.ID && ur.Account == user.Account
                        select new { r.Name };
                    foreach (var item in userRoles)
                    {
                        Roles roleOfUser = new Roles();

                        roleOfUser.SetRoles(item.Name);
                        roles.Add(roleOfUser);
                    }
                    infoUser.SetRoles(roles);
                }
            }
            catch (Exception ex)
            {
               throw new Exception("Can't get user by account: " + account + ": " + ex.Message);
            }

            return infoUser;
        }

        /// <summary>
        /// Edits the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't not Edit account: " + user.Account + ": " + ex.Message</exception>
        public bool Edit(User user, List<int> role)
        {
            bool checkEditUser=false;

            try
            {
                // get user
                User oldUser = _userRepository.Get(x => x.Account == user.Account).SingleOrDefault();
                if (oldUser != null)
                {
                    oldUser.Active = user.Active;
                    oldUser.Username = GetFullName(user.Account);
                    _userRoleRepository.DeleteMulti(x => x.Account == user.Account);
                    if (role != null && role.Count != 0)
                    {
                        foreach (var item in role)
                        {
                            UserRole userRoles = new UserRole
                            {
                                Account = user.Account,
                                RoleID = item
                            };

                            _userRoleRepository.Add(userRoles);
                        }
                    }
                    _unitOfWork.Commit();
                    checkEditUser = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't not Edit account: " + user.Account + ": " + ex.Message);
            }

            return checkEditUser;
        }

        /// <summary>
        /// Gets the list user by account.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't not get list user: " + ex.Message</exception>
        public List<InfoUser> GetListUserByAccount(string searchString)
        {
            //  Initializes a new InfoUser
            List<InfoUser> listInfoUser = new List<InfoUser>();

            try
            {
                IEnumerable<User> user;
                IEnumerable<UserRole> userRole;
                IEnumerable<Role> role;

                if (string.IsNullOrEmpty(searchString))
                {
                    user           =  _userRepository.Get();
                    userRole       =  _userRoleRepository.Get();
                    role           =  _roleRepository.Get();
                }
                else
                {
                    userRole       = _userRoleRepository.Get();
                    role           = _roleRepository.Get();
                    var userSearch = _userRepository.Get(x=>x.Account.Contains(searchString) || x.Username.Contains(searchString));    
                    if (userSearch.Count() != 0)
                    {
                        user           = userSearch;
                    }
                    else
                    {
                        var roleSearch = _roleRepository.Get(x => x.Name.Contains(searchString));
                        user           = (from u in _userRepository.Get()
                                         join ur in userRole
                                         on u.Account equals ur.Account
                                         join r in roleSearch on ur.RoleID equals r.ID
                                         select u).Distinct();
                        
                    }                              
                }
                var userRoles      = from ur in userRole
                                     from r in role
                                     where ur.RoleID == r.ID
                                     select new { ur.Account, r.Name };
                foreach (var itemUser in user)
                {
                    // InfoUser is class InfoUser in UserBusiness
                    InfoUser infouser = new InfoUser();
                    //ListRole is list Role in UserBusiness not in Model
                    List<Roles> listRoles = new List<Roles>();

                    infouser.SetAccount(itemUser.Account);
                    infouser.SetFullName(itemUser.Username);
                    infouser.SetActive(itemUser.Active);        
                    foreach (var itemRole in userRoles)
                    {
                        if (itemUser.Account == itemRole.Account)
                        {
                            // Roles is class role in UserBusiness not in Model
                            Roles role1 = new Roles();

                            role1.SetRoles(itemRole.Name);
                            listRoles.Add(role1);
                        }
                    }
                    infouser.SetRoles(listRoles);
                    listInfoUser.Add(infouser);
                } 
            }
            catch (Exception ex)
            {
                throw new Exception("Can't not get list user: " + ex.Message);
            }

            return listInfoUser;
        }

        /// <summary>
        /// Gets the i dby account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not get ID of Account: " + ex.Message</exception>
        public int GetIDbyAccount(string account)
        {
            int idAcount = -1 ;

            try
            {
                User user = _userRepository.Get(x => x.Account == account).SingleOrDefault();
                if (user != null) idAcount = user.ID;
            }
            catch (Exception ex)
            {
                throw new Exception("Not get ID of Account: " + ex.Message);
            }

            return idAcount;
        }   
    }
}
