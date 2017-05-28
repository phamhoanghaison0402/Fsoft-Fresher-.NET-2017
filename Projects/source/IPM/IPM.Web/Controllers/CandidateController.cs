using AutoMapper;
using IPM.Common.CustomExceptions;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using IPM.Web.MultiLanguage;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    public class CandidateController : BaseController
    {

        private readonly ICandidateService _candidateService;
        private readonly IPositionService _positionService;
        private readonly IInterviewAdminService _inteviewAdminService;
        private readonly ISkillService _skillService;
        private readonly ICandidateSkillService _candidateSkillService;
        private readonly ICertificateService _certificateService;

        public CandidateController(
            ICandidateService candidateService, 
            IPositionService positionService, 
            IInterviewAdminService interviewAdminService, 
            ISkillService skillService, 
            ICandidateSkillService candidateSkillService,
            ICertificateService certificateService
            )
        {
            this._candidateService = candidateService;
            this._positionService = positionService;
            this._inteviewAdminService = interviewAdminService;
            this._skillService = skillService;
            this._candidateSkillService = candidateSkillService;
            this._certificateService = certificateService;
        }
        /// <summary>
        /// Show Index View of Candidate
        /// </summary>
        /// <param name="pageNumber">page number of view</param>
        /// <returns>List candidate</returns>
        public ActionResult Index(int? recordNumber, int pageNumber = 1)
        {
            log.Info("Index Candidate");
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

                if (lstCandidate.Count == 0)
                {
                    SetAlert(Resource.DataIsNull, "error");
                }

                return View(candidates);
            }
            catch (Exception e)
            {
                log.Error("Get All Candidate is fail: ",e);
                SetAlert(Resource.LoadCandidateIsFail, "error");
            }

            return View();
        }

        /// <summary>
        /// Search candidates with search data(name, phone...) and concident data(color)
        /// </summary>
        /// <param name="frm">Form collection</param>
        /// <returns>Index View with list candidate</returns>
        public ActionResult Search(FormCollection frm)
        {
            //get data user need search
            var searchData = frm["searchData"];
            var concidentData = frm["concidentData"];

            //log info
            log.Info(string.Format("Search candidate: {0} - {1}", searchData, concidentData));
            var candidates = new List<Candidate>();

            if(searchData == "" && concidentData == null)
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
            catch(Exception e)
            {
                log.Error(e.Message);
                SetAlert(Resource.CandidateNotFound, "error");
            }
            ViewBag.record = candidates.Count;

            //if candidates.Count = 0, pagelist is error, so set candidates.Count = 1
            var record = candidates.Count;
            if(record == 0)
            {
                record = 1;
            }

            return View("Index", candidates.OrderByDescending(z => z.ID).ToPagedList(1, record));
        }

        /// <summary>
        /// Delete candidates
        /// </summary>
        /// <param name="Id">Candidate id</param>      
        /// <returns>Index view< with message</returns>
        public ActionResult DeleteCandidate(int Id)
        {
            log.Info(string.Format("Delete candidate id: {0}", Id));

            try
            {
                if (_candidateService.DeleteCandidate(Id))
                {
                    _candidateService.Save();
                    SetAlert(Resource.DeleteCandidateSuccessful, "success");
                }             
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                SetAlert(Resource.DeleteCandidateFail, "error");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// View candidate profile information.
        /// </summary>
        /// <param name="id">Candidate id</param>
        /// <returns>Candidate profile view</returns>
        public ActionResult Detail(int id)
        {
            log.Info("View candidate profile.");
            try
            {
                var candidate = _candidateService.GetCandidateProfile(id);

                return View(candidate);
            }
            catch(DatabaseException)
            {
                // Error logged at business layer.
                SetAlert(Resource.CanNotGetCandidateProfile, "error");
            }
            catch (EntityNotFoundException)
            {
                // Error logged at business layer.
                SetAlert(Resource.CandidateNotFound, "error");
            }
            catch (Exception ex)
            {
                log.Error("Can not get candidate profile. " + ex.Message);
                SetAlert(Resource.CanNotGetCandidateProfile, "error");
            }

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Add new candidates screen: Load position from database.
        /// </summary>
        public ActionResult Create()
        {
            try
            {
                var positions = _positionService.GetAll();
                var candidateVm = new CandidateViewModel
                {
                    Positions = positions
                };

                return View(candidateVm);
            }
            catch (Exception ex)
            {
                SetAlert(ex.Message, "error");

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Add new candidates into database.
        /// </summary>
        /// <param name="candidateVm"></param>
        [HttpPost]
        public ActionResult Create(CandidateViewModel candidateVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var folderPath = ConfigurationManager.AppSettings["folderPath"];
                    var candidate = Mapper.Map<CandidateViewModel, Candidate>(candidateVm);
                    _candidateService.InsertCandidate(candidate, candidateVm.UploadedFiles, folderPath);
                    _candidateService.Save();
                    SetAlert(Resource.SuccessCandidateCreate, "success");

                    return Redirect("Index");
                }
            }
            catch(FileUploadErrorException)
            {
                // Error logged at business layer.
                SetAlert("Can not create candidate, save file failed.", "error");
            }
            catch (Exception e)
            {
                log.Error("Can not create candidate. " + e.Message);
                SetAlert(Resource.ErrorCandidateCreateFail, "error");
            }

            var positions = _positionService.GetAll();
            candidateVm.Positions = positions;

            return View(candidateVm);
        }

        /// <summary>
        /// Edit candidates: Load edit screen with data from database.
        /// </summary>
        /// <param name="id">Candidate.ID</param>
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var positions = _positionService.GetAll();
                var interviewAdmins = _inteviewAdminService.GetAll();
                var skills = _skillService.GetAll();
                var candidate = _candidateService.GetSingleById(id);
                //var certificates = _certificateService.GetAll();
                var candidateVm = Mapper.Map<Candidate, CandidateViewModel>(candidate);

                candidateVm.Positions = positions;
                candidateVm.InterviewAdmins = interviewAdmins;
                candidateVm.ListSkill = skills;

                //Load skill
                ViewBag.ListSkillOfCandidate = _candidateSkillService.GetListSkillByCandidateId(id);
                ViewBag.ListSkillNotCandidate = _candidateService.ListSkill(id);

                //Load certificate
                ViewBag.ListCertificateOfCandidate = _candidateService.GetCertificateByCandidateID(id);
                ViewBag.ListCertificateNotCandidate = _candidateService.ListCertificate(id);

                //candidateVm.CandidateCertificates = certificates;

                return View(candidateVm);
            }
            catch (Exception e)
            {
                log.Error("Can not get candidate profile. " + e.Message);
                SetAlert(Resource.ErrorCandidateLoadProfileFail, "error");

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Edit candidate's infomation then update database with change.
        /// </summary>
        /// <param name="candidateVm">Candidate View Model</param>
        [HttpPost]
        public ActionResult Edit(CandidateViewModel candidateVm)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    //Load list of positions after input not valid
                    var positions = _positionService.GetAll();
                    candidateVm.Positions = positions;

                    //Load list of interiew admins after input not valid
                    var interviewAdmins = _inteviewAdminService.GetAll();
                    candidateVm.InterviewAdmins = interviewAdmins;

                    //Load list of skills after input not valid
                    var skills = _skillService.GetAll();
                    candidateVm.ListSkill = skills;

                    return View(candidateVm);
                }

                if (ModelState.IsValid)
                {
                    var candidate = Mapper.Map<CandidateViewModel, Candidate>(candidateVm);
                    _candidateService.EditCandidate(candidate);

                    if (candidateVm.SelectSkillID != null)
                    {
                        _candidateSkillService.Update(candidate.ID, candidateVm.SelectSkillID.ToList());
                    }
                    
                    if(candidateVm.SelectCertificateId != null)
                    {
                        _candidateSkillService.UpdateCandidateCertificate(candidate.ID, candidateVm.SelectCertificateId.ToList());
                    }

                    _candidateService.Save();
                    SetAlert(Resource.SuccessCandidateUpdate, "success");
                }
            }
            catch (Exception e)
            {
                log.Error("Can not save candidate profile. " + e.Message);
                SetAlert(Resource.ErrorCandidateSaveProfileFail, "error");

                //Load list of positions after catch exception
                var positions = _positionService.GetAll();
                candidateVm.Positions = positions;

                //Load list of interiew admins after catch exception
                var interviewAdmins = _inteviewAdminService.GetAll();
                candidateVm.InterviewAdmins = interviewAdmins;

                //Load list of skills after catch exception
                var skills = _skillService.GetAll();
                candidateVm.ListSkill = skills;

                return View(candidateVm);
            }

            return RedirectToAction("Index");
        }
    }
}
