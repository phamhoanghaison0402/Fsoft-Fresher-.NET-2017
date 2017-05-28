using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.Data.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        /// <summary>
        /// check code is existed
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckCode(string code);

        /// <summary>
        /// check name is existed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckName(string name);
    }

    /// <summary>
    /// class inheritance interface and repository
    /// </summary>
    public class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        public PositionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        
        /// <summary>
        /// function check code is existed
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckCode(string code)
        {
            var query = from b in DbContext.Positions
                        where b.Active == true
                        && b.Code == code
                        select b;

            return query.Count() > 0 ? true : false;
        }

        /// <summary>
        /// function check name is existed
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            var query = from b in DbContext.Positions
                        where b.Active == true
                        && b.Name == name
                        select b;

            return query.Count() > 0 ? true : false;
        }
    }
}
