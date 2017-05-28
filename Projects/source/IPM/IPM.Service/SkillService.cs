using IPM.Business;
using IPM.Model.Models;
using log4net;
using System.Collections.Generic;
using System;

namespace IPM.Service
{
    public interface ISkillService : IServiceBase<Skill>
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

        /// <summary>
        /// Validate Name of skill
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ValidateName(string name);
    }

    /// <summary>
    /// Class skill service
    /// </summary>
    public class SkillService : ServiceBase<Skill>, ISkillService
    {
        private ISkillBusiness _skillBusiness;
        private readonly ILog _log = LogManager.GetLogger("SkillService.cs");
        public SkillService(ISkillBusiness skillBusiness) : base(skillBusiness)
        {
            this._skillBusiness = skillBusiness;
        }

        /// <summary>
        /// Check: Name is exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            _log.Info("Check: Name is exist ");

            return _skillBusiness.CheckName(name);
        }

        /// <summary>
        /// Search skill by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Skill> Search(string name)
        {
            return _skillBusiness.Search(name);
        }

        /// <summary>
        /// Validate Name of skill
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateName(string name)
        {
            return _skillBusiness.ValidateName(name);
        }
    }
}