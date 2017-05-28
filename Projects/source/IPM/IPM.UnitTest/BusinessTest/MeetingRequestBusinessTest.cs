using IPM.Business;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IPM.UnitTest.BusinessTest
{
    [TestClass]
    public class MeetingRequestBusinessTest
    {
        private Mock<IMeetingRequestRepository> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private IMeetingRequestBusiness _meetingRequestBusiness;
        private List<MeetingRequest> _listMeetingRequests;
        private List<MeetingRequest> _listNull;

        /// <summary>
        /// Initial
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IMeetingRequestRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _meetingRequestBusiness = new MeetingRequestBusiness(_mockRepository.Object, _mockUnitOfWork.Object);
            _listMeetingRequests = new List<MeetingRequest>()
            {
                new MeetingRequest() {ID = 1, AppointmentID = "1",EmailContent = "abc",
                    InterviewAdmin = "ThuongTT2", StartDate = DateTime.Now,EndDate = DateTime.Now.AddHours(2),
                    InterviewerEmail = "thuongtt2@fsoft.fpt,vn",RoomID = 1, Subject = "Phong Van", Active = true},
            };
            _listNull = null;
        }

        /// <summary>
        /// Get all
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_GetAll()
        {
            //setup method
            _mockRepository.Setup(m => m.Get(null, null, "")).Returns(_listMeetingRequests);

            //call action
            var result = _meetingRequestBusiness.GetAll().ToList();

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        /// <summary>
        /// Get all
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_GetAllFlowAccount()
        {
            String account = "ThuongTT2";
            //setup method
            _mockRepository.Setup(m => m.Get(x=>x.InterviewAdmin == "ThuongTT2", null, "")).Returns(_listMeetingRequests);

            //call action
            var result = _meetingRequestBusiness.GetAll("ThuongTT2").ToList();

            //compare
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Insert
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_Insert()
        {
            MeetingRequest meetingRequest = new MeetingRequest();
            int id = 1;
            meetingRequest.Subject = "Test";

            _mockRepository.Setup(m => m.Add(meetingRequest)).Returns((MeetingRequest p) =>
            {
                p.ID = 1;
                return p;
            });

            var result = _meetingRequestBusiness.Insert(meetingRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

        /// <summary>
        /// Update meeting request
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_Update()
        {
            MeetingRequest meetingRequest = new MeetingRequest();
            int id = 1;
            meetingRequest.Subject = "Test";

            _mockRepository.Setup(m => m.Update(meetingRequest));

            var result = _meetingRequestBusiness.Insert(meetingRequest);

            Assert.AreEqual(true, true);
        }
        /// <summary>
        /// Test for function GetSingleByID
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_GetSingleById()
        {
            MeetingRequest meetingRequest = new MeetingRequest();

            meetingRequest.Subject = "Test";
            meetingRequest.ID = 1;

            _mockRepository.Setup(m => m.GetSingleById(It.IsAny<int>())).Returns(meetingRequest);

            var result = _meetingRequestBusiness.GetSingleById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

        /// <summary>
        /// Test for function delete
        /// </summary>
        [TestMethod]
        public void MeetingRequest_Business_Delete()
        {
            MeetingRequest meetingRequest = new MeetingRequest();

            meetingRequest.ID = 1;

            _mockRepository.Setup(m => m.Delete(It.IsAny<int>())).Returns(meetingRequest);

            var result = _meetingRequestBusiness.Delete(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ID);
        }

        [TestMethod]
        public void Save()
        {
            //setup method
            _mockUnitOfWork.Setup(m => m.Commit());

            //call action
            _meetingRequestBusiness.Save();

            //compare
            Assert.AreEqual(true, true);
        }
    }
}
