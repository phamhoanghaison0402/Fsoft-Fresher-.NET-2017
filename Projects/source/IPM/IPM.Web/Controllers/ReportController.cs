using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IPM.Service;
using IPM.Data.ModelReport;
using log4net;
using IPM.Web.Models;
using IPM.Data.Repositories;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using IPM.Common;


namespace IPM.Web.Controllers
{
    /// <summary>
    /// Controller Report
    /// </summary>
    public class ReportController : BaseController
    {
        private readonly IReportRepositoty _reportRepositoty;
        private readonly IReportService _reportService;
        private readonly IPositionService _positionService;

        /// <summary>
        /// Initialize Controller Report.
        /// </summary>
        /// <param name="reportService"></param>
        /// <param name="reportRepositoty"></param>
        public ReportController(IReportService reportService, IReportRepositoty reportRepositoty, IPositionService positionService)
        {
            _reportRepositoty = reportRepositoty;
            _reportService = reportService;
            _positionService = positionService;
        }

        /// <summary>
        /// Function return view ReportCandidates.cshtml.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCandidates()
        {
            return View();
        }

        /// <summary>
        /// Function return view ReportCandidatesGST.cshtml.
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCandidatesGst()
        {
            return View();
        }

        /// <summary>
        /// Function return view ReportInterviewers.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportInterviewers()
        {
            return View();
        }

        /// <summary>
        /// Function return view ReportSkillsandCareers.cshtml
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportSkillsandCareers()
        {
            ViewBag.ListPosition = _positionService.GetAll();

            return View();
        }

