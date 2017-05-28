using AutoMapper;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using IPM.Web.MultiLanguage;


namespace IPM.Web.Controllers
{
    /// <summary>
    /// Interview Controller.
    /// </summary>
    public class InterviewController : BaseController
    {
        /// <summary>
        /// Call Service for Interivew, User ( to get ID of Interviewer to get right access ), Reference ( to get Infomations of Guideline ) , page (pageNumber ), pageSize.
        /// Set constaint FIRST_PAGE and DEFAULT_PAGE_SIZE
        /// </summary>
        const int FIRST_PAGE = 1;
        private readonly IInterviewService _interviewService;
        private readonly IInterviewAnswersService _interviewAnswerservice;
        private readonly IAnswerQuestionService _answerQuestionService;
        private readonly IUserService _userService;
        private readonly ICatalogService _catalogService;
        private readonly IQuestionService _questionService;
        private readonly ICatalogQuestionService _catalogQuestionService;

        /// <summary>
        /// Constructor for services: Interview, InterviewAnswer, User, AnswerQuestion, Catalog, Question, CatalogQuestion.
        /// </summary>
        /// <param name="interviewService"></param>
        /// <param name="interviewAnswerservice"></param>
        /// <param name="userService"></param>
        /// <param name="answerQuestionService"></param>
        /// <param name="catalogService"></param>
        /// <param name="questionService"></param>
        public InterviewController(IInterviewService interviewService, IInterviewAnswersService interviewAnswerservice,
            IUserService userService, IAnswerQuestionService answerQuestionService,
            ICatalogService catalogService, IQuestionService questionService, ICatalogQuestionService catalogQuestionService)

        {
            this._interviewService = interviewService;
            this._interviewAnswerservice = interviewAnswerservice;
            this._answerQuestionService = answerQuestionService;
            this._userService = userService;
            this._catalogService = catalogService;
            this._questionService = questionService;
            this._catalogQuestionService = catalogQuestionService;
        }

