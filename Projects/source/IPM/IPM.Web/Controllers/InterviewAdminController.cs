using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPM.Service;
using PagedList;
using IPM.Model.Models;
using IPM.Web.Models;
using log4net;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// Class interview admin controller.
    /// </summary>
    public class InterviewAdminController : BaseController
    {

        /// <summary>
        /// User role of interview admin.
        /// </summary>
        public const int INT_INTERVIEWADMIN_ROLE = 2;

        /// <summary>
        /// Result update success.
        /// </summary>
        public const int INT_UPDATE_SUCESS = 1;

        /// <summary>
        /// Result cannot update because not select other interview admin.
        /// </summary>
        public const int INT_CANNOT_UPDATE = 0;

        const int intFirstPage = 1;

        /// <summary>
        /// Object IInterviewAdminService service.
        /// </summary>
        private IInterviewAdminService _interviewAdminService;

        /// <summary>
        /// Object IInterviewService service.
        /// </summary>
        private IInterviewService _interviewService;

        /// <summary>
        /// Constructor with params: interview admin service, interview service.
        /// </summary>
        /// <param name="interviewAdminService"></param>
        /// <param name="interviewService"></param>
        public InterviewAdminController(IInterviewAdminService interviewAdminService, IInterviewService interviewService)
        {
            this._interviewAdminService = interviewAdminService;
            this._interviewService = interviewService;
        }
        
        /// <summary>
        /// Load list interview admin.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string currentFilter, string searchString, int? page, int? pageSize)
        {

            int? intPageSize = pageSize ?? 8;
            int? intPageNumber = page ?? 1;

            if (searchString != null)
            {
                page = intFirstPage;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.currentFilter = searchString;
            var interviewAdmins = _interviewAdminService.GetAllByRoleId(INT_INTERVIEWADMIN_ROLE, searchString);

            if (interviewAdmins != null)
            {
                log.Info("Load list interview admin success.");
            }
            else
            {
                log.Info("Interview admin not found.");
            }

            return View(interviewAdmins.OrderBy(i => i.ID).ToList().ToPagedList((int)intPageNumber, (int)intPageSize));
        }

        /// <summary>
        /// Get list candidate by InterviewAdminID (candidate of this interview admin manage).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult ViewListCandidate(string account, int interviewAdminId, int page = 1, int pageSize = 10)
        {
            
            // id of interview admin selected
            ViewBag.InterviewAdminId = interviewAdminId;

            // account of interview admin selected
            ViewBag.InterviewAdminAccount = account;

            // list candidate of interview admin manage
            var candidate = _interviewAdminService.GetCandidateByInterViewAdminID(interviewAdminId).ToPagedList(page, pageSize);

            if (candidate != null)
            {
                log.Info("Load list candidate success");
            }
            else
            {
                log.Info("candidate not found. with interview admin id = " + interviewAdminId);
            }

            return View(candidate);
        }

        /// <summary>
        /// Call transmit candidate dialog.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interviewAdminId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ViewPatial(int candidateId, int interviewAdminId, string name, int positionId)
        {
            
            // candidate selected
            Candidate can = new Candidate();
            can.ID = candidateId;
            can.Name = name;
            can.PositionID = positionId;
            can.InterviewAdminID = interviewAdminId;
            ViewBag.CandidateSelected = can;

            // list interview admin
            ViewBag.listInterviewAdmins = _interviewAdminService.GetAllByRoleId(INT_INTERVIEWADMIN_ROLE,"");

            // list interview of candidate selected
            ViewBag.interviews = _interviewAdminService.GetInterviewByInterviewAdmin(can);

            return PartialView("TransmitCandidatePatial");
        }

        /// <summary>
        /// Transmit candidate when click button "Transmit".
        /// </summary>
        /// <param name="candidateId"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult TransmitCandidate(int candidateId, FormCollection form)
        {

            // interview admin id current round
            var interviewAdminId = Convert.ToInt32(form["interviewAdminId"]);
            var interviewId = Convert.ToInt32(form["interviewId"]);

            // return result when update candidate
            int resultUpdidateCandidate = 0;

            // return result when update interview
            int resultUpdateInterview = 0;
            Candidate candidate = new Candidate();

            try
            {
                candidate.ID = candidateId;
                if(interviewAdminId != 0)
                {
                    resultUpdidateCandidate = _interviewAdminService.UpdateCandidate(candidate, interviewAdminId);
                    resultUpdateInterview = _interviewService.UpdateInterview(interviewId, interviewAdminId);
                }

                if (resultUpdidateCandidate == INT_UPDATE_SUCESS && resultUpdateInterview == INT_UPDATE_SUCESS)
                {
                    _interviewAdminService.Save();
                    _interviewService.Save();
                    SetAlert(@IPM.Web.MultiLanguage.Resource.TransmitSucess, "success");
                    log.Info("Transmit candidate success. with candidate id = " + candidateId);

                    return RedirectToAction("Index");
                }
                else if (resultUpdidateCandidate == INT_CANNOT_UPDATE || resultUpdateInterview == INT_CANNOT_UPDATE)
                {
                    log.Info("Cannot Trasnmit candidate with candidate id = " + candidateId);
                    SetAlert(@IPM.Web.MultiLanguage.Resource.SelectOtherInterviewAdmin, "error");

                    return RedirectToAction("Index");
                }
                else
                {
                    log.Info("Trasnmit candidate failed with candidate id = " + candidateId);
                    SetAlert(@IPM.Web.MultiLanguage.Resource.CannotTransmit, "error");

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                SetAlert(@IPM.Web.MultiLanguage.Resource.TransmitFail, "error");
                log.Error("Trasnmit candidate failed with candidate id = " + candidateId + "----" + ex.Message);

                return RedirectToAction("Index");
            }
           
        }
    }
}