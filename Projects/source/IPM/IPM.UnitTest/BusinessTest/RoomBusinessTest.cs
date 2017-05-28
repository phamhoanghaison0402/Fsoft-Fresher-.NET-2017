using System;
using System.Collections.Generic;
using System.Linq;
using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IPM.UnitTest.BusinessTest
{
    [TestClass]
    public class RoomBusinessTest
    {
        private Mock<IRoomRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IRoomBusiness _roomBusiness;
        private List<Room> _listRoom;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRoomRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _roomBusiness = new RoomBusiness(_mockRepository.Object, _mockUnitOfWork.Object);
            _listRoom = new List<Room>()
            {
                new Room() {RoomID = 1 ,Name="Room1"},
                new Room() {RoomID = 2 ,Name="Room2"},
                new Room() {RoomID = 3 ,Name="Room3"}
            };
        }

        [TestMethod]
        public void Room_Business_GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.GetAll(null)).Returns(_listRoom);

            //call action
            var result = _roomBusiness.GetAll() as List<Room>;

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

    }
}
