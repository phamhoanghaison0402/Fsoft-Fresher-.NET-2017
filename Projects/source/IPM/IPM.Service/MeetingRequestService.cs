using IPM.Business;
using log4net;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using MeetingRequest = IPM.Model.Models.MeetingRequest;

namespace IPM.Service
{
    public interface IMeetingRequestService : IServiceBase<MeetingRequest>
    {

        /// <summary>
        /// Get All MR flowing account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<MeetingRequest> GetAll(string account);

        String CreateMeetingRequest(ExchangeService service, MeetingRequest meetingRequest);

        /// <summary>
        /// Edit MR
        /// </summary>
        /// <param name="service"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        bool EditMeetingRequest(ExchangeService service, MeetingRequest meetingRequest);

        /// <summary>
        /// Cancel MR
        /// </summary>
        /// <param name="service"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        bool CancelMeetingRequest(ExchangeService service, MeetingRequest meetingRequest);
    }

    /// <summary>
    /// Meeting Request Service
    /// </summary>
    public class MeetingRequestService : ServiceBase<MeetingRequest>, IMeetingRequestService
    {
        private readonly IMeetingRequestBusiness _meetingRequestBusiness;
        private readonly ILog _log = LogManager.GetLogger("MeetingRequestService.cs");

        public MeetingRequestService(IMeetingRequestBusiness meetingRequestBusiness) : base(meetingRequestBusiness)
        {
            _meetingRequestBusiness = meetingRequestBusiness;
        }

        public IEnumerable<MeetingRequest> GetAll(string account)
        {
            return _meetingRequestBusiness.GetAll(account);
        }

        public String CreateMeetingRequest(ExchangeService service, MeetingRequest meetingRequest)
        {
            try
            {
                Appointment meeting = new Appointment(service);

                meeting.Subject = meetingRequest.Subject;
                meeting.Body = meetingRequest.EmailContent;
                meeting.Start = meetingRequest.StartDate;
                meeting.End = meetingRequest.EndDate;
                meeting.Location = meetingRequest.RoomID.ToString();

                if (meetingRequest.InterviewerEmail.Contains(';'))
                {
                    string[] emails = meetingRequest.InterviewerEmail.Split(';');
                    foreach (string email in emails)
                    {
                        //fix: if one of email is empty string, do not send
                        if (!String.IsNullOrWhiteSpace(email))
                        {
                            meeting.RequiredAttendees.Add(email);
                        }                            
                    }
                }
                else
                {
                    meeting.RequiredAttendees.Add(meetingRequest.InterviewerEmail);
                }
                meeting.Save(SendInvitationsMode.SendToAllAndSaveCopy);
                _log.Info("Create meeting request success");

                return meeting.Id.ToString();
            }
            catch
            {
                _log.Error("Create meeting request failed");
                return null;
            }
        }
        /// <summary>
        /// Edit meeting request send mail throught exchange service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        public bool EditMeetingRequest(ExchangeService service, MeetingRequest meetingRequest)
        {
            try
            {
                //Bind to an existing meeting request by using its unique identifier.
                Appointment meeting = Appointment.Bind(service, new ItemId(meetingRequest.AppointmentID));

                meeting.Subject = meetingRequest.Subject;
                meeting.Body = meetingRequest.EmailContent;
                meeting.Start = meetingRequest.StartDate;
                meeting.End = meetingRequest.EndDate;
                meeting.Location = meetingRequest.RoomID.ToString();

                if (meetingRequest.InterviewerEmail.Contains(';'))
                {
                    string[] emails = meetingRequest.InterviewerEmail.Split(';');
                    foreach (string email in emails)
                    {
                        meeting.RequiredAttendees.Add(email);
                    }
                }
                else
                {
                    meeting.RequiredAttendees.Add(meetingRequest.InterviewerEmail);
                }
                meeting.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy);
                _log.Info("Edit meeting request success");

                return true;
            }
            catch
            {
                _log.Error("Edit meeting request failed");

                return false;
            }
        }
        /// <summary>
        /// Cancel meeting request send mail throught exchange service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="meetingRequest"></param>
        /// <returns></returns>
        public bool CancelMeetingRequest(ExchangeService service, MeetingRequest meetingRequest)
        {
            try
            {
                _log.Info("Begin cancel meeting request");
                //Bind to an existing meeting request by using its unique identifier.
                Appointment meeting = Appointment.Bind(service, new ItemId(meetingRequest.AppointmentID));
                // Delete the appointment and move the deleted appointment to the Deleted Items folder.
                meeting.Delete(DeleteMode.MoveToDeletedItems);
                _log.Info("Cancel meeting request success");

                return true;
            }
            catch
            {
                _log.Error("Cancel meeting request failed");

                return false;
            }
        }
    }
}