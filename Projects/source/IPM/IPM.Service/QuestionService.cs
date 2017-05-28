using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;

namespace IPM.Service
{
    /// <summary>
    /// Interface of QuestionService
    /// </summary>
    public interface IQuestionService : IServiceBase<Question>
    {
        /// <summary>
        /// Get skill name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string SkillName(int id);
        /// <summary>
        /// Get all question by Catalog ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetAll(int id);
    }

    /// <summary>
    /// Declare Question Service
    /// </summary>
    public class QuestionService : ServiceBase<Question>, IQuestionService
    {
        private readonly IQuestionBusiness _questionBusiness;

        /// <summary>
        /// Init Quetsion service
        /// </summary>
        /// <param name="questionBusiness"></param>
        public QuestionService(IQuestionBusiness questionBusiness) : base(questionBusiness)
        {
            this._questionBusiness = questionBusiness;
        }

        /// <summary>
        /// Get skill by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string SkillName(int id)
        {
            return _questionBusiness.SkillName(id);
        }    

        /// <summary>
        /// Get all by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetAll(int id)
        {
            return _questionBusiness.GetAll(id);
        }
    }
}