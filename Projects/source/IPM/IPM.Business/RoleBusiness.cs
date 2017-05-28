using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Business
{
    public interface IRoleBusiness
    {
        IEnumerable<Role> getRoles();
    }
   public class RoleBusiness : IRoleBusiness
    {
        private IRoleRepository _roleRepository;
        private IUnitOfWork _unitOfWork;
        public RoleBusiness(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this._roleRepository = roleRepository; 
            this._unitOfWork = unitOfWork;
            
        }
        public IEnumerable<Role> getRoles()
        {
            return _roleRepository.Get();
        }
    }
}
