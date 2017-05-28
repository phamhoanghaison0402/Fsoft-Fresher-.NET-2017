using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IPM.Data.Repositories;
using Moq;
using IPM.Data.Infrastructure;
using IPM.Business;
using IPM.Model.Models;

namespace IPM.UnitTest.BusinessTest
{
    /// <summary>
    /// Summary description for QuestionBusinessTest
    /// </summary>
    [TestClass]
    public class QuestionBusinessTest
    {
        private Mock<IQuestionRepository> _mockQuestionRepository;
        private Mock<ISkillRepository> _mockSkillRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IQuestionBusiness _questionBusiness;

        private List<Question> _listQuestion;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _mockSkillRepository = new Mock<ISkillRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _questionBusiness = new QuestionBusiness(_mockQuestionRepository.Object, _mockUnitOfWork.Object,
                _mockSkillRepository.Object);

            _listQuestion = new List<Question>()
            {
                new Question() {ID = 1, SkillID = 1, Content = "aaa", Answer = "123", Active = true},
            };

        }

        /// <summary>
        /// Test for function GetAll
        /// </summary>
        [TestMethod]
        public void Question_Business_GetAll()
        {
            //setup method
            _mockQuestionRepository.Setup(m => m.GetAll(null)).Returns(_listQuestion);

            //call action
            var result = _questionBusiness.GetAll() as List<Question>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        /// <summary>
        /// Test for function add
        /// </summary>
        [TestMethod]
        public void Question_Business_Add()
        {
            Question question = new Question();
            question.ID = 1;
            question.SkillID =2;
            question.Content = "ABC";
            question.Answer = "ADKJBF";
            question.Active = true;

            //setup method
            _mockQuestionRepository.Setup(m => m.Add(question)).Returns((Question q) =>
            {
                q.ID = 1;
                return q;
            });

            //call action
            var result = _questionBusiness.Insert(question);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

    }
}
