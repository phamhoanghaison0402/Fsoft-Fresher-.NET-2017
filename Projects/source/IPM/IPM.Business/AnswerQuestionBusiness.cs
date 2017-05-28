using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPM.Business
{
    /// <summary>
    /// Interface of AnswerQuestionBusiness
    /// </summary>
    public interface IAnswerQuestionBusiness : IBusinessBase<AnswerQuestion>
    {
    }

    /// <summary>
    /// Class AnswerQuestionBusiness implement BusinessBass and IAswerQuestionBusiness
    /// </summary>
    public class AnswerQuestionBusiness : BusinessBase<AnswerQuestion>, IAnswerQuestionBusiness
    {
        private readonly IAnswerQuestionRepository _answerQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Interface of AnswerQuestionBusiness
        /// </summary>
        /// <param name="answerQuestionRepository"></param>
        /// <param name="unitOfWork"></param>
        public AnswerQuestionBusiness(IAnswerQuestionRepository answerQuestionRepository, IUnitOfWork unitOfWork) :base(answerQuestionRepository, unitOfWork)
        {
            this._answerQuestionRepository = answerQuestionRepository;
            this._unitOfWork = unitOfWork;
        }

    }
}