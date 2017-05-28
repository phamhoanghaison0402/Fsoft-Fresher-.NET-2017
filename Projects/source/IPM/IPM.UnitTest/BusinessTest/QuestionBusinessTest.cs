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
    /// Summary description for GuidelineBusinessTest
    /// </summary>
    [TestClass]
    public class GuidelineBusinessTest
    {
        private Mock<IGuidelineRepository> _mockGuidelineRepository;
        private Mock<ICatalogRepository> _mockCatalogRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IGuidelineBusiness _guidelineBusiness;

        private IEnumerable<Guideline> _listGuideline;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockGuidelineRepository = new Mock<IGuidelineRepository>();
            _mockCatalogRepository = new Mock<ICatalogRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _guidelineBusiness = new GuidelineBusiness(_mockGuidelineRepository.Object, _mockUnitOfWork.Object);

            _listGuideline = new List<Guideline>()
            {
                new Guideline() {ID = 1, Name = "guideline12",Active = true},
            };

        }

        /// <summary>
        /// Test for function GetAll
        /// </summary>
        [TestMethod]
        public void Guideline_Business_GetAll()
        {
            //setup method
            _mockGuidelineRepository.Setup(m => m.GetAll(null)).Returns(_listGuideline);

            //call action
            var result = _guidelineBusiness.GetAll() as List<Guideline>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
        /// <summary>
        /// Test for function add
        /// </summary>
        [TestMethod]
        public void Guideline_Business_Add()
        {
            Guideline guideline = new Guideline();
            guideline.ID = 1;
            guideline.Name = "ABC";
            guideline.Active = true;

            //setup method
            _mockGuidelineRepository.Setup(m => m.Add(guideline)).Returns((Guideline q) =>
            {
                q.ID = 1;
                return q;
            });

            //call action
            var result = _guidelineBusiness.Insert(guideline);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

    }
}