        /// <summary>
        /// List Interview Scheduel of User (Role-Interviewer) via User Account get by Session["Account"], have Paging.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(int? pageSize)
        {
            try
            {
                int thisPageSize = (pageSize ?? Int32.Parse(ConfigurationManager.AppSettings["PageSize"].Split(',')[0]));
                int page = FIRST_PAGE;
                var useraccount = Session["Account"].ToString();
                if (useraccount == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var interviewerId = _userService.GetIDbyAccount(useraccount);
                var interviews = _interviewService.GetInterviewList(interviewerId).ToPagedList(page, thisPageSize);
                return View(interviews);
            }
            catch (Exception ex)
            {
                log.Error(Resource.LoadInterviewScheduleFail + ":" + ex.Message);
                SetAlert(Resource.SystemMaintain, "error");
                return RedirectToAction("Index", "Home");
            }
           

        }

        /// <summary>
        /// Get All Interview have Result other than null (meaning the interview has done), have Paging.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult InterviewResults(int? pageSize)
        {
            int thisPageSize = (pageSize ?? Int32.Parse(ConfigurationManager.AppSettings["PageSize"].Split(',')[0]));
            int page = FIRST_PAGE;
            try
            {
                IPagedList<Interview> interviews = _interviewService.GetInterviewResultList().ToPagedList(page, thisPageSize);
                return View(interviews);
            }
            catch(Exception ex)
            {
                log.Error(Resource.LoadInterviewResultsFail + ":" + ex.Message);
                SetAlert(Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Get Interview Answer depend on Interview ID +  Generate Empty Answer for the Interview.
        /// </summary>
        /// <param name="itv"></param>
        /// <returns></returns>
        public Interview GenerateInterviewAnswers(Interview itv)
        {
            try
            {
                itv.InterviewAnswers = _interviewAnswerservice.GetInterviewAnswersList(itv.ID);
                var itvInterviewAnswers = itv.InterviewAnswers as IList<InterviewAnswer> ?? itv.InterviewAnswers.ToList();
                if (!itvInterviewAnswers.Any())
                {
                    var catalogids = _catalogService.GetAll(itv.RoundProcess.InterviewRound.Guideline.ID);
                    foreach (var idcatalog in catalogids)
                    {
                        _interviewAnswerservice.Insert(new InterviewAnswer() { InterviewID = itv.ID, CatalogID = idcatalog });
                        _interviewAnswerservice.Save();
                        var questionids = _questionService.GetAll(idcatalog);
                        var currentid = _interviewAnswerservice.GetLastInserted();
                        foreach (int questionid in questionids)
                        {
                            _answerQuestionService.Insert(new AnswerQuestion() { InterviewAnswerID = currentid, QuestionID = questionid });
                        }
                        _answerQuestionService.Save();
                    }
                    itv.InterviewAnswers = _interviewAnswerservice.GetInterviewAnswersList(itv.ID);
                }
                return itv;
            }
            catch(Exception ex)
            {
                log.Error(Resource.GenerateInterviewAnswerFail + ":" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Map Interviews info to InterviewViewModel for loading to View.
        /// </summary>
        /// <param name="itvs"></param>
        public InterviewListViewModel LoadInterviewInfo(IEnumerable<Interview> itvs)
        {
            try
            {
                var interviews = itvs as IList<Interview> ?? itvs.ToList();
                InterviewListViewModel interviewList = new InterviewListViewModel { Candidate = interviews[0].Candidate };
                interviewList.Interviews = new List<InterviewViewModel>();
                foreach (var item in interviews)
                {
                    Interview itv = GenerateInterviewAnswers(item);
                    InterviewViewModel interview = Mapper.Map<Interview, InterviewViewModel>(itv);
                    interview.ListInterviewAnswers = new List<InterviewAnswerViewModel>();
                    foreach (var item1 in itv.InterviewAnswers)
                    {
                        InterviewAnswerViewModel interviewAnswer =
                            Mapper.Map<InterviewAnswer, InterviewAnswerViewModel>(item1);
                        interviewAnswer.ListAnswerQuestions = new List<AnswerQuestionViewModel>();
                        foreach (var item2 in item1.AnswerQuestions)
                        {
                            AnswerQuestionViewModel answerQuestion =
                                Mapper.Map<AnswerQuestion, AnswerQuestionViewModel>(item2);
                            interviewAnswer.ListAnswerQuestions.Add(answerQuestion);
                        }
                        interviewAnswer.RowSpan = item1.AnswerQuestions.Count() + 1;
                        interview.ListInterviewAnswers.Add(interviewAnswer);
                    }
                    interviewList.Interviews.Add(interview);
                }
                return interviewList;
            }
            catch (Exception ex)
            {
                log.Error(Resource.LoadInterviewFail + ":" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Interview Infos of the Candidate get from selected interview, and Load to Interview Process view for Interview.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult InterviewProcess(int id)
        {
            try
            {
                if (CheckNegative(id))
                {
                    return RedirectToAction("Index");
                }
                var itv = _interviewService.GetSingleById(id);
                if (CheckNull(itv))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.CurrentRound = itv.ID;
                var itvs = _interviewService.GetInterviewListofCandidate(itv.CandidateID);
                InterviewListViewModel interviewList = LoadInterviewInfo(itvs);
                ViewBag.Questions = _questionService.GetAll();
                return View(interviewList);
            }
            catch(Exception ex)
            {
                log.Error(Resource.LoadInterviewFail + ":" + ex.Message);
                SetAlert(Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Get Interview Results of the Candidate get from selected interview, and Load to Interview Result view for Interview.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult InterviewResult(int id)
        {
            try
            {
                if(CheckNegative(id))
                {
                    return RedirectToAction("Index");
                }
                var itv = _interviewService.GetSingleById(id);
                if(CheckNull(itv))
                {
                    return RedirectToAction("Index");
                }
                ViewBag.CurrentRound = itv.ID;
                var itvs = _interviewService.GetInterviewResultofCandidate(itv.CandidateID);
                InterviewListViewModel interviewList = LoadInterviewInfo(itvs);
                return View(interviewList);
            }
            catch(Exception ex)
            {
                log.Error(String.Format(Resource.LoadInterviewFail + ":" + ex.Message));
                SetAlert(Resource.SystemMaintain, "error");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Update Candidate Answer or Comment of that Question.
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        [HttpPost]
        public bool UpdateAnswer(string pk, string value, string name)
        {
            try
            {
                int key = Convert.ToInt32(pk);
                if (key <= 0) return false;
                AnswerQuestion selectedQuestion = _answerQuestionService.GetSingleById(key);
                if (CheckNull(selectedQuestion)) return false;
                if (name == "CandidateAnswer")
                {
                    selectedQuestion.CandidateAnswer = value;
                }
                else
                {
                    selectedQuestion.Comment = value;
                }
                _answerQuestionService.Save();
                return true;
            }
            catch(Exception ex)
            {
                log.Error(Resource.SetAswerorCommentFail + ":" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Update Point for the Interview Answer.
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="value"></param>
        [HttpPost]
        public bool UpdateCatalogMark(string pk, string value)
        {
            try
            {
                int key = Convert.ToInt32(pk);
                if (key <= 0) return false;
                InterviewAnswer selectedAnswer = _interviewAnswerservice.GetSingleById(key);
                if (CheckNull(selectedAnswer)) return false;
                selectedAnswer.Mark = Convert.ToInt32(value);
                _interviewAnswerservice.Save();
                return true;
            }
            catch(Exception ex)
            {
                log.Error(Resource.SetCatalogMarkFail + ":" +  ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Set result Pass or Fail for the Interview
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="value"></param>
        [HttpPost]
        public bool SetResult(string pk, string value)
        {
            try
            {
                int key = Convert.ToInt32(pk);
                if (key <= 0) return false;
                Interview selectedInterview = _interviewService.GetSingleById(key);
                if (CheckNull(selectedInterview)) return false;
                if (value == "Pass")
                {
                    selectedInterview.Result = true;
                }
                else if (value == "Fail")
                {
                    selectedInterview.Result = false;
                }
                else
                {
                    log.Error(Resource.SetResultFail + Resource.PassOrFail);
                    return false;
                }
                _interviewService.Save();
                return true;
            }
            catch(Exception ex)
            {
                log.Error(Resource.SetResultFail + ":" + ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Add Question to Guideline during Interview process.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetQuestion(string id, string pk)
        {
            int key = Convert.ToInt32(id);
            if (key <= 0) return null;
            Question newQuestion = _questionService.GetSingleById(key);
            if (CheckNull(newQuestion)) return null;
            /// Add new Answer Question to the selected Interview Answer 
            try
            {
                AnswerQuestion newAnswerQuestion = _answerQuestionService.Insert(
                    new AnswerQuestion() { Question = newQuestion, QuestionID = newQuestion.ID, InterviewAnswerID = Convert.ToInt32(pk) });
                _answerQuestionService.Save();
                /// Add new CatalogQuestion to use for other interview
                CatalogQuestion newCatalogQuestion = _catalogQuestionService.Insert(
                    new CatalogQuestion() { CatalogID = newAnswerQuestion.InterviewAnswerID, QuestionID = newAnswerQuestion.QuestionID });
                _catalogQuestionService.Save();
                var viewModal = new { idquestion = newAnswerQuestion.QuestionID, content = newAnswerQuestion.Question.Content, answer = newAnswerQuestion.Question.Answer, id = newAnswerQuestion.ID, pk = newAnswerQuestion.InterviewAnswerID };
                return Json(viewModal);
            }
            catch(Exception ex)
            {
                log.Error(Resource.AddQuestionFail + ":" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Get list insertable Question.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListQuestion(string id)
        {
            int key = Convert.ToInt32(id);
            if (key <= 0) return Json(null);
            /// Select all Question in selected Catalog
            try
            {
                List<int> listExitsQuestionId = _questionService.GetAll(key);
                List<Question> listQuestion = _questionService.GetAll().ToList();
                var list = listQuestion.Select(x => new {
                    ID = x.ID,
                    Content = x.Content
                });
                return Json(new { existList = listExitsQuestionId, list = list }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error(Resource.LoadQuestionFails + ":" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Check if ID is negative or equal 0.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckNegative(int id)
        {
            if (id <= 0)
            {
                log.Info(Resource.IDPositive);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if object is null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckNull(object obj)
        {
            if (obj == null)
            {
                log.Info(Resource.ObjectNotNull);
                return true;
            }
            return false;
        }
    }
}