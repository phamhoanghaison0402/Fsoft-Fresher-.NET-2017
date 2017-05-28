using System.Web.Mvc;
using IPM.Data.Repositories;
using IPM.Service;
using PagedList;

namespace IPM.Web.Controllers
{
    /// <summary>   
    /// </summary>
    /// <seealso cref="IPM.Web.Controllers.BaseController" />
    public class InterviewerController : BaseController
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InterviewerController" /> class.
        /// </summary>
        /// <param name="interviewerService">The interviewer service.</param>
        /// <param name="userRoleRepository"></param>
        public InterviewerController(IInterviewerService interviewerService, IUserRoleRepository userRoleRepository)
        {
            InterviewerService = interviewerService;
        }

        /// <summary>
        ///     Gets the interviewer service.
        /// </summary>
        /// <value>
        ///     The interviewer service.
        /// </value>
        public IInterviewerService InterviewerService { get; }

        // GET: Interviewer
        /// <summary>
        ///     Indexes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var interviewer = InterviewerService.GetAll("").ToPagedList(page, pageSize);
            //if list interviewer not null
            if (interviewer != null)
            {
                return View(interviewer);
            }
            SetAlert("khong co interviewer nao ca", "error");

            return View();
        }
    }
}