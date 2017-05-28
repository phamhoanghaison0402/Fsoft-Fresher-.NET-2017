using IPM.Business;
using IPM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;

namespace IPM.Web.Models
{
    public class UserRoleProvider : RoleProvider
    {
        IUserService _userService;
        IRoleService _roleService;

        public UserRoleProvider()
        {
            _userService=new UserService(new UserBusiness(new UserRepository(new DbFactory()),new UnitOfWork(new DbFactory()),new UserRoleRepository(new DbFactory()),new RoleRepository(new DbFactory())));
        }
        //public UserRoleProvider(IUserService userService, IRoleService roleService)
        //{
        //    _userService = userService;
        //    _roleService = roleService;
        //}
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var userAccount = _userService.GetUserByAccount(username);
            if (userAccount == null)
            {
                return null;
            }
            else
            {   
                var listRole = userAccount.GetRoles();             
                List<string> list=new List<string>();
                //int i = 0;
                foreach (var ls in listRole)
                {
                    list.Add(ls.ToString());
                }
                return list.ToArray();
            }
        }
            
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}