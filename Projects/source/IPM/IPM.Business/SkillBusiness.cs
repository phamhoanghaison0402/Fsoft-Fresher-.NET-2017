using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IPM.Business
{
    public interface ISkillBusiness: IBusinessBase<Skill>
    {
        /// <summary>
        /// check: Name is exist
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
    /// Class skill business
    /// </summary>
    public class SkillBusiness : BusinessBase<Skill>, ISkillBusiness
    {
        private ISkillRepository _skillRepository;
        private IUnitOfWork _unitOfWork;
        private readonly ILog _log = LogManager.GetLogger("SkillBusiness.cs");
        public SkillBusiness(ISkillRepository skillRepository, IUnitOfWork unitOfWork) : base(skillRepository, unitOfWork)
        {
            this._skillRepository = skillRepository;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get list of skill
        /// </summary>
        /// <returns></returns>
        override public IEnumerable<Skill> GetAll()
        {
            _log.Info("Get list of skill");

            return _skillRepository.Get(x => x.Active == true);
        }

        /// <summary>
        /// delete a skill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        override public Skill Delete(int id)
        {
            var skillDb = _skillRepository.GetSingleById(id);

            skillDb.Active = false;
            _skillRepository.Update(skillDb);
            _log.Info("delete a skill");

            return skillDb;
        }

        /// <summary>
        /// Check: Name is exist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            _log.Info("Check: Name is exist");

            return _skillRepository.CheckName(name);
        }

        /// <summary>
        /// Search skill by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Skill> Search(string name)
        {
            return _skillRepository.Search(name);
        }

        /// <summary>
        ///  Validate Name of skill
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateName(string name)
        {
            Regex rgx = new Regex(@"^([A-Za-zAÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶEÉÈẺẼẸÊẾỀỂỄỆIÍÌỈĨỊOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢUÚÙỦŨỤƯỨỪỬỮỰYÝỲỶỸỴĐaáàảãạâấầẩẫậăắằẳẵặeéèẻẽẹêếềểễệiíìỉĩịoóòỏõọôốồổỗộơớờởỡợuúùủũụưứừửữựyýỳỷỹỵđ]{1})+([A-Za-z0-9 AÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶEÉÈẺẼẸÊẾỀỂỄỆIÍÌỈĨỊOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢUÚÙỦŨỤƯỨỪỬỮỰYÝỲỶỸỴĐaáàảãạâấầẩẫậăắằẳẵặeéèẻẽẹêếềểễệiíìỉĩịoóòỏõọôốồổỗộơớờởỡợuúùủũụưứừửữựyýỳỷỹỵđ#+]){0,79}$");
            return rgx.IsMatch(name);
        }
    }
}