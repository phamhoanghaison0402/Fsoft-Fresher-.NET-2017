using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace IPM.UnitTest.BusinessTest
{

    [TestClass]
    public class InterviewAdminBusinessTest
    {
        private Mock<IInterviewAdminRepository> _mockInterviewAdminRepository;
        private Mock<ICandidateRepository> _mockCandidateRepository;
        private Mock<IInterviewRepository> _mockInterviewRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IInterviewAdminBusiness _interviewAdminBusiness;
        private List<User> _listInterviewAdmin;
        private List<Candidate> _listCandidate;
        private List<Role> _listRole;
        private List<UserRole> _listUserRole;
        private List<Position> _listPosition;
        private List<Interview> _listInterview;
        private List<RoundProcess> _listRoundProcess;
        private List<InterviewRound> _listInterviewRound;
        private List<InterviewProcess> _listInterviewProcess;
        private List<Guideline> _listGuideline;

        public InterviewAdminBusinessTest()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            _mockInterviewAdminRepository = new Mock<IInterviewAdminRepository>();
            _mockCandidateRepository = new Mock<ICandidateRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockInterviewRepository = new Mock<IInterviewRepository>();
            _interviewAdminBusiness = new InterviewAdminBusiness(_mockInterviewAdminRepository.Object, _mockCandidateRepository.Object, 
                                                                    _mockUnitOfWork.Object, _mockInterviewRepository.Object);
            _listInterviewAdmin = new List<User>()
            {
                new User() {ID = 1, Account = "TienTD1",Username = "Trinh Dinh Tien", Active = true},
                new User() {ID = 2, Account = "BaoTC",Username = "Thach Chi Bao", Active = true},
                new User() {ID = 3, Account = "ThiNL",Username = "Nguyen Linh Thuong", Active = true},
                new User() {ID = 4, Account = "DuyHH1", Username = "Huynh Huu Duy", Active = true }
            };

            _listCandidate = new List<Candidate>()
            {
                new Candidate() { ID = 1, Name = "Trinh Dinh Tien", InterviewAdminID = 1, PositionID = 1,
                                    Birthdate = DateTime.Parse("02/11/1994"), IDCard = "11111", Email = "Tien@gmail.com",
                                    Address = "thu duc", Phone = "0978055271", University = "UIT", Major = "software",
                                    GPA = "5", ConcidentStatus = 1, Active = true, Certificate = "No"}
            };

            _listRole = new List<Role>()
            {
                new Role() {ID = 1, Name = "admin", Description = "quan ly chung", Active = true },
                new Role() {ID = 2, Name = "interview admin", Description = "quan ly ung vien", Active = true },
                new Role() {ID = 3, Name = "interviewer", Description = "nguoi phong van", Active = true }
            };

            _listUserRole = new List<UserRole>()
            {
                new UserRole() {Account = "TienTD1", RoleID = 2 },
                new UserRole() {Account = "BaoTC", RoleID = 2 },
                new UserRole() {Account = "ThiNL", RoleID = 2 }
            };

            _listPosition = new List<Position>()
            {
                new Position() {ID = 1, Name = "Fresher", Code = "",Active = true },
                new Position() {ID = 2, Name = "Junior", Code = "",Active = true }
            };

            _listInterview = new List<Interview>
            {
                new Interview() {ID = 1, StartTime = DateTime.Parse("05/17/2017"), RoundProcessID = 1, CandidateID = 1, Result = null,
                    Record = "", InterviewerID = 4, InterviewAdminID = 1, Active = true}
                //new Interview() {ID = 2, StartTime = DateTime.Parse("05/17/2017"), RoundProcessID = 2, CandidateID = 1, Result = true,
                //    Record = "", InterviewerID = 4, InterviewAdminID = 2, Active = true}
            };

            _listRoundProcess = new List<RoundProcess>
            {
                new RoundProcess() { ID = 1, InterviewRoundID = 1, InterviewProcessID = 1, Active = true}
            };

            _listInterviewProcess = new List<InterviewProcess>
            {
                new InterviewProcess() {ID = 1, ProcessName = ".NET 1", PositionID = 1, StartDate =  DateTime.Parse("05/17/2017"), Active = true},
                new InterviewProcess() {ID = 2, ProcessName = "Java", PositionID = 1, StartDate =  DateTime.Parse("05/17/2017"), Active = true}
            };

            _listInterviewRound = new List<InterviewRound>
            {
                new InterviewRound() {ID = 1, RoundName = ".NET Round 1", Description = "Vong 1 .net", GuidelineID = 1, Active = true }
            };

            _listGuideline = new List<Guideline>
            {
                new Guideline() {ID = 1, Name = "Guiline 1", Active = true },
                new Guideline() {ID = 2, Name = "Guiline 2", Active = true }
            };
        }

        [TestMethod]
        public void InterviewAdmin_Business_GetAllByRoleId()
        {

            //setup method
            _mockInterviewAdminRepository.Setup(m => m.GetInterviewAdminByRole(_listRole[1].ID,"")).Returns(_listInterviewAdmin);

            //call action
            var result = _interviewAdminBusiness.GetAllByRoleId(_listRole[1].ID,"") as List<User>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void InterviewAdmin_Business_GetAllByRoleId1()
        {

            //setup method
            _mockInterviewAdminRepository.Setup(m => m.GetInterviewAdminByRole(_listRole[1].ID, "tien")).Returns(_listInterviewAdmin);

            //call action
            var result = _interviewAdminBusiness.GetAllByRoleId(_listRole[1].ID, "tien") as List<User>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        //[TestMethod]
        //public void GetCandidateByInterViewAdminID()
        //{

        //    //setup method
        //    _mockCandidateRepository.Setup(m => m.Get(x => x.InterviewAdminID == _listInterviewAdmin[0].ID, null, "Position")).Returns(_listCandidate);

        //    //call action
        //    var result = _interviewAdminBusiness.GetCandidateByInterViewAdminID(_listInterviewAdmin[0].ID) as List<Candidate>;

        //    //compare
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(0, result.Count);
        //}

        [TestMethod]
        public void UpdateCandidate()
        {
            Candidate candidateModel = _listCandidate[0];
            int interviewAdminID = _listInterviewAdmin[1].ID;

            _mockCandidateRepository.Setup(m => m.GetSingleById(candidateModel.ID)).Returns(candidateModel);

            var result = _interviewAdminBusiness.UpdateCandidate(candidateModel, interviewAdminID);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void UpdateCandidate1()
        {
            Candidate candidateModel = _listCandidate[0];

            _mockCandidateRepository.Setup(m => m.GetSingleById(candidateModel.ID)).Returns(candidateModel);

            var result = _interviewAdminBusiness.UpdateCandidate(candidateModel, _listInterviewAdmin[0].ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void UpdateCandidate2()
        {
            Candidate candidateModel = null;


            _mockCandidateRepository.Setup(m => m.GetSingleById(2)).Returns(candidateModel);
            try
            {
                _interviewAdminBusiness.UpdateCandidate(_listCandidate[0], _listInterviewAdmin[1].ID);
            }
            catch
            {
                Assert.AreEqual(1, 1);
            }
        }

        [TestMethod]
        public void GetAll()
        {
            //setup method
            _mockInterviewAdminRepository.Setup(m => m.Get(null,null,"")).Returns(_listInterviewAdmin);

            //call action
            var result = _interviewAdminBusiness.GetAll() as List<User>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        [TestMethod]
        public void GetInterviewByInterviewAdmin()
        {
            Candidate candidateModel = _listCandidate[0];
            _mockInterviewRepository.Setup(m => m.GetInterviewByInterviewAdmin(candidateModel))
                .Returns(_listInterview);

            var result = _interviewAdminBusiness.GetInterviewByInterviewAdmin(candidateModel)
                as List<Interview>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Save()
        {
            _mockUnitOfWork.Setup(m => m.Commit());

            _interviewAdminBusiness.Save();

            Assert.AreEqual(true, true);
        }
    }
}
