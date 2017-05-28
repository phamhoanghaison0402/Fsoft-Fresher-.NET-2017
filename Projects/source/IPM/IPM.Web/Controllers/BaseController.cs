using IPM.Common;
using log4net;
using System;
using System.Web;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILog log;

        public BaseController()
        {
            log = LogManager.GetLogger(GetType().Name);
        }

        /// <summary>
        /// Custom notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="type"> Type of notification
        ///     - success: successfull notification, green color
        ///     - warning: warning notification, yellow color
        ///     - error: error notification, red color
        /// </param>
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
                TempData["AlertType"] = "alert-success";
            else if (type == "warning")
                TempData["AlertType"] = "alert-warning";
            else if (type == "error")
                TempData["AlertType"] = "alert-danger";
        }

        // Here I have created this for execute each time any controller (inherit this) load 
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
            {
                lang = langCookie.Value;
            }
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                {
                    lang = userLang;
                }
                else
                {
                    lang = SiteLanguages.GetDefaultLanguage();
                }
            }

            new SiteLanguages().SetLanguage(lang);

            return base.BeginExecuteCore(callback, state);
        }
    }
}