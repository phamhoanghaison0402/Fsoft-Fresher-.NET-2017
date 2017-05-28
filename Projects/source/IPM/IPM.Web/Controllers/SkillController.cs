using AutoMapper;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using System;
using System.Web.Mvc;
using PagedList;
using log4net;
using IPM.Web.MultiLanguage;
using System.Collections.Generic;
using System.Configuration;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// skill controller class
    /// </summary>
    public class SkillController : BaseController
    {
        private ISkillService _skillService;
        private readonly ILog _log = LogManager.GetLogger("SkillController.cs");
        public SkillController(ISkillService skillService)
        {
            this._skillService = skillService;
        }

        /// <summary>
        /// Return list of skill
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string search, int? pagesize, int page = 1)
        {
            var skills = _skillService.GetAll();
            int intPageSize = (pagesize ?? Int32.Parse(ConfigurationManager.AppSettings["PageSize"].Split(',')[0]));

            if (search != null)
            {
                skills = _skillService.Search(search);
            }
            skills = skills.ToPagedList(page, intPageSize);
            ViewBag.CurrentFilter = search;
            ViewBag.Page = page;
            ViewBag.PageSize = intPageSize;
            _log.Info("Load list of skill");
   
            return View(skills);
        }

        /// <summary>
        /// Ajax insert new skill
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(string name)
        {
            try
            {
                if (!_skillService.ValidateName(name))
                {
                    SetAlert(Resource.SkillNameUnvalid, "error");
                    _log.Info("Create skill unsuccess, Error: Name is unvalid");
                }
                else
                {
                    if (_skillService.CheckName(name))
                    {
                        SetAlert(Resource.ErrorNameExistSkill, "error");
                        _log.Info("Create skill unsuccess, Error: Name is exist");
                    }
                    else
                    {
                        Skill skill = new Skill
                        {
                            Name = name,
                            Active = true
                        };
                        _skillService.Insert(skill);
                        _skillService.Save();
                        SetAlert(Resource.CreateSkillSuccess, "success");
                        _log.Info("Create skill success");
                    }
                }
            }
            catch(Exception ex)
            {
                SetAlert(Resource.CreateSkillFailed, "error");
                _log.Error("Create skill unsuccess " + ex.Message);
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax update a skill
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(int id, string name)
        {
            try
            {
                if (!_skillService.ValidateName(name))
                {
                    SetAlert(Resource.SkillNameUnvalid, "error");
                    _log.Info("Create skill unsuccess, Error: Name is unvalid");
                }
                else
                {
                    if (_skillService.CheckName(name))
                    {
                        SetAlert(Resource.ErrorNameExistSkill, "error");
                        _log.Info("Update skill unsuccess, Error: Name is exist");
                    }
                    else
                    {
                        Skill skill = new Skill
                        {
                            ID = id,
                            Name = name,
                            Active = true
                        };
                        _skillService.Update(skill);
                        _skillService.Save();
                        SetAlert(Resource.UpdateSkillSuccess, "success");
                        _log.Info("Update skill success");
                    }
                }
            }
            catch (Exception ex)
            {
                SetAlert(Resource.UpdateSkillFailed, "error");
                _log.Info("Update skill unsuccess " + ex.Message);
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax delete a skill
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                _skillService.Delete(id);
                _skillService.Save();
                SetAlert(Resource.DeleteSkillSuccess, "success");
                _log.Info("Delete skill success");
            }
            catch (Exception ex)
            {
                SetAlert(Resource.DeleteSkillFailed, "error");
                _log.Error("Delete skill unsuccess " + ex.Message);
            }
            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }
    }
}