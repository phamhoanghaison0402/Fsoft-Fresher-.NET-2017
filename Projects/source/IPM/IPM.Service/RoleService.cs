using IPM.Business;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> getRole();
    }
    class RoleService : IRoleService
    {
        private IRoleBusiness _roleBusiness;

        public RoleService(IRoleBusiness roleBusiness)
        {
            this._roleBusiness = roleBusiness;
        }
        public IEnumerable<Role> getRole()
        {
            return _roleBusiness.getRoles();
        }
    }
}
