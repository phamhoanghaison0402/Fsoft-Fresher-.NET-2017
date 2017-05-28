using AutoMapper;
using IPM.Service;
using IPM.Web.Models;
using log4net;
using Microsoft.Exchange.WebServices.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using IPM.Common;
using MeetingRequest = IPM.Model.Models.MeetingRequest;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IPM.Web.Controllers
{    
        /// <summary>
        /// MeetingRequest Controller
        /// </summary>
        public class MeetingRequestController : BaseController
    {
        private readonly IMeetingRequestService _meetingRequestService;
        private readonly IRoomService _roomService;
        private static ExchangeService _service = null;
        private readonly ILog _log = LogManager.GetLogger("MeetingRequestController.cs");
        


        public MeetingRequestController(IMeetingRequestService meetingRequestService, IRoomService roomService)
        {
            _meetingRequestService = meetingRequestService;
            _roomService = roomService;
        }
        /// <summary>
        /// Return list meeting request
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = 0)
        {
            string account = Session["account"] as String;
            string stringPageSize = ConfigurationManager.AppSettings["PageSize"];
            string[] arrPageSize = stringPageSize.Split(',');
            int intPageSize = 0;

            //initialize page size with first size in webconfig
            if (pageSize == 0)
            {
                intPageSize = int.Parse(arrPageSize[0]);
            }
            else
            {
                intPageSize = pageSize;
            }
            var model = _meetingRequestService.GetAll(account);
            var meetingRequestVm = Mapper.Map<IEnumerable<MeetingRequest>, IEnumerable<MeetingRequestViewModel>>(model);

            if (model != null)
            {
                _log.Info("Load list meeting request success");
            }
            else
            {
                _log.Info("No record not found");
            }

            return View(meetingRequestVm.ToPagedList(page, intPageSize));
        }
        /// <summary>
        /// Get list of room as SelectList
        /// </summary>
        /// <returns></returns>
        private SelectList GetRoomList()
        {
            var roomList = _roomService.GetAll();

            if (roomList != null)
            {
                _log.Info("Load room list success");
            }
            else
            {
                _log.Info("Room not found");
            }

            return new SelectList(roomList, "RoomID", "Name");
        }
        /// <summary>
        /// Validate MeetingRequestViewModel sending from client by using SetAlert
        /// Validate when create or edit a meeting request
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private bool ValidateMeetingRequestVm(MeetingRequestViewModel vm)
        {
            DateTime dtmTimeTemp = new DateTime();
            int iInt = 0;
            RegexUtilities util = new RegexUtilities();
            if (vm == null)
            {
                SetAlert("Please full fill information before pressing Send button!", "error");

                return false;
            }
            if (String.IsNullOrWhiteSpace(vm.InterviewerEmail))
            {
                SetAlert("Interviewer Emails could not be empty!", "error");

                return false;
            }
            var emailList = vm.InterviewerEmail.Split(';');

            if(emailList.Length < 1)
            {
                SetAlert("Interviewer Emails could not be empty!", "error");

                return false;
            }
            for (; iInt < emailList.Length; iInt++)
            {
                if (!String.IsNullOrWhiteSpace(emailList[iInt]))
                    break;
            }
            if(iInt >= emailList.Length)
            {
                SetAlert("Interviewer Emails list could not be empty!", "error");

                return false;
            }

            for (iInt = 0; iInt < emailList.Length; iInt++)
            {
                if (String.IsNullOrWhiteSpace(emailList[iInt]))
                    continue;
                if (!util.IsValidEmail(emailList[iInt].Trim()))
                    break;
            }
            if (iInt < emailList.Length)
            {
                SetAlert("Interviewer Emails invalid! Please review email list again.", "error");

                return false;
            }
            if (String.IsNullOrWhiteSpace(vm.Subject))
            {
                SetAlert("Subject could not be empty!", "error");

                return false;
            }
            if (vm.Subject.Length > 300)
            {
                //subject string: limit 300 chars
                SetAlert("Subject length must less than 300 characters!", "error");

                return false;
            }
            if ((int)vm.RoomID < 1)
            {
                SetAlert("Room haven't been choosen!", "error");

                return false;
            }
            if (!DateTime.TryParse(vm.StartDate.ToString(), out dtmTimeTemp)
                || vm.StartDate <= DateTime.MinValue || vm.StartDate.Year<1900)
            {
                SetAlert("Start Time not valid!", "error");

                return false;
            }
            if (!DateTime.TryParse(vm.EndDate.ToString(), out dtmTimeTemp)
                || vm.EndDate <= DateTime.MinValue || vm.StartDate.Year < 1900)
            {
                SetAlert("End Time not valid!", "error");

                return false;
            }
            if (vm.StartDate > vm.EndDate)
            {
                SetAlert("End Time must be later than Start Time", "error");

                return false;
            } 
                       
            return true;
        }
        /// <summary>
        /// Return view create meeting request
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListRoom = GetRoomList();
            return View();
        }
        /// <summary>
        /// Handle create meeting request
        /// </summary>
        /// <param name="meetingRequestVm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(MeetingRequestViewModel meetingRequestVm)
        {
            //validate viewmodel & return validation message if input invalid
            if (!ValidateMeetingRequestVm(meetingRequestVm))
            {
                ViewBag.ListRoom = GetRoomList();
                return View(meetingRequestVm);
            }
            _log.Info("Inputs valid");
            try
            {
                _service = Session["Service"] as ExchangeService;

                meetingRequestVm.AppointmentID = _meetingRequestService.CreateMeetingRequest(_service,
                    Mapper.Map<MeetingRequestViewModel, MeetingRequest>(meetingRequestVm));

                // Insert meeting request into database
                meetingRequestVm.InterviewAdmin = Session["Account"] as String;
                meetingRequestVm.Active = true;
                _meetingRequestService.Insert(Mapper.Map<MeetingRequestViewModel, MeetingRequest>(meetingRequestVm));
                _meetingRequestService.Save();
                _log.Info("Save meeting request success");

                SetAlert("Create Meeting Request successful!!!", "success");
                return RedirectToAction("Index", "MeetingRequest");
            }
            catch (Exception ex)
            {
                _log.Error("Save meeting request error");
                SetAlert("Error: " + ex + " !", "error");
            }
            
            return RedirectToAction("Index", "MeetingRequest");
        }
        /// <summary>
        /// Return view edit meeting request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.ListRoom = GetRoomList();
                var meetingrequest = _meetingRequestService.GetSingleById(id);
                var meetingrequestVm = Mapper.Map<MeetingRequest, MeetingRequestViewModel>(meetingrequest);
                _log.Info("Edit meeting request success");
                return View(meetingrequestVm);
            }
            catch (Exception ex)
            {
                _log.Error("Edit meeting request error");
                SetAlert(ex.Message, "error");
                return RedirectToAction("Index", "MeetingRequest");
            }
        }
        /// <summary>
        /// Handle edit meeting request
        /// </summary>
        /// <param name="meetingRequestVm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(MeetingRequestViewModel meetingRequestVm)
        {
            //validate viewmodel & return validation message if input invalid
            if (!ValidateMeetingRequestVm(meetingRequestVm))
            {
                ViewBag.ListRoom = GetRoomList();
                return View(meetingRequestVm);
            }
            _log.Info("Inputs valid");
            try
            {
                _service = Session["Service"] as ExchangeService;
                // Insert meeting request into database
                meetingRequestVm.InterviewAdmin = Session["Account"] as String;
                _meetingRequestService.EditMeetingRequest(_service, Mapper.Map<MeetingRequestViewModel, MeetingRequest>(meetingRequestVm));
                _meetingRequestService.Update(Mapper.Map<MeetingRequestViewModel, MeetingRequest>(meetingRequestVm));
                _meetingRequestService.Save();
                _log.Info("Edit meeting request success");

                SetAlert("Edit Meeting Request successful!!!", "success");
                return RedirectToAction("Index", "MeetingRequest");
            }
            catch (Exception ex)
            {
                _log.Error("Edit meeting request error");
                SetAlert("Error: " + ex + " !", "error");
            }
            
            return RedirectToAction("Index", "MeetingRequest");
        }
        /// <summary>
        /// Delete meeting request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete_Cancel(int id)
        {
            _log.Info("Get id Meeting Request");

            try
            {
                var meetingrequest = _meetingRequestService.GetSingleById(id);
                var meetingrequestVm = Mapper.Map<MeetingRequest, MeetingRequestViewModel>(meetingrequest);

                _service = Session["Service"] as ExchangeService;
                // Insert meeting request into database
                _meetingRequestService.CancelMeetingRequest(_service, Mapper.Map<MeetingRequestViewModel, MeetingRequest>(meetingrequestVm));
                _meetingRequestService.Delete(id);
                _meetingRequestService.Save();
                _log.Info("Cancel meeting request success");

                SetAlert("Cancel Meeting Request successful!!!", "success");
                return RedirectToAction("Index", "MeetingRequest");
            }
            catch (Exception ex)
            {
                _log.Error("Cancel meeting request error");
                SetAlert("Error: " + ex + " !", "error");
            }
            return RedirectToAction("Index", "MeetingRequest");
        }
    }
}