using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPM.UnitTest.BusinessTest
{
    /// <summary>
    /// Interview process business test
    /// </summary>
    [TestClass]
    public class InterviewProcessBusinessTest
    {
        private Mock<IInterviewProcessRepository> _mockInterviewProcessReposition;
        private Mock<IRoundProcessRepository> _mockRoundProcessReposition;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IInterviewProcessBusiness objBusiness;
        private List<InterviewProcess> listInterviewProcess;
        private List<RoundProcess> listRoundProcess;
        const int intBadId = 0;

        [TestInitialize]
        public void Initialize()
        {
            _mockInterviewProcessReposition = new Mock<IInterviewProcessRepository>();
            _mockRoundProcessReposition = new Mock<IRoundProcessRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            objBusiness = new InterviewProcessBusiness(_mockInterviewProcessReposition.Object
                , _mockRoundProcessReposition.Object, _mockUnitOfWork.Object);

            listInterviewProcess = new List<InterviewProcess>()
            {
                new InterviewProcess {ID = 1, Active=true, PositionID = 1, ProcessName = "process 1", StartDate = DateTime.Now },
                new InterviewProcess {ID = 2, Active=true, PositionID = 2, ProcessName = "process 2", StartDate = DateTime.Now },
                new InterviewProcess {ID = 3, Active=true, PositionID = 3, ProcessName = "process 3", StartDate = DateTime.Now }
            };

            listRoundProcess = new List<RoundProcess>()
            {
                new RoundProcess {ID = 1, InterviewProcessID = 1, InterviewRoundID = 1 },
                new RoundProcess {ID = 1, InterviewProcessID = 1, InterviewRoundID = 1 },
                new RoundProcess {ID = 1, InterviewProcessID = 1, InterviewRoundID = 1 }
            };
        }

        [TestMethod]
        public void InterviewProcessBusiness_GetAll_Success()
        {
            //setup method
            _mockInterviewProcessReposition.Setup(m => m.Get(null, null, It.IsAny<string>())).Returns(listInterviewProcess);

            //call action
            var result = objBusiness.GetAll() as List<InterviewProcess>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.ToList().Count);
        }

        [TestMethod]
        public void InterviewProcessBusiness_AddProcess_Success()
        {
            //setup method
            _mockInterviewProcessReposition.Setup(m => m.Add(It.IsAny<InterviewProcess>())).Returns(listInterviewProcess.First());
            _mockRoundProcessReposition.Setup(m => m.Add(It.IsAny<RoundProcess>())).Returns(listRoundProcess.First());
            //call action
            var result = objBusiness.AddProcess(listInterviewProcess.First(), listRoundProcess) as int?;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(listInterviewProcess.First().ID, result);
        }

        [TestMethod]
        public void InterviewProcessBusiness_AddProcess_AddProcessError()
        {
            //setup method
            _mockInterviewProcessReposition.Setup(m => m.Add(It.IsAny<InterviewProcess>())).Returns((InterviewProcess)null);
            //call action
            var result = objBusiness.AddProcess(listInterviewProcess.First(), listRoundProcess) as int?;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(intBadId, result);
        }

        [TestMethod]
        public void InterviewProcessBusiness_AddProcess_AddRoundProcessError()
        {
            //setup method
            _mockInterviewProcessReposition.Setup(m => m.Add(It.IsAny<InterviewProcess>())).Returns(listInterviewProcess.First());
            _mockRoundProcessReposition.Setup(m => m.Add(It.IsAny<RoundProcess>())).Returns((RoundProcess)null);
            //call action
            var result = objBusiness.AddProcess(listInterviewProcess.First(), listRoundProcess) as int?;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(intBadId, result);
        }

        [TestMethod]
        public void InterviewProcessBusiness_GetProcessById_Success()
        {
            //setup method
            _mockInterviewProcessReposition.Setup(m => m.Add(It.IsAny<InterviewProcess>())).Returns(listInterviewProcess.First());
            _mockRoundProcessReposition.Setup(m => m.Add(It.IsAny<RoundProcess>())).Returns((RoundProcess)null);
            //call action
            var result = objBusiness.AddProcess(listInterviewProcess.First(), listRoundProcess) as int?;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(intBadId, result);
        }
    }
}
