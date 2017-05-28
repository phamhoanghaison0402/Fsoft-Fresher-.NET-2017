using AutoMapper;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// Interview process controller.
    /// </summary>
    public class InterviewProcessController : BaseController
    {
        IInterviewProcessService _interviewProcessService;
        IPositionService _positionService;
        IRoundProcessService _roundProcessService;
        IInterviewRoundService _interviewRoundService;
        IGuidelineService _guideLineService;
        /// <summary>
        /// Empty string.
        /// </summary>
        const string stringEmpty = "";
        /// <summary>
        /// number of item empty list interview round.
        /// </summary>
        const int intListEmpty = 0;
        /// <summary>
        /// result add interview process failed.
        /// </summary>
        const int intAddProcessFailed = 0;
        /// <summary>
        /// bad object id.
        /// </summary>
        const int intBadId = 0;
        /// <summary>
        /// Max interview process name length.
        /// </summary>
        const int intMaxLengthProcessName = 100;

        /// <summary>
        /// interview process controller contructor.
        /// </summary>
        /// <param name="interviewProcessService">interview process service.</param>
        /// <param name="positionService">position service.</param>
        /// <param name="roundProcessService">round process service.</param>
        /// <param name="interviewRoundService">interview process service.</param>
        /// <param name="guideLineService">guideline service.</param>
        public InterviewProcessController(IInterviewProcessService interviewProcessService,
            IPositionService positionService, IRoundProcessService roundProcessService,
            IInterviewRoundService interviewRoundService, IGuidelineService guideLineService)
        {
            this._interviewProcessService = interviewProcessService;
            this._positionService = positionService;
            this._roundProcessService = roundProcessService;
            this._interviewRoundService = interviewRoundService;
            this._guideLineService = guideLineService;
        }
        /// <summary>
        /// Interview process management page.
        /// </summary>
        /// <param name="search">search string to search interview process.</param>
        /// <param name="page">page.</param>
        /// <param name="pageSize">page size.</param>
        /// <returns>interview process management page.</returns>
        [HttpGet]
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            log.Info(String.Format("Fucntion: Index - Search: {0} - page: {1} - pageSize: {2}", search, page, pageSize));
            try
            {
                //Get list position
                var listPositionname = _positionService.GetAll().Select(x => new
                {
                    Text = x.Name,
                    Value = x.ID
                }).ToList();
                ViewBag.ListPosition = new SelectList(listPositionname, "Value", "Text");
                ViewBag.SearchStr = search;

                //Get list interview process
                IEnumerable<InterviewProcess> interviewProcess = null;
                if (search == null)
                {
                    interviewProcess = _interviewProcessService.GetAll().OrderByDescending(p => p.ID).ToPagedList(page, pageSize);
                }
                else
                {
                    interviewProcess = _interviewProcessService.Search(search).OrderByDescending(p => p.ID).ToPagedList(page, pageSize);
                }

                return View(interviewProcess);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Get list interview process load page failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return RedirectToAction("Home");
            }
        }
        /// <summary>
        /// Create interview process page.
        /// </summary>
        /// <returns>create interview process page.</returns>
        public ActionResult Create()
        {
            log.Info("Function: Create");
            try
            {
                //Get list position
                var listPositionname = _positionService.GetAll().Select(x => new
                {
                    Text = x.Name,
                    Value = x.ID
                }).ToList();
                ViewBag.ListPosition = new SelectList(listPositionname, "Value", "Text"); ;

                //Get all list interview round
                List<InterviewRound> listIR = _interviewRoundService.GetAll().ToList();
                var listIRname = listIR.Select(x => new
                {
                    Text = x.RoundName,
                    Value = x.ID
                }).ToList();
                ViewBag.ListIRname = new SelectList(listIRname, "Value", "Text");

                return View();
            }
            catch (Exception e)
            {
                log.Error(String.Format("Create interview process page load failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Get round detail.
        /// </summary>
        /// <param name="roundId">interview round id. Default: 0.</param>
        /// <returns>Json object: interview round.</returns>
        [HttpGet]
        public ActionResult GetDetailRound(int roundId = intBadId)
        {
            log.Info(String.Format("Fucntion: GetDetailRound - Id: {0}", roundId));
            if (roundId == intBadId)
            {
                log.Info(String.Format("roundId: {0} - Message: Get Detail interview round failed", roundId));
                SetAlert(MultiLanguage.Resource.InterviewRoundNotFound, "error");
                return new HttpStatusCodeResult(404);
            }
            try
            {
                InterviewRound result = _interviewRoundService.GetSingleById(roundId);
                if (result == null)
                {
                    log.Info(String.Format("Id: {0} - Get round detail failed!", roundId));
                    SetAlert(MultiLanguage.Resource.DetailInterviewRoundPageLoadFailed, "error");
                    return new HttpStatusCodeResult(404);
                }
                InterviewRoundViewModel roundViewModel = Mapper.Map<InterviewRound, InterviewRoundViewModel>(result);
                return Json(roundViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Get round detail failed: {0} - Message: {1}", roundId, e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return new HttpStatusCodeResult(404);
            }
        }
        /// <summary>
        /// Create interview process.
        /// </summary>
        /// <param name="process">interview process.</param>
        /// <param name="listRound">list interview round in process.</param>
        /// <returns>Http status code result.</returns>
        [HttpPost]
        public ActionResult AddProcesses(InterviewProcessViewModel process, List<InterviewRoundViewModel> listRound)
        {
            log.Info(String.Format("Fucntion: AddProcesses - process: {0} - listRound: {1}", process, listRound));
            //validate input data
            if (process == null)
            {
                SetAlert(MultiLanguage.Resource.InterviewProcessNull, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.ProcessName == stringEmpty || process.ProcessName == null)
            {
                SetAlert(MultiLanguage.Resource.ProcessNameNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.ProcessName.Length > intMaxLengthProcessName)
            {
                SetAlert(MultiLanguage.Resource.InterviewProcessNameTooLong, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.StartDate == null)
            {
                SetAlert(MultiLanguage.Resource.ProcessStartDateNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }
            if (listRound == null || listRound.Count == intListEmpty)
            {
                SetAlert(MultiLanguage.Resource.ProcessListRoundNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }
            //add interview process
            try
            {
                InterviewProcess interviewProcess = Mapper.Map<InterviewProcessViewModel, InterviewProcess>(process);

                List<RoundProcess> listRoundProcess = new List<RoundProcess>();
                foreach (InterviewRoundViewModel round in listRound)
                {
                    RoundProcess roundProcess = new RoundProcess();
                    roundProcess.InterviewRoundID = round.ID;

                    listRoundProcess.Add(roundProcess);
                }

                int addProcessResult = _interviewProcessService.AddProcess(interviewProcess, listRoundProcess);
                if (addProcessResult == intAddProcessFailed)
                {
                    log.Info(String.Format("Process: {0} - ListRound: {1} - Message: Add interview process failed", process, listRound));
                    SetAlert(MultiLanguage.Resource.AddInterviewProcessFailed, "error");
                    return new HttpStatusCodeResult(404);
                }
                SetAlert(MultiLanguage.Resource.AddProcessSuccess, "success");
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Add interview process failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return new HttpStatusCodeResult(404);
            }
        }
        /// <summary>
        /// Edit interview process page.
        /// </summary>
        /// <param name="id">interview process id. Default: 0.</param>
        /// <returns>interview process edit page.</returns>
        [HttpGet]
        public ActionResult Edit(int id = intBadId)
        {
            log.Info(String.Format("Fucntion: Edit - Id: {0}", id));
            if (id == intBadId)
            {
                log.Info(String.Format("Id: {0} - Message: Edit interview process page load failed", id));
                SetAlert(MultiLanguage.Resource.EditInterviewProcessFailed, "error");
                return RedirectToAction("Index");
            }
            try
            {
                //Get interview process by id
                InterviewProcess interviewProcess = _interviewProcessService.GetProcessById(id);
                if (interviewProcess == null)
                {
                    log.Info(String.Format("Id: {0} - Message: Edit interview process not found", id));
                    SetAlert(MultiLanguage.Resource.InterviewProcessNotFound, "error");
                    return RedirectToAction("Index");
                }
                //Get list position
                var listPositionname = _positionService.GetAll().Select(x => new
                {
                    Text = x.Name,
                    Value = x.ID
                }).ToList();
                ViewBag.ListPosition = new SelectList(listPositionname, "Value", "Text"); ;
                //Get all list interview round
                List<InterviewRound> listIR = _interviewRoundService.GetAll().ToList();
                var listIRname = listIR.Select(x => new
                {
                    Text = x.RoundName,
                    Value = x.ID
                }).ToList();
                ViewBag.ListIRname = new SelectList(listIRname, "Value", "Text");
                //map view model and view bag
                InterviewProcessViewModel interviewProcessMapped = Mapper.Map<InterviewProcess, InterviewProcessViewModel>(interviewProcess);
                ViewBag.interviewProcess = interviewProcessMapped;

                List<InterviewRoundViewModel> ListRoundInProcess = new List<InterviewRoundViewModel>();
                List<RoundProcess> listRoundProcess = interviewProcess.RoundProcesses.OrderBy(x=>x.RoundOrder).ToList();

                foreach (RoundProcess roundProcess in listRoundProcess)
                {
                    InterviewRoundViewModel interviewRoundMapped = Mapper.Map<InterviewRound, InterviewRoundViewModel>(roundProcess.InterviewRound);
                    ListRoundInProcess.Add(interviewRoundMapped);
                }
                ViewBag.ListRoundInProcess = ListRoundInProcess;

                return View(interviewProcess);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Edit interview process page load failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Edit interview process.
        /// </summary>
        /// <param name="process">interview process.</param>
        /// <param name="listRound">list interview round in process.</param>
        /// <returns>Http status code result.</returns>
        [HttpPost]
        public ActionResult EditProcess(InterviewProcessViewModel process, List<InterviewRoundViewModel> listRound)
        {
            log.Info(String.Format("Fucntion: EditProcess - process: {0} - listRound: {1}", process, listRound));
            //validate input data
            if (process == null)
            {
                SetAlert(MultiLanguage.Resource.InterviewProcessNull, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.ProcessName == "" || process.ProcessName == null)
            {
                SetAlert(MultiLanguage.Resource.ProcessNameNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.ProcessName.Length > intMaxLengthProcessName)
            {
                SetAlert(MultiLanguage.Resource.InterviewProcessNameTooLong, "error");
                return new HttpStatusCodeResult(404);
            }
            if (process.StartDate == null)
            {
                SetAlert(MultiLanguage.Resource.ProcessStartDateNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }
            if (listRound == null || listRound.Count == 0)
            {
                SetAlert(MultiLanguage.Resource.ProcessListRoundNotEmpty, "error");
                return new HttpStatusCodeResult(404);
            }

            //add process
            try
            {
                //map data from view model to model
                InterviewProcess interviewProcess = Mapper.Map<InterviewProcessViewModel, InterviewProcess>(process);
                List<RoundProcess> listRoundProcess = new List<RoundProcess>();
                foreach (InterviewRoundViewModel round in listRound)
                {
                    RoundProcess roundProcess = new RoundProcess();
                    roundProcess.InterviewRoundID = round.ID;
                    roundProcess.InterviewProcessID = process.ID;
                    listRoundProcess.Add(roundProcess);
                }
                //edit interview process
                InterviewProcess editProcessResult = _interviewProcessService.Edit(interviewProcess, listRoundProcess);
                if (editProcessResult == null)
                {
                    log.Info(String.Format("Process: {0} - ListRound: {1} - Message: Edit interview process failed", process, listRound));
                    SetAlert(MultiLanguage.Resource.EditInterviewProcessFailed, "error");
                    return new HttpStatusCodeResult(404);
                }

                SetAlert(MultiLanguage.Resource.EditProcessSuccess, "success");
                return new HttpStatusCodeResult(200);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Edit interview process page failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return new HttpStatusCodeResult(404);
            }
        }
        /// <summary>
        /// Detail interview process page.
        /// </summary>
        /// <param name="id">interview process id. Default: 0.</param>
        /// <returns>interview process detail page.</returns>
        [HttpGet]
        public ActionResult Detail(int id = intBadId)
        {
            log.Info(String.Format("Fucntion: Detail - Id: {0}", id));
            if (id == intBadId)
            {
                log.Info(String.Format("Id: {0} - Message: Detail interview process page load failed", id));
                SetAlert(MultiLanguage.Resource.DetailInterviewProcessPageLoadFailed, "error");
                return RedirectToAction("Index");
            }
            try
            {
                //find interview process by id
                InterviewProcess interviewProcess = _interviewProcessService.GetProcessById(id);
                if (interviewProcess == null)
                {
                    log.Info(String.Format("Id: {0} - Message: Edit interview process not found", id));
                    SetAlert(MultiLanguage.Resource.InterviewProcessNotFound, "error");
                    return RedirectToAction("Index");
                }
                //map data from model to view model
                InterviewProcessViewModel interviewProcessMapped = Mapper.Map<InterviewProcess, InterviewProcessViewModel>(interviewProcess);
                ViewBag.interviewProcess = interviewProcessMapped;

                List<InterviewRoundViewModel> ListRoundInProcess = new List<InterviewRoundViewModel>();
                List<RoundProcess> listRoundProcess = interviewProcess.RoundProcesses.OrderBy(x => x.RoundOrder).ToList();

                foreach (RoundProcess roundProcess in listRoundProcess)
                {
                    InterviewRoundViewModel interviewRoundMapped = Mapper.Map<InterviewRound, InterviewRoundViewModel>(roundProcess.InterviewRound);
                    ListRoundInProcess.Add(interviewRoundMapped);
                }
                ViewBag.ListRoundInProcess = ListRoundInProcess;

                return View(interviewProcess);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Detail interview process page load failed: {0}", e.Message));
                SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Render view for Detail InterviewRound.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>interview round detail partial view.</returns>
        public ActionResult RoundDetail(int id = intBadId)
        {
            log.Info(String.Format("Fucntion: RoundDetail - Id: {0}", id));
            if (id == intBadId)
            {
                log.Info(String.Format("Id: {0} - Message: Detail interview round page load failed", id));
                SetAlert(MultiLanguage.Resource.DetailInterviewRoundPageLoadFailed, "error");
                return new HttpStatusCodeResult(404);
            }
            else
            {
                try
                {
                    //get informattion
                    var interviewRound = _interviewRoundService.GetSingleById(id);
                    if (interviewRound == null)
                    {
                        log.Info(String.Format("Id: {0} - Message: Detail interview round page load failed", id));
                        SetAlert(MultiLanguage.Resource.DetailInterviewRoundPageLoadFailed, "error");
                        return new HttpStatusCodeResult(404);
                    }
                    ViewBag.ListGuidelineName = _guideLineService.GetAll().FirstOrDefault(x => x.ID == interviewRound.GuidelineID).Name;
                    ViewBag.ListProcess = _roundProcessService.GetByRoundId(id);
                    //render round detail
                    return PartialView("_RoundDetail", interviewRound);
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Id: {0} - Message: {1}", id, e.Message));
                    SetAlert(MultiLanguage.Resource.SystemMaintain, "error");
                    return RedirectToAction("Index");
                }
            }
        }
    }
}