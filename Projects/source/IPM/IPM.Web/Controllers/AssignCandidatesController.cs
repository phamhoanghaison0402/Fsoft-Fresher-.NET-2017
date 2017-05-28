using AutoMapper;
using IPM.Common.CustomExceptions;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using IPM.Web.MultiLanguage;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    public class AssignCandidatesController : BaseController
    {
        private readonly ICandidateService _candidateService;
        private readonly IInterviewerService _interviewerService;
        private readonly IInterviewService _interviewService;
        private readonly ILog _log = LogManager.GetLogger("AssignCandidatesController.cs");
        public AssignCandidatesController(ICandidateService candidateService, IInterviewerService interviewerService, IInterviewService interviewService)
        {
            this._candidateService = candidateService;
            this._interviewerService = interviewerService;
            this._interviewService = interviewService;
        }

        /// <summary>
        /// Index assigncadidate
        /// </summary>
        /// <param name="recordNumber"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ActionResult Index(int? recordNumber, int pageNumber = 1)
        {
            log.Info("Index Assign Candidate to interviewer");
            try
            {
                ViewBag.record = recordNumber;
                //send record to Search
                TempData["record"] = recordNumber;

                //set Select Record
                string[] lstRecord = ConfigurationManager.AppSettings["PageSize"].Split
                    (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                var record = recordNumber ?? int.Parse(lstRecord[0]);
                ViewBag.SelectRecord = lstRecord;

                //set status on Index page
                ViewBag.Status = "Index";

                //get list candidate from database
                var lstCandidate = _candidateService.GetAll().ToList();
                var candidates = lstCandidate.OrderByDescending(z => z.ID).ToPagedList(pageNumber, record);

                //Get color, title for concident status
                var concidenColorTitle = new string[6, 2];

                for (int i = 0; i < concidenColorTitle.GetLength(0); i++)
                {
                    concidenColorTitle[i, 0] = ConfigurationManager.AppSettings[String.Format("ConcidentColorLevel{0}", i)];
                    concidenColorTitle[i, 1] = ConfigurationManager.AppSettings[String.Format("ConcidentTitleLevel{0}", i)];
                }

                ViewBag.concidenColorTitle = concidenColorTitle;
                //
                //IEnumerable<User> result = _interviewerService.GetAll("");
                //ViewBag.interviewers = Mapper.Map<IEnumerable<User>, UserViewModel>(result);
                ViewBag.interviewers = _interviewerService.GetAll("");

                if (lstCandidate.Count == 0)
                {
                    SetAlert(Resource.DataIsNull, "error");
                }

                return View(candidates);
            }
            catch (Exception e)
            {
                log.Error("Get All Candidate to interviewer is fail: ", e);
                SetAlert(Resource.LoadCandidateIsFail, "error");
            }

            return View();
        }

        /// <summary>
        /// Search candidate in order to assign to interviewer
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        public ActionResult Search(FormCollection frm)
        {
            //get data user need search
            var searchData = frm["searchData"];
            var concidentData = frm["concidentData"];

            //log info
            log.Info(string.Format("Search candidate: {0} - {1}", searchData, concidentData));
            var candidates = new List<Candidate>();

            if (searchData == "" && concidentData == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                candidates = _candidateService.Search(searchData, concidentData);

                //Get color, title for concident
                var concidenColorTitle = new string[6, 2];

                for (int i = 0; i < concidenColorTitle.GetLength(0); i++)
                {
                    concidenColorTitle[i, 0] = ConfigurationManager.AppSettings[string.Format("ConcidentColorLevel{0}", i)];
                    concidenColorTitle[i, 1] = ConfigurationManager.AppSettings[string.Format("ConcidentTitleLevel{0}", i)];
                }

                //set Select Record
                string[] lstRecord = ConfigurationManager.AppSettings["PageSize"].Split
                    (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                ViewBag.SelectRecord = lstRecord;

                ViewBag.dataSearch = searchData;
                ViewBag.candidates = _candidateService.GetAll().ToList();
                ViewBag.concidenColorTitle = concidenColorTitle;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                SetAlert(Resource.CandidateNotFound, "error");
            }
            ViewBag.record = candidates.Count;
            ViewBag.interviewers = _interviewerService.GetAll("");

            //if candidates.Count = 0, pagelist is error, so set candidates.Count = 1
            var record = candidates.Count;
            if (record == 0)
            {
                record = 1;
            }

            return View("Index", candidates.OrderByDescending(z => z.ID).ToPagedList(1, record));
        }

        /// <summary>
        /// Assigning cadidate to interviewer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Assign()
        {
            log.Info("Assigning Candidate");
            try
            {
                int id = Int32.Parse(Request.Form["idinterview"]);
                DateTime date = DateTime.Parse(Request.Form["date"]);
                int idinterviewer = Int32.Parse(Request.Form["idinterviewer"]);

                Interview interview = _interviewService.GetSingleById(id);
                interview.StartTime = date;
                interview.InterviewerID = idinterviewer;

                _interviewService.Update(interview);
                _interviewService.Save();
                SetAlert("Assign success", "success");
            }
            catch(Exception ex)
            {
                SetAlert("Assign unsuccess", "error");
                log.Error("Assign unsuccess: " + ex.Message);
            }
            
            return Redirect("/AssignCandidates");
        }
    }
}