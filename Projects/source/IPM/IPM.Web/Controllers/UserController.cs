using System;
using System.Configuration;
using AutoMapper;
using IPM.Business;
using IPM.Model.Models;
using IPM.Service;
using IPM.Web.Models;
using log4net;
using System.Linq;
using System.Web.Mvc;
using IPM.Web.MultiLanguage;
using PagedList;

namespace IPM.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly ILog _log = LogManager.GetLogger("log");
        IUserService _userService;
        IRoleService _roleService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="roleService"></param>
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="roleSearch"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        
        [HttpGet]
        // GET List User
        public ActionResult Index(string searchString, int page = 1, int record=10)
        {
            try
            {
                ViewBag.SelectRecord = ConfigurationManager.AppSettings["PageSize"].Split
                    (new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                ViewBag.record = record;
                ViewBag.page = page;
                ViewBag.searchString = searchString;
                var inforUser = _userService.GetListUserByAccount(searchString).ToPagedList(page, record);
                _log.Info(string.Format("Load list user success."));

               return View(inforUser);   
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Load list user fail: {0}", ex.Message));
                SetAlert("Load list user fail","error");

                return Redirect("/Home");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // Create user method GET
        public ActionResult Create()
        {
            ViewBag.ListRole = _roleService.getRole();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // Create user method POST
        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            try
            {
                if (model.SelectRoleID == null)
                {
                    ModelState.AddModelError("RoleName", Resource.RoleNotChoose);
                }
                if (model.Account == null)
                {
                    ModelState.AddModelError("Account", Resource.AccountRequired);
                }
                else
                {
                    if (_userService.CheckExistAccount(model.Account))
                    {
                        ModelState.AddModelError("Account", Resource.AccountExist);
                    }
                    if (_userService.CheckAccountFpt(model.Account) == false)
                    {
                        ModelState.AddModelError("Account", Resource.AccountIncorrect);
                    }
                }
                ViewBag.ListRole = _roleService.getRole();
                if (ModelState.IsValid)
                {
                    User user = Mapper.Map<UserViewModel, User>(model);

                    if (_userService.Insert(user, model.SelectRoleID.ToList()))
                    {
                        SetAlert(Resource.AddUserSuccess, "success");
                        _log.Info(string.Format("{0}: Create success.", user.Account));

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _log.Info(string.Format("{0}: Create success.", user.Account));
                        SetAlert(Resource.AddUserFail, "error");
                    }
                }              
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Account:{0} - Create fail: {1}.", model.Account, ex.Message));
                SetAlert(Resource.AddUserFail, "error");
            }

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // Update user method GET
        public ActionResult Update(string account, UserViewModel model)
        {
            try
            {
                InfoUser user = _userService.GetUserByAccount(account);
                ViewBag.ListRole = _roleService.getRole();             
                ViewBag.user = user;
      
                return View();
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("{0}: Can't get user information: {1}.", model.Account, ex.Message));

                return RedirectToAction("Index");
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // Update user method Post
        [HttpPost]
        public ActionResult Update(UserViewModel model)
        {
            try
            {
                if (model.SelectRoleID == null || model.SelectRoleID.Length == 0)
                {
                    ModelState.AddModelError("RoleName",Resource.RoleNotChoose);
                }
                ViewBag.ListRole = _roleService.getRole();
                InfoUser user = _userService.GetUserByAccount(model.Account);
                ViewBag.user = user;

                if (ModelState.IsValid)
                {
                    User user1 = Mapper.Map<UserViewModel, User>(model);

                    if (_userService.Edit(user1, model.SelectRoleID.ToList()))
                    {
                        SetAlert(Resource.UpdateUserSuccess, "success");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert(Resource.UpdateUserFail, "error");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Account:{0} - Update fail: {1}.",model.Account, ex.Message));
                SetAlert(Resource.UpdateUserFail, "error");

                return View();
            }
        }     
    }
}