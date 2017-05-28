using System;
using System.Collections.Generic;
using System.Linq;
using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.ModelReport;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IPM.UnitTest.BusinessTest
{
    [TestClass]
    public class ReportBusinessTest
    {
        private Mock<IReportRepositoty> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IReportBusiness _reportBusiness;
        private List<ReportSkillAndPosition> _reportSkillAndPositions;
        private List<ReportCandidates> _reportCandidates;
        private List<ReportCandidates> _reportCandidatesGst;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IReportRepositoty>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _reportBusiness = new ReportBusiness(_mockRepository.Object);
            _reportSkillAndPositions = new List<ReportSkillAndPosition>()
            {
                new ReportSkillAndPosition()
                {
                    StartDate = DateTime.Now,
                    Name = "ThuongTT2",
                    Position = ".NET",
                    PositionID = 1,
                    Skills = "Java, C#"
                }
            };
            _reportCandidates = new List<ReportCandidates>()
            {
                new ReportCandidates()
                {
                    ID = 1,
                    Result = 1
                }
            };

            _reportCandidatesGst = new List<ReportCandidates>()
            {
                new ReportCandidates()
                {
                    ID = 1,
                    Result = 1,
                    Certificate = ""
                }
            };
        }

        [TestMethod]
        public void ReportBusiness_GetReportSkillAndPosition()
        {
            //setup method
            _mockRepository.Setup(
                m => m.GetReportSkillAndPosition(DateTime.Now.ToLongDateString(), DateTime.Now.AddDays(2).ToLongDateString(),
                    "ThuongTT2", ".NET")).Returns(_reportSkillAndPositions);

            //call action
            var result = _reportBusiness.GetReportSkillAndPosition(DateTime.Now.ToLongDateString(),
                DateTime.Now.AddDays(2).ToLongDateString(),
                "ThuongTT2", ".NET");

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ReportBusiness_GetReportCandidates()
        {
            //setup method
            _mockRepository.Setup(
                m => m.GetReportCandidates(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), "1")).Returns(_reportCandidates);

            //call action
            var result = _reportBusiness.GetReportCandidates(DateTime.Now.ToLongDateString(), DateTime.Now.AddDays(2).ToLongDateString(),
                    "ThuongTT2", "1").ToList();

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ReportBusiness_GetReportCandidatesGst()
        {
            //setup method
            _mockRepository.Setup(
                m => m.GetReportCandidatesGst(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), "1")).Returns(_reportCandidates);

            //call action
            var result = _reportBusiness.GetReportCandidatesGst("2017-05-08",null,null,"1").ToList();

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
