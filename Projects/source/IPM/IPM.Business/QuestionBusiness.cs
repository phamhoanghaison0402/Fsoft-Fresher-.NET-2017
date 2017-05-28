using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using log4net;
using System;
using System.Collections.Generic;

namespace IPM.Business
{
    /// <summary>
    /// Interface for QuestionBusiness
    /// </summary>
    public interface IQuestionBusiness : IBusinessBase<Question>
    {
        /// <summary>
        /// Get Skill name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string SkillName(int id);
        /// <summary>
        /// Get All Question by Catalog ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetAll(int id);
    }

    /// <summary>
    /// Declare Quetsion business
    /// </summary>
    public class QuestionBusiness : BusinessBase<Question>, IQuestionBusiness
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISkillRepository _skillRepository;
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// Init Question business
        /// </summary>
        /// <param name="questionRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="skillRepository"></param>
        public QuestionBusiness(IQuestionRepository questionRepository, IUnitOfWork unitOfWork, ISkillRepository skillRepository) : base(questionRepository, unitOfWork)
        {
            this._questionRepository = questionRepository;
            this._skillRepository = skillRepository;
            this._unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get question by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string SkillName(int id)
        {
            var skillName = _skillRepository.GetSingleById(id);
            if (skillName == null)
            {
                throw new Exception("Skill not found");
            }
            else
            {
                return skillName.Name;
            }
        }

        /// <summary>
        /// Get all Question of this Catalog via Catalog ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            return _questionRepository.GetAll(id);
        }
    }
}