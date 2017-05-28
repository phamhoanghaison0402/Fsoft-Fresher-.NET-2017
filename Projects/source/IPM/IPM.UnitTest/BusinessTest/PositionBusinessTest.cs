using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IPM.Data.Repositories;
using IPM.Data.Infrastructure;
using IPM.Business;
using IPM.Model.Models;
using Moq;
using System.Collections.Generic;

namespace IPM.UnitTest.BusinessTest
{
    [TestClass]
    public class PositionBusinessTest
    {
        private Mock<IPositionRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IPositionBusiness _positionBusiness;
        private List<Position> _listposition;


        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IPositionRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _positionBusiness = new PositionBusiness(_mockRepository.Object, _mockUnitOfWork.Object);
            _listposition = new List<Position>()
            {
                new Position() {ID = 1 , Code="Po1", Name="Position1", Active = true},
                new Position() {ID = 2 , Code="Po2", Name="Position2", Active = true},
                new Position() {ID = 3 , Code="Po3", Name="Position3", Active = true}
            };
        }

        /// <summary>
        /// get all list position
        /// </summary>
        [TestMethod]
        public void GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.Get(x => x.Active == true, null, "")).Returns(_listposition);

            //call action
            var result = _positionBusiness.GetAll() as List<Position>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        /// <summary>
        /// function insert position
        /// </summary>
        [TestMethod]
        public void Insert()
        {
            //setup method
            Position position = new Position();
            position.ID = 8;
            _mockRepository.Setup(m => m.Add(position)).Returns(position);

            //call action
            var result = _positionBusiness.Insert(position);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.ID);
        }

        /// <summary>
        /// update position information
        /// </summary>
        [TestMethod]
        public void Update()
        {
            //setup method
            Position position = new Position();
            position.ID = 8;
            position.Code = "ts01";
            position.Active = false;
            position.Name = "test";

            _mockRepository.Setup(m => m.Update(position));

            //call action
            _positionBusiness.Update(position);

            //compare
            Assert.AreEqual(true, true);
        }
        
        /// <summary>
        /// delete position function
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            //setup method
            Position position = new Position();
            position.ID = 8;
            position.Code = "ts01";
            position.Active = false;
            position.Name = "test";

            _mockRepository.Setup(m => m.GetSingleById(8)).Returns(position);
            _mockRepository.Setup(m => m.Update(position));

            //call action
            var result = _positionBusiness.Delete(8);

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.ID);
        }

        /// <summary>
        /// check code is existed
        /// </summary>
        [TestMethod]
        public void CheckCode()
        {
            //setup method
            string code = "FR001";
            _mockRepository.Setup(m => m.CheckCode(code)).Returns(true);

            //call action
            var result = _positionBusiness.CheckCode(code);//true

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// check name is existed
        /// </summary>
        [TestMethod]
        public void CheckName()
        {
            //setup method
            string name = "Fresher .Net";
            _mockRepository.Setup(m => m.CheckName(name)).Returns(false);

            //call action
            var result = _positionBusiness.CheckName(name);//true

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ValidateCode()
        {
            //setup method
            string code = "fresher .net";

            //call action
            var result = _positionBusiness.ValidateCode(code);//false

            //compare
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ValidateName()
        {
            //setup method
            string name = "Skill";

            //call action
            var result = _positionBusiness.ValidateName(name);//true

            //compare
            Assert.AreEqual(true, result);
        }
    }
}
