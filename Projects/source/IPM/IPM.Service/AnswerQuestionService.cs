using IPM.Business;
using IPM.Model.Models;
using System.Collections.Generic;

namespace IPM.Service
{
    /// <summary>
    /// interface of AnswerQuestionService
    /// </summary>
    public interface IAnswerQuestionService : IServiceBase<AnswerQuestion>
    {
    }

    /// <summary>
    /// class AnswerQuestionService implement ServiceBase, IAnswerQuestionService
    /// </summary>
    public class AnswerQuestionService : ServiceBase<AnswerQuestion>, IAnswerQuestionService
    {
        private IAnswerQuestionBusiness _answerQuestionBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="answerQuestionBusiness"></param>
        public AnswerQuestionService(IAnswerQuestionBusiness answerQuestionBusiness)
            : base(answerQuestionBusiness)
        {
            this._answerQuestionBusiness = answerQuestionBusiness;
        }
    }
}