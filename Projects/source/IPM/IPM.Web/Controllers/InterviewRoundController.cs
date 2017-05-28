using AutoMapper;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.MultiLanguage;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using IPM.Web.Mappings;
using IPM.Data.Infrastructure;
using PagedList;
using IPM.Data.Repositories;
using System.Linq;


namespace IPM.Web.Controllers
{

    /// <summary>
    /// Controler for InterviewRound
    /// </summary>
    /// <seealso cref="IPM.Web.Controllers.BaseController" />
    public class InterviewRoundController : BaseController
    {

        IInterviewRoundService _interviewRoundService;

        IInterviewProcessService _interviewProcessService;

        IGuidelineService _guideLineService;

        IRoundProcessService _roundProcessService;

        IInterviewRoundRepository _interviewRoundRepository;

        const int intFirstPage = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterviewRoundController"/> class.
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        /// <param name="guideLineService">The guide line service.</param>
        /// <param name="interviewProcessService">The interview process service.</param>
        /// <param name="roundProcessService">The round process service.</param>
        /// <param name="interviewRoundRepository">The interview round repository.</param>
        public InterviewRoundController(IInterviewRoundService interviewRound, IGuidelineService guideLineService,
                                        IInterviewProcessService interviewProcessService, IRoundProcessService roundProcessService,
                                        IInterviewRoundRepository interviewRoundRepository)
        {
            this._interviewRoundService = interviewRound;

            this._guideLineService = guideLineService;

            this._interviewProcessService = interviewProcessService;

            this._roundProcessService = roundProcessService;

            this._interviewRoundRepository = interviewRoundRepository;

        }


        /// <summary>
        /// Get list GuideLine
        /// </summary>
        public void SetViewBag()
        {
            ViewBag.ListGuideline = new SelectList(_guideLineService.GetAll(), "ID", "Name");
        }


        /// <summary>
        /// Indexes the specified current filter.
        /// </summary>
        /// <param name="currentFilter">The current filter.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
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

            var interviewRounds = _interviewRoundService.GetList(searchString);

            SetViewBag();

            return View(interviewRounds.OrderByDescending(x => x.ID).ToList().ToPagedList((int)intPageNumber, (int)intPageSize));
        }


        /// <summary>
        /// Render view for Create InterviewRound
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }


        /// <summary>
        /// Create InterviewRound
        /// </summary>
        /// <param name="interviewRound">The interview round.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(InterviewRound interviewRound)
        {
            try
            {

                _interviewRoundService.AddRound(interviewRound);
                SetAlert(MultiLanguage.Resource.CreateInterviewRoundSuccess, "success");
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                log.Error(string.Format("Insert InterviewRound , Message : {0}", ex.Message));
                SetAlert(MultiLanguage.Resource.CreateInterviewRoundFail, "error");
                return RedirectToAction("Index");
            }

        }

        /// <summary>
        /// Render view Edit InterviewRound
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            try
            {
                var interviewRounds = _interviewRoundService.ListInterviewRound((int)id).OrderBy(i => i.ID).ToPagedList(1, 1);
                SetViewBag();
                return PartialView("Edit", interviewRounds);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("InterviewRound ID : {0} , Message : {1}", id, ex.Message));
                SetAlert(MultiLanguage.Resource.InterviewRoundNotFound, "error");
                return RedirectToAction("Index");
            }
        }


        /// <summary>
        /// Submit Edit InterviewRound
        /// </summary>
        /// <param name="id">InterviewRound.ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(InterviewRound interviewRound)
        {
            try
            {

                _interviewRoundService.Update(interviewRound);
                SetAlert(MultiLanguage.Resource.EditInterviewRoundSuccess, "success");
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                log.Error(string.Format("Update InterviewRound , Message : {0}", ex.Message));
                SetAlert(MultiLanguage.Resource.EditInterviewRoundFail, "error");
                return View();
            }


        }


        /// <summary>
        /// Render view for Delete InterviewRound 
        /// </summary>
        /// <param name="id">InterviewRound.ID</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            try
            {
                var interviewRounds = _interviewRoundService.ListInterviewRound((int)id).OrderBy(i => i.ID).ToPagedList(1, 1);
                return PartialView("Delete", interviewRounds);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("InterviewRound ID : {0} , Message : {1}", id, ex.Message));
                SetAlert(MultiLanguage.Resource.InterviewRoundNotFound, "error");
                return RedirectToAction("Index");
            }
        }


        /// <summary>
        /// Update interviewRound set active=false
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _interviewRoundService.Update_Status(id);
                SetAlert(MultiLanguage.Resource.DeleteInterviewRoundSuccess, "success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(string.Format("InterviewRound ID : {0} , Message : {1}", id, ex.Message));
                SetAlert(MultiLanguage.Resource.DeleteInterviewRoundFail, "error");
                return RedirectToAction("Index");
            }


        }


        /// <summary>
        /// Render view for Detail InterviewRound
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int? id)
        {
            try
            {
                var interviewRounds = _interviewRoundService.ListInterviewRound((int)id).OrderBy(i => i.ID).ToPagedList(1, 1);
                SetViewBag();
                ViewBag.ListProcess = _roundProcessService.GetByRoundId((int)id);
                return PartialView("Detail", interviewRounds);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("InterviewRound ID : {0} , Message : {1}", id, ex.Message));
                SetAlert(MultiLanguage.Resource.InterviewRoundNotFound, "error");
                return RedirectToAction("Index");
            }
        }

    }
}