        /// <summary>
        /// Get Data report Candiddates with 4 params from FormReportViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GetDataReportCandidates(FormReportViewModel model)
        {
            if (model.FromDate == null && model.ToDate == null && model.Name == null && model.InterviewResult == null)
            {
                SetAlert(@IPM.Web.MultiLanguage.Resource.ErrorFillReport, "error");
                return Json("error");
            }
            else
            {
                Session["FormReportViewModel"] = model;
                var list = _reportService.GetReportCandidates(model.FromDate, model.ToDate, model.Name, model.InterviewResult)
                    .Select(i => new FormReportViewModel()
                    {
                        No = i.No,
                        Name = i.Name,
                        Result = i.Result,
                        Date = i.Date
                    }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Data Report Interviewers
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GetDataReportInterviewers(FormReportViewModel model)
        {
            Session["FormReportViewModel"] = model;
            DateTime? FromDate = null, ToDate = null;
            int? interviewResult = null;
            DateTime dtmTimeTemp;
            int iTemp;
            if (model.Name == null)
            {
                model.Name = "";
            }
            if (DateTime.TryParse(model.FromDate, out dtmTimeTemp))
            {
                FromDate = dtmTimeTemp;
            }
            if (DateTime.TryParse(model.ToDate, out dtmTimeTemp))
            {
                ToDate = dtmTimeTemp;
            }
            if (Int32.TryParse(model.InterviewResult, out iTemp))
            {
                if (iTemp == 0 || iTemp == 1 || iTemp == 2)
                    interviewResult = iTemp;
            }
            var list = _reportService.GetReportInterviewers(FromDate, ToDate, model.Name, interviewResult)
                .Select(i => new ReportInterviewers()
                {
                    ID = model.ID,
                    Name = model.Name,
                    Account = i.Account,
                    NumOfCandidates = i.NumOfCandidates,
                    PassFail = i.PassFail
                }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function return view ReportCandidatesGst.cshtml
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDataReportCandidatesGst(FormReportViewModel model)
        {
            if (model.FromDate == null && model.ToDate == null && model.Name == null && model.InterviewResult == null)
            {
                SetAlert(IPM.Web.MultiLanguage.Resource.ErrorFillReport, "error");
                return Json("error");
            }
            else
            {
                Session["FormReportViewModel"] = model;
                var list = _reportService.GetReportCandidatesGst(model.FromDate, model.ToDate, model.Name,
                    model.InterviewResult).Select(i => new FormReportViewModel()
                    {
                        No = i.No,
                        Name = i.Name,
                        Result = i.Result,
                        Date = i.Date,
                        Certificate = i.Certificate
                    }).ToList();


                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get data for Skill and Positon
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDataReportSkillAndPosition(FormReportViewModel model)
        {
            Session["FormReportViewModel"] = model;
            var list = _reportService.GetReportSkillAndPosition(model.FromDate, model.ToDate, model.Name, model.Position)
                .Select(i => new FormReportViewModel()
                {
                    No = "",
                    Name = i.Name,
                    Position = i.Position,
                    Skills = i.Skills
                }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export data Report Candidates to Excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcelReportCandidates()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;

                var list = _reportService.GetReportCandidates(model.FromDate, model.ToDate, model.Name, model.InterviewResult)
                .Select(i => new
                {
                    No = i.No,
                    Name = i.Name,
                    Result = (EnumResult)i.Result,
                    Date = i.Date,
                }).ToList();

                try
                {
                    //export excel
                    var gv = new GridView();
                    gv.DataSource = list;
                    gv.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=ReportCandidates.xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                    gv.RenderControl(objHtmlTextWriter);

                    Response.Output.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    log.Error("Export file fail with error :" + ex.Message);
                    throw new Exception("Export file fail with error :" + ex.Message);
                }

                SetAlert("Export file fail", "error");
                return RedirectToAction("ReportCandidates");
            }
            else
            {
                return RedirectToAction("ReportCandidates");
            }
        }

        /// <summary>
        /// Export data Report Candidates Gst to Excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcelReportCandidatesGst()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;

                var list = _reportService.GetReportCandidatesGst(model.FromDate, model.ToDate, model.Name, model.InterviewResult)
                    .Select(i => new
                    {
                        No = i.No,
                        Name = i.Name,
                        Result = (EnumResult)i.Result,
                        Date = i.Date,
                        Certificate = i.Certificate
                    }).ToList();

                try
                {
                    //export excel
                    var gv = new GridView();
                    gv.DataSource = list;
                    gv.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=ReportCandidates.xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                    gv.RenderControl(objHtmlTextWriter);

                    Response.Output.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    log.Error("Export file fail with error :" + ex.Message);
                    throw new Exception("Export file fail with error :" + ex.Message);
                }

                SetAlert("Export file fail", "error");
                return RedirectToAction("ReportCandidatesGst");
            }
            else
            {
                return RedirectToAction("ReportCandidatesGst");
            }
        }

        /// <summary>
        /// Export data Report Skill and Position to Excel file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ExportExcelReportSkillAndPosition()
        {
            if (Session["FormReportViewModel"] != null)
            {
                var model = Session["FormReportViewModel"] as FormReportViewModel;
                var list = _reportService.GetReportSkillAndPosition(model.FromDate, model.ToDate, model.Name,
                        model.Position)
                    .Select(i => new
                    {
                        No = "",
                        Name = i.Name,
                        Position = i.Position,
                        Skills = i.Skills
                    }).ToList();

                try
                {
                    //export excel
                    var gv = new GridView();
                    gv.DataSource = list;
                    gv.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=ReportSkillAndPosition.xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    gv.RenderControl(objHtmlTextWriter);

                    Response.Output.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    log.Error("Export file fail with error :" + ex.Message);
                    throw new Exception("Export file fail with error :" + ex.Message);
                }

                SetAlert("Export file fail", "error");
                return RedirectToAction("ReportSkillsandCareers");
            }
            else
            {
                return RedirectToAction("ReportSkillsandCareers");
            }
        }

        /// <summary>
        /// Export data Report Interviewers to Excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcelReportInterviewers()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;
                DateTime? FromDate = null, ToDate = null;
                int? interviewResult = null;
                DateTime dtmTimeTemp;
                int iTemp;
                if (model.Name == null)
                {
                    model.Name = "";
                }
                if (DateTime.TryParse(model.FromDate, out dtmTimeTemp))
                {
                    FromDate = dtmTimeTemp;
                }
                if (DateTime.TryParse(model.ToDate, out dtmTimeTemp))
                {
                    ToDate = dtmTimeTemp;
                }
                if (Int32.TryParse(model.InterviewResult, out iTemp))
                {
                    if (iTemp == 0 || iTemp == 1 || iTemp == 2)
                        interviewResult = iTemp;
                }
                var list = _reportService.GetReportInterviewers(FromDate, ToDate, model.Name, interviewResult)
                .Select(i => new ReportInterviewers
                {
                    ID = i.ID,
                    Name = i.Name,
                    Account = i.Account,
                    NumOfCandidates = i.NumOfCandidates,
                    PassFail = i.PassFail,
                }).ToList();

                try
                {
                    //export excel
                    var gv = new GridView();
                    gv.DataSource = list;
                    gv.DataBind();

                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=ReportInterviewers.xls");
                    Response.ContentType = "application/ms-excel";

                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                    gv.RenderControl(objHtmlTextWriter);

                    Response.Output.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                }
                catch (Exception ex)
                {
                    log.Error("Export file fail with error :" + ex.Message);
                    throw new Exception("Export file fail with error :" + ex.Message);
                }

                SetAlert("Export file fail", "error");
                return RedirectToAction("ReportInterviewers");
            }
            else
            {
                return RedirectToAction("ReportInterviewers");
            }
        }

        /// <summary>
        /// Export to pdf using crystal report for skill and positon.
        /// </summary>
        public ActionResult ExportPdfSkillAndPosition()
        {
            if (Session["FormReportViewModel"] != null)
            {
                var model = Session["FormReportViewModel"] as FormReportViewModel;
                var list = _reportService.GetReportSkillAndPosition(model.FromDate, model.ToDate, model.Name,
                        model.Position)
                    .Select(i => new
                    {
                        No = "",
                        Name = i.Name,
                        Position = i.Position,
                        Skills = i.Skills
                    }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportSkillAndPosition.rpt")));
                rd.SetDataSource(list);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReportSkillsAndCareers.pdf");
            }
            else
            {
                return RedirectToAction("ReportSkillsandCareers");
            }
        }

        /// <summary>
        /// Export data Report Candidates to pdf file.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportPdfReportCandidates()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;

                var list = _reportRepositoty.GetReportCandidates(model.FromDate, model.ToDate, model.Name, model.InterviewResult)
                .Select(i => new
                {
                    No = i.No,
                    Name = i.Name,
                    Result = ((EnumResult)i.Result).ToString(),
                    Date = i.Date,
                }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportCandidate.rpt")));
                rd.SetDataSource(list);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReportCandidates.pdf");
            }
            else
            {
                return RedirectToAction("ReportCandidates");
            }
        }

        /// <summary>
        /// Export data Report Candidates Gst to pdf file
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportPdfReportCandidatesGst()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;

                var list = _reportRepositoty.GetReportCandidatesGst(model.FromDate, model.ToDate, model.Name, model.InterviewResult)
                    .Select(i => new
                    {
                        No = i.No,
                        Name = i.Name,
                        Result = ((EnumResult)i.Result).ToString(),
                        Date = i.Date,
                        Certificate = i.Certificate
                    }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportCandidateGST.rpt")));
                rd.SetDataSource(list);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReportCandidatesGST.pdf");
            }
            else
            {
                return RedirectToAction("ReportCandidates");
            }
        }

        /// <summary>
        /// Export data Report Interviewers to Excel file
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportPdfReportInterviewers()
        {
            if (Session["FormReportViewModel"] != null)
            {
                FormReportViewModel model = new FormReportViewModel();
                model = Session["FormReportViewModel"] as FormReportViewModel;
                DateTime? FromDate = null, ToDate = null;
                int? interviewResult = null;
                DateTime dtmTimeTemp;
                int iTemp;
                if (model.Name == null)
                {
                    model.Name = "";
                }
                if (DateTime.TryParse(model.FromDate, out dtmTimeTemp))
                {
                    FromDate = dtmTimeTemp;
                }
                if (DateTime.TryParse(model.ToDate, out dtmTimeTemp))
                {
                    ToDate = dtmTimeTemp;
                }
                if (Int32.TryParse(model.InterviewResult, out iTemp))
                {
                    if (iTemp == 0 || iTemp == 1 || iTemp == 2)
                        interviewResult = iTemp;
                }
                var list = _reportRepositoty.GetReportInterviewers(FromDate, ToDate, model.Name, interviewResult)
                .Select(i => new ReportInterviewers
                {
                    ID = i.ID,
                    Name = i.Name,
                    Account = i.Account,
                    NumOfCandidates = i.NumOfCandidates,
                    PassFail = i.PassFail,
                }).ToList();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reports/CrystalReportInterviewer.rpt")));
                rd.SetDataSource(list);
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "ReportInterviewer.pdf");
            }
            else
            {
                return RedirectToAction("ReportInterviewers");
            }
        }
    }
}