using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IPM.Data;
using IPM.Model.Models;
using IPM.Business;
using IPM.Service;
using IPM.Web.Models;
using PagedList;
using IPM.Web.MultiLanguage;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// Class Question controller
    /// </summary>
    public class QuestionsController : BaseController
    {
        private IPMDbContext db = new IPMDbContext();
        IQuestionService _questionService;
        ISkillService _skillService;
        private string success = "success";
        private string error = "error";
        /// <summary>
        /// Init Question controller
        /// </summary>
        /// <param name="questionService"></param>
        /// <param name="skillService"></param>
        public QuestionsController(IQuestionService questionService, ISkillService skillService)
        {
            this._questionService = questionService;
            this._skillService = skillService;
        }

        /// <summary>
        /// GET: Questions
        /// </summary>
        public ActionResult Index(int? page, int? numberOfRows)
        {
            //Vừa vào hoặc giá trị là null thì đưa vào trang 1
            var pageNumber = page ?? 1;
            int recordNumber = numberOfRows ?? 10;

            List<QuestionViewModel> listquestionvm = new List<QuestionViewModel>();
            List<Question> listquestion = (List<Question>)_questionService.GetAll();
            foreach (var item in listquestion)
            {
                QuestionViewModel a = new QuestionViewModel();
                a.ID = item.ID;
                a.Answer = item.Answer;
                a.Content = item.Content;
                a.Active = item.Active;
                a.SkillID = item.SkillID;
                a.SkillName = _questionService.SkillName(item.SkillID);
                listquestionvm.Add(a);
            }
            ViewBag.Skill = _skillService.GetAll();

            return View(listquestionvm.OrderBy(l=>l.ID).ToPagedList(pageNumber, recordNumber));
            
        }

        /// <summary>
        /// Add question 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public void Create(Question question)
        {
            try
            {
                _questionService.Insert(question);
                _questionService.Save();
                SetAlert(Resource.CreateQuestionSuccess, success);
            }
            catch (Exception ex)
            {
                SetAlert(Resource.CreateQuestionFails + ex.Message, error);
            }
            
        }

        /// <summary>
        /// Update quetsion
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public void Update(Question question)
        {

            try
            {
                _questionService.Update(question);
                _questionService.Save();
                SetAlert(Resource.UpdateQuestionSuccess, success);
            }
            catch (Exception ex)
            {
                SetAlert(Resource.UpdateQuestionFails + ex.Message, error);
            }
            
        }

        /// <summary>
        /// Delete Question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(int id)
        {
            try { 
            _questionService.Delete(id);
            _questionService.Save();
            SetAlert(Resource.DeteleQuestionSuccess, success);
            }
            catch( Exception ex)
            {
                SetAlert(Resource.DeteleQuestionFails + ex.Message, error);
            }         
        }

        /// <summary>
        /// Dispose db
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
