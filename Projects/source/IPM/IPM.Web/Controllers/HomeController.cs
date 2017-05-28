using System;
using Authentication;
using IPM.Common;
using IPM.Service;
using Microsoft.Exchange.WebServices.Data;
using System.Web.Mvc;
using System.Web.Security;
using log4net;


namespace IPM.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly ILog _log = LogManager.GetLogger("log");

        /// <summary>
        /// 
        /// </summary>
        public struct Credentials
        {
            public string Username;
            public string Password;
        }

        IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logins the specified f.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            try
            {
                string account = f["account"];
                string password = f["password"];

                if (account != "" && password != "")
                {
                    bool checkLogin = _userService.CheckLogin(account, password);
                    if (checkLogin)
                    {
                        Session["Account"] = account;
                        FormsAuthentication.SetAuthCookie(account,false);
                        ExchangeService service =
                            Authentication.Service.ConnectToService(
                                UserDataFromConsole.GetUserData(account + "@fsoft.fpt.vn", password), new TraceListener());      
                        Session["Service"] = service;                                         
                    }
                    else
                    {
                        Session["Account"] = null;
                        TempData["notify"] = "Account and Password incorrect!!!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Account"] = null;
                    if (account == "")
                    {
                        TempData["notify1"] = "Please enter account!!!";
                    }
                    if (password == "")
                    {
                        TempData["notify2"] = "Please enter password!!!";
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Can't login: {0}", e.Message));

                return RedirectToAction("Index");
            }  
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Changes the language.
        /// </summary>
        /// <param name="lang">The language.</param>
        /// <returns></returns>
        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            new SiteLanguages().SetLanguage(lang);
            return Redirect(returnUrl);
        }
    }
}