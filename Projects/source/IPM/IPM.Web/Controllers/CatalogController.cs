using AutoMapper;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly ICatalogService _CatalogService;
        private readonly ICatalogQuestionService _CatalogQuestionService;
        public CatalogController(ICatalogService CatalogService, ICatalogQuestionService CatalogQuestionService)
        {
            this._CatalogService = CatalogService;
            this._CatalogQuestionService = CatalogQuestionService;
        }

        // GET: Catalog
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var TypeObject = _CatalogService.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                TypeObject = TypeObject.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            TypeObject = TypeObject.OrderByDescending(s => s.Name);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var searchResult = TypeObject.ToPagedList(pageNumber, pageSize);
            return View(searchResult);
        }


        [HttpPost]
        public JsonResult Create(string name, string point)
        {
            int point2 = Int16.Parse(point);
            try
            {
                Catalog catalog = new Catalog
                {
                    Name = name,
                    MaxPoint = point2,
                    Active = true
                };
                _CatalogService.Insert(catalog);
                _CatalogService.Save();
                SetAlert("Add catalog success", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Add catalog failed: ", "error");
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Update(string id, string name, string point)
        {
            int id2 = Int32.Parse(id);
            int point2 = Int32.Parse(point);
            try
            {
                Catalog catalog = new Catalog
                {
                    ID = id2,
                    Name = name,
                    MaxPoint = point2,
                    Active = true
                };
                _CatalogService.Update(catalog);
                _CatalogService.Save();
                SetAlert("Update catalog success", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Update catalog failed: ", "error");
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            int id2 = Int32.Parse(id);
            try
            {
                _CatalogService.ChangeActive(id2);
                _CatalogService.Save();
                SetAlert("Delete catalog success", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Delete catalog uncuccess", "error");
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }


       
         public ActionResult Question(int id, int page = 1)
        {
            int intPageSize = 10;
            ViewBag.catalog = _CatalogService.GetSingleById(id);
            ViewBag.AvailableQuestion = _CatalogService.AvailableQuestion(id).ToList();
            var catalogquestions = _CatalogService.GetQuestion(id).ToPagedList(page, intPageSize);

            return View(catalogquestions);
        }

        
         [HttpPost]
        public JsonResult AddQuestion(string idcatalog, string idquestion)
        {
            int idcatalog2 = Int32.Parse(idcatalog);
            int idquestion2 = Int32.Parse(idquestion);
            try
            {
                CatalogQuestion catalogquestion = new CatalogQuestion
                {
                    CatalogID = idcatalog2,
                    QuestionID = idquestion2,
                    Active = true
                };
                _CatalogQuestionService.Insert(catalogquestion);
                _CatalogQuestionService.Save();
                SetAlert("Add Question success", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Delete Question uncuccess", "error");
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult DeleteQuestion(string id)
        {
            int id2 = Int32.Parse(id);
            try
            {
                _CatalogQuestionService.Delete(id2);
                _CatalogQuestionService.Save();
                SetAlert("Delete question success", "success");
            }
            catch (Exception ex)
            {
                SetAlert("Delete question uncuccess", "error");
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }
    }
}