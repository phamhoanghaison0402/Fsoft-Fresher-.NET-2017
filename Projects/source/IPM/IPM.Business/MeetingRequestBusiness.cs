using System;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Model.Models;
using System.Collections.Generic;
using log4net;

namespace IPM.Business
{
    public interface IMeetingRequestBusiness : IBusinessBase<MeetingRequest>
    {
        IEnumerable<MeetingRequest> GetAll(String account);

    }

    /// <summary>
    /// Meeting Request Business
    /// </summary>
    public class MeetingRequestBusiness : BusinessBase<MeetingRequest>, IMeetingRequestBusiness
    {
        private readonly IMeetingRequestRepository _meetingRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetingRequestBusiness(IMeetingRequestRepository meetingRequestRepository, IUnitOfWork unitOfWork) : base(meetingRequestRepository, unitOfWork)
        {
            _meetingRequestRepository = meetingRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MeetingRequest> GetAll(string account)
        {
            return _meetingRequestRepository.Get(x => x.InterviewAdmin == account, null, "");
        }

        
    }
}