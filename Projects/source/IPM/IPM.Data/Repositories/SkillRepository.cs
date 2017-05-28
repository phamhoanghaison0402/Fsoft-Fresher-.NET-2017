using IPM.Data.Infrastructure;
using IPM.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace IPM.Data.Repositories
{
    /// <summary>
    /// ISkillRepository Interface
    /// </summary>
    public interface ISkillRepository : IRepository<Skill>
    {
        /// <summary>
        /// Check: Name is exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckName(string name);

        /// <summary>
        /// Search skill by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<Skill> Search(string name);
    }

    /// <summary>
    /// SkillRepository Class
    /// </summary>
    public class SkillRepository : RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        /// <summary>
        ///  Check: Name is exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            var query = from b in DbContext.Skills
                        where b.Active == true
                        && b.Name == name
                        select b;

            return query.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Search skill by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Skill> Search(string name)
        {
            var query = (from b in DbContext.Skills
                         where b.Name.Contains(name)
                         && b.Active == true
                         select b).OrderBy(x => x.Name);

            return query;
        }
    }
}