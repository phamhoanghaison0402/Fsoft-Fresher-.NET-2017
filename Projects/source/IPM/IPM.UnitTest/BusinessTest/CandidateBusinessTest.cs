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
    /// Summary description for CandidateBusinessTest
    /// </summary>
    [TestClass]
    public class CandidateBusinessTest
    {
        private Mock<ICandidateRepository> _mockCandidateRepository;
        private Mock<ICandidateSkillRepository> _mockCandidateSkillRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ISkillRepository> _mockSkillRepository;
        private ICandidateBusiness _candidateBusiness;
        private List<User> _listUser;
        private List<Position> _listPosition;
        private List<Candidate> _listCandidate;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockCandidateRepository = new Mock<ICandidateRepository>();
            _mockCandidateSkillRepository = new Mock<ICandidateSkillRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSkillRepository = new Mock<ISkillRepository>();
            //_candidateBusiness = new CandidateBusiness(_mockCandidateRepository.Object, _mockCandidateSkillRepository.Object, _mockUnitOfWork.Object, _mockSkillRepository.Object);

            _listUser = new List<User>()
            {
                new User() {ID = 1, Account = "PhuongPD2", Username = "Phan Đức Phương", Active = true },
                new User() {ID = 2, Account = "DuyPA", Username = "Phạm Anh Duy", Active = true },

            };
            _listPosition = new List<Position>()
            {
                new Position() {ID = 1, Name = "NET", Code = "NET", Active = true },
                new Position() {ID = 2, Name = "JAVA", Code = "JAVA", Active = true },

            };
            _listCandidate = new List<Candidate>()
            {
                new Candidate()
                {
                    ID = 1,InterviewAdminID = 1, Name = "Phan Đức Phương",Email = "dphuong510@gmail.com",
                    Birthdate = DateTime.Parse("05/10/1994"), IDCard = "312192417",
                    Address = "Bình Thạnh",Phone = "01653341131", University = "Tôn Đức Thắng",
                    Major = "Khoa học máy tính",GPA = "7.24",Certificate = "NET", ConcidentStatus = 0,
                    PositionID = 1,  Active = true
                },

                new Candidate()
                {
                    ID = 2,InterviewAdminID = 2, Name = "Phạm Anh Duy",Email = "dphuong510@gmail.com",
                    Birthdate = DateTime.Parse("05/10/1994"), IDCard = "312192417",
                    Address = "Bình Thạnh",Phone = "01653341131", University = "Tôn Đức Thắng",
                    Major = "Khoa học máy tính",GPA = "7.24",Certificate = "NET", ConcidentStatus = 0,
                    PositionID = 1,  Active = true
                },

                new Candidate()
                {
                    ID = 3,InterviewAdminID = 2, Name = "Trần Mai Phương",Email = "dphuong510@gmail.com",
                    Birthdate = DateTime.Parse("05/10/1994"), IDCard = "312192417",
                    Address = "Bình Thạnh",Phone = "01653341131", University = "Tôn Đức Thắng",
                    Major = "Khoa học máy tính",GPA = "7.24",Certificate = "NET", ConcidentStatus = 0,
                    PositionID = 1,  Active = true
                },

                new Candidate()
                {
                    ID = 4,InterviewAdminID = 1, Name = "Hoàng Thị Hiền",Email = "dphuong510@gmail.com",
                    Birthdate = DateTime.Parse("05/10/1994"), IDCard = "312192417",
                    Address = "Bình Thạnh",Phone = "01653341131", University = "Tôn Đức Thắng",
                    Major = "Khoa học máy tính",GPA = "7.24",Certificate = "NET", ConcidentStatus = 0,
                    PositionID = 1,  Active = true
                },
            };
        }

        /// <summary>
        /// Test for function GetAll
        /// </summary>
        [TestMethod]
        public void Candidate_Business_GetAll()
        {
            //setup method
            _mockCandidateRepository.Setup(m => m.GetAll(null)).Returns(_listCandidate);

            //call action
            var result = _candidateBusiness.GetAll() as List<Candidate>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Test for function Search
        /// </summary>
        [TestMethod]
        public void Candidate_Business_Search()
        {           
            string searchData = "Phương";
            //setup method
            _mockCandidateRepository.Setup(m => m.Search(searchData,null)).Returns(_listCandidate);

            //call action
            var result = _candidateBusiness.Search(searchData,null);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }

        /// <summary>
        /// Test for function add
        /// </summary>
        [TestMethod]
        public void Candidate_Business_Add()
        {
            Candidate candidate = new Candidate();
            candidate.ID = 1;
            candidate.Name = "Phương";

            //setup method
            _mockCandidateRepository.Setup(m => m.Add(candidate)).Returns((Candidate c) =>
            {
                c.ID = 1;
                return c;
            });

            //call action
            var result = _candidateBusiness.Insert(candidate);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

        /// <summary>
        /// Test for function delete
        /// </summary>
        [TestMethod]
        public void Candidate_Business_DeleteCandidate()
        {
            Candidate candidate = new Candidate();
            candidate.ID = 1;

            //setup method
            _mockCandidateRepository.Setup(m => m.DeleteCandidate(candidate.ID)).Returns(true);

            //call action
            var result = _candidateBusiness.DeleteCandidate(candidate.ID);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);          
        }

        /// <summary>
        /// Test for function update
        /// </summary>
        [TestMethod]
        public void Candidate_Business_Update()
        {
            Candidate candidate = new Candidate();
            candidate.ID = 1;
            candidate.Email = "abc@gmail.com";

            //setup method
            _mockCandidateRepository.Setup(m => m.Update(candidate));

            //call action
            var result = _candidateBusiness.Insert(candidate);

            //compare
            Assert.AreEqual(true, true);
        }

        /// <summary>
        /// Test for function Save
        /// </summary>
        [TestMethod]
        public void Candidate_Business_Save()
        {
            //setup method
            _mockUnitOfWork.Setup(m => m.Commit());

            //call action
            _candidateBusiness.Save();

            //compare
            Assert.AreEqual(true, true);
        }

        /// <summary>
        /// Test for function GetSingleByID
        /// </summary>
        [TestMethod]
        public void Candidate_Business_GetSingleById()
        {
            Candidate candidate = new Candidate();
            candidate.Name = "Phương";
            candidate.ID = 1;

            //setup method
            _mockCandidateRepository.Setup(m => m.GetSingleById(It.IsAny<int>())).Returns(candidate);

            //call action
            var result = _candidateBusiness.GetSingleById(1);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }
    }
}
