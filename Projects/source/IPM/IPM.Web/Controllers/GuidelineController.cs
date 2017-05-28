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
    /// <summary>
    /// Declare GuidelineController.
    /// </summary>
    public class GuidelineController : BaseController
    {
        private readonly IGuidelineService _GuidelineService;
        private readonly ICatalogService _CatalogService;
        private readonly IGuidelineCatalogService _GuidelineCatalogService;
        public GuidelineController(IGuidelineService GuidelineService, IGuidelineCatalogService GuidelineCatalogService, ICatalogService CatalogService)
        {
            _GuidelineService = GuidelineService;
            _CatalogService = CatalogService;
            _GuidelineCatalogService = GuidelineCatalogService;
        }

        /// <summary>
        /// GET: List Guideline .
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            try
            {
                var TypeObject = _GuidelineService.GetAll();
                ViewBag.CurrentFilter = searchString;
                ViewBag.CurrentSort = sortOrder;

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    TypeObject = TypeObject.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper()));
                }
                TypeObject = TypeObject.OrderByDescending(s => s.Name);
                log.Info(IPM.Web.MultiLanguage.Resource.LoadGuidelineSuccess);
                var searchResult = TypeObject.ToPagedList(pageNumber, pageSize);

                return View(searchResult);
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.LoadGuidelineFails + ex.Message);
                SetAlert(IPM.Web.MultiLanguage.Resource.LoadGuidelineFails, "error");

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// POST: Create guideline .
        /// </summary>
        /// <param name="name"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(string name, string point)
        {
            int point2 = Int16.Parse(point);

            try
            {
                Guideline guideline = new Guideline
                {
                    Name = name,
                    Active = true
                };

                _GuidelineService.Insert(guideline);
                _GuidelineService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.CreateGuidelineSuccess);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateGuidelineSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.CreateGuidelineFails + ex.Message);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateGuidelineFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// POST:  Update Guideline .
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(string id, string name)
        {
            int id2 = Int32.Parse(id);

            try
            {
                Guideline guideline = new Guideline
                {
                    ID = id2,
                    Name = name,
                    Active = true
                };

                _GuidelineService.Update(guideline);
                _GuidelineService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.UpdateGuidelineSuccess);
                SetAlert(IPM.Web.MultiLanguage.Resource.UpdateGuidelineSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.UpdateGuidelineFails + ex.Message);
                SetAlert(IPM.Web.MultiLanguage.Resource.UpdateGuidelineFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// POST : Delete Guideline .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            int id2 = Int32.Parse(id);

            try
            {
                _GuidelineService.ChangeActive(id2);
                _GuidelineService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.DeteleGuidelineSuccess);
                SetAlert(IPM.Web.MultiLanguage.Resource.DeteleGuidelineSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.DeteleGuidelineFails);
                SetAlert(IPM.Web.MultiLanguage.Resource.DeteleGuidelineFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GET : List catalog in guideline .
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Catalog(int id, int page = 1)
        {
            int intPageSize = 10;

            try
            {
                ViewBag.guideline = _GuidelineService.GetSingleById(id);
                ViewBag.AvailableCatalog = _GuidelineService.AvailableCatalog(id).ToList();
                var guidelineCatalogs = _GuidelineService.GetCatalog(id).ToPagedList(page, intPageSize);
                log.Info(IPM.Web.MultiLanguage.Resource.LoadCatalogSuccess);

                return View(guidelineCatalogs);
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.LoadCatalogFails);
                SetAlert(IPM.Web.MultiLanguage.Resource.LoadCatalogFails, "error");

                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// POST : Add new catalog to DB and add to existing Guideline .
        /// </summary>
        /// <param name="idguideline"></param>
        /// <param name="addnewcatalogname"></param>
        /// <param name="addnewcatalogpoint"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddNewCatalog(string idguideline, string addnewcatalogname, string addnewcatalogpoint)
        {
            int idguideline2 = Int32.Parse(idguideline);
            Catalog newCatalog = new Catalog();

            newCatalog.Name = addnewcatalogname;
            newCatalog.MaxPoint = int.Parse(addnewcatalogpoint);
            _CatalogService.Insert(newCatalog);
            try
            {


                GuidelineCatalog guidelineCatalog = new GuidelineCatalog
                {

                    GuidelineID = idguideline2,
                    CatalogID = (_CatalogService.GetAll().Last().ID + 1),
                    Active = true
                };
                _GuidelineCatalogService.Insert(guidelineCatalog);
                _GuidelineCatalogService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.CreateCatalogSuccess);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateCatalogSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.CreateCatalogFails);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateCatalogFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// POST : Add Catalog existing in DB to Guideline .
        /// </summary>
        /// <param name="idguideline"></param>
        /// <param name="idcatalog"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddCatalog(string idguideline, string idcatalog)
        {
            int idguideline2 = Int32.Parse(idguideline);
            int idcatalog2 = Int32.Parse(idcatalog);

            try
            {
                GuidelineCatalog guidelineCatalog = new GuidelineCatalog
                {
                    GuidelineID = idguideline2,
                    CatalogID = idcatalog2,
                    Active = true
                };
                _GuidelineCatalogService.Insert(guidelineCatalog);
                _GuidelineCatalogService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.CreateCatalogSuccess);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateCatalogSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.CreateCatalogFails);
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateCatalogFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// POST : Remove catalog from guideline .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCatalog(string id)
        {
            int id2 = Int32.Parse(id);

            try
            {
                _GuidelineCatalogService.Delete(id2);
                _GuidelineCatalogService.Save();
                log.Info(IPM.Web.MultiLanguage.Resource.DeteteCatalogSuccess + id2);
                SetAlert(IPM.Web.MultiLanguage.Resource.DeteteCatalogSuccess, "success");
            }
            catch (Exception ex)
            {
                log.Error(IPM.Web.MultiLanguage.Resource.DeteteCatalogFails + id2);
                SetAlert(IPM.Web.MultiLanguage.Resource.DeteteCatalogFails, "error");
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }
    }
}