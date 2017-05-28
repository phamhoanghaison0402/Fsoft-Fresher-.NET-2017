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
using System.Configuration;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// Class inhenritance basecontroller.
    /// </summary>
    public class PositionController : BaseController
    {
        private IPositionService _positionService;
        private readonly ILog _log = LogManager.GetLogger("PositionController.cs");

        public PositionController(IPositionService positionService)
        {
            this._positionService = positionService;
        }
        
        /// <summary>
        /// Action index of position controller.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int? pageSize, int page = 1)
        {
            int intPageSize = (pageSize ?? Int32.Parse(ConfigurationManager.AppSettings["PageSize"].Split(',')[0]));
            var positions = _positionService.GetAll().ToPagedList(page, intPageSize);
    
            return View(positions);
        }

        /// <summary>
        /// Function create new position.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(string code, string name)
        {
            try
            {
                if (!_positionService.ValidateCode(code) || !_positionService.ValidateName(name))
                {
                    SetAlert(IPM.Web.MultiLanguage.Resource.DataUnvalid, "error");
                    _log.Info("create unsuccessfully: " + IPM.Web.MultiLanguage.Resource.DataUnvalid);
                }
                else
                {
                    if (_positionService.CheckCode(code) || _positionService.CheckName(name))
                    {
                        SetAlert(IPM.Web.MultiLanguage.Resource.CodeExist, "error");
                        _log.Info("create unsuccessfully, code exist");
                    }
                    else
                    {
                        Position position = new Position
                        {
                            Name = name,
                            Code = code,
                            Active = true
                        };
                        _positionService.Insert(position);
                        _positionService.Save();
                        SetAlert(IPM.Web.MultiLanguage.Resource.CreateSuPo, "success");
                        _log.Info("create position successfully ");
                    }
                }
            }
            catch (Exception ex)
            {
                SetAlert(IPM.Web.MultiLanguage.Resource.CreateErPo, "error");
                _log.Error("error, create not successfully" + ex.Message);
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function update position
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(string id, string code, string name)
        {
            int idBrowser = Int32.Parse(id);

            try
            {
                if (!_positionService.ValidateCode(code) || !_positionService.ValidateName(name))
                {
                    SetAlert(IPM.Web.MultiLanguage.Resource.DataUnvalid, "error");
                    _log.Info("create unsuccessfully: " + IPM.Web.MultiLanguage.Resource.DataUnvalid);
                }
                else
                {
                    if (_positionService.CheckCode(code) || _positionService.CheckName(name))
                    {
                        SetAlert(IPM.Web.MultiLanguage.Resource.NameExist, "error");
                        _log.Info("create unsuccessfully, code exist");
                    }
                    else
                    {
                        Position position = new Position
                        {
                            ID = idBrowser,
                            Code = code,
                            Name = name,
                            Active = true
                        };
                        _positionService.Update(position);
                        _positionService.Save();
                        SetAlert(IPM.Web.MultiLanguage.Resource.EditSuPo, "success");
                        _log.Info("update position successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                SetAlert(IPM.Web.MultiLanguage.Resource.EditErPo, "error");
                _log.Error("error update not succsessfully" +ex.Message);
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// function delete position
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string id)
        {
            int idBrowser = Int32.Parse(id);

            try
            {
                _positionService.Delete(idBrowser);
                _positionService.Save();
                SetAlert(IPM.Web.MultiLanguage.Resource.DeleteSuPo, "success");
                _log.Info("delete position successfully");
            }
            catch (Exception ex)
            {
                SetAlert(IPM.Web.MultiLanguage.Resource.DeleteErPo, "error");
                _log.Error("error, delete failed" +ex.Message);
            }

            return Json(new { result = "1" }, JsonRequestBehavior.AllowGet);
        }
    }
}