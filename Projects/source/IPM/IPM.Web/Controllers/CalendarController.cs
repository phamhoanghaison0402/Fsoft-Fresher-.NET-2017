using IPM.Web.Models;
using log4net;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace IPM.Web.Controllers
{
    //[Authorize(Roles = "Interviewer,Admin,AdminInterview")]
    public class CalendarController : BaseController
    {
        const int Fisrt=1;
        protected static readonly ILog _log = LogManager.GetLogger("IPM.Web.Controllers.CalendarController.cs");
        static ExchangeService _service = null;
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the calendar events.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCalendarEvents()
        {           
            try
            {
                IAsyncResult result;
                IEnumerable<object> rows = new List<object>();
                //_service =(ExchangeService)Request.Cookies["cookService"].Value;  
                //set time out
                Action action = () =>
                {
                    _service = Session["Service"] as ExchangeService;
                    var listOutlook = GetAppointment(_service);
                    var eventList = listOutlook.Select(x => new
                    {
                        id = x.ID,
                        title = x.Title,
                        start = x.Start,
                        end = x.End,
                        location = x.Location,
                        address = x.Address,
                        body = x.Body,
                        allDay = false,
                    });                 
                    rows = eventList;
                };
                result = action.BeginInvoke(null, null);
                string message="";
                bool isResult;
                // time out 600s
                if (result.AsyncWaitHandle.WaitOne(600000))
                {
                    isResult = true;                   
                }
                else
                {
                    isResult = false;
                    message = "Method timed out";
                }
                return Json(new { data = rows, status = isResult,message=message}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Load Calendar error {0}", ex.Message));
                return Json(new { status = false,message=ex.Message }, JsonRequestBehavior.AllowGet);                     
            }                       
        }

        /// <summary>
        /// Gets the appointment.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>List Calendar</returns>
        private List<CalendarEventItem> GetAppointment(ExchangeService service)
        {
            try
            {
                DateTime startDate = DateTime.Now.AddMonths(-2);
                DateTime endDate = DateTime.Now.AddMonths(+2);
                // Initialize the calendar folder object with only the folder ID. 
                CalendarFolder calendar = CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());
                // Set the start and end time and number of appointments to retrieve.
                CalendarView cView = new CalendarView(startDate, endDate);
                // Limit the properties returned to the appointment's subject, start time, and end time.
                cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);
                // Retrieve a collection of appointments by using the calendar view.
                FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);
                List<CalendarEventItem> CalendarList = new List<CalendarEventItem>();
                PropertySet Props = new PropertySet(BasePropertySet.IdOnly);
                Props.Add(AppointmentSchema.Id);
                Props.Add(AppointmentSchema.Subject);
                Props.Add(AppointmentSchema.Start);
                Props.Add(AppointmentSchema.End);
                Props.Add(AppointmentSchema.Location);
                Props.Add(AppointmentSchema.Organizer);
                Props.Add(AppointmentSchema.TextBody);
                service.LoadPropertiesForItems(appointments.Items, Props);
                foreach (Appointment a in appointments)
                {
                    CalendarEventItem clen = new CalendarEventItem();
                    clen.ID = a.Id.UniqueId;
                    clen.Start = a.Start;
                    clen.End = a.End;
                    clen.Title = a.Subject;
                    clen.Body = a.TextBody.ToString();
                    clen.Location = a.Location;
                    clen.Address = a.Organizer.Address;
                    CalendarList.Add(clen);
                }

                return CalendarList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        /// <summary>
        /// Calendars the drop.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CalendarDrop(CalendarEventItem data)
        {
            DateTime startTime = data.Start;
            DateTime endTime = data.End;
            string idAppoinment = data.ID;
            string message;
            bool result;
            try
            {
                Appointment appointment = Appointment.Bind(_service, new ItemId(idAppoinment));
                appointment.Start = startTime;
                appointment.End = endTime;
                appointment.Location = data.Location;
                appointment.Subject = data.Title;
                appointment.Body = data.Body;
                SendInvitationsOrCancellationsMode mode = appointment.IsMeeting ?
                SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy : SendInvitationsOrCancellationsMode.SendToNone;
                appointment.Update(ConflictResolutionMode.AlwaysOverwrite, mode);
                message = "Update Success";             
                result = true;
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Update Calendar Fail {0}",ex.Message));              
                message = "Update Fail";
                result = false;
            }

            return Json(new { status = result,message=message });
        }

        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            bool result;
            try
            {
                Appointment appointment = Appointment.Bind(_service, id);
                // Delete the meeting by using the Delete method.
                appointment.Delete(DeleteMode.MoveToDeletedItems, SendCancellationsMode.SendToAllAndSaveCopy);
                // Verify that the meeting has been deleted by looking for a matching subject in the Deleted Items folder's first entry.
                ItemView itemView = new ItemView(Fisrt);
                itemView.Traversal = ItemTraversal.Shallow;
                // Note that the FindItems method results in a call to EWS
                FindItemsResults<Item> deletedItems = _service.FindItems(WellKnownFolderName.DeletedItems, itemView);
                Item deletedItem = deletedItems.First();
                Folder parentFolder = Folder.Bind(_service, deletedItem.ParentFolderId, new PropertySet(FolderSchema.DisplayName));
                result = true;
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Delete Calendar Fail {0}",ex.Message));
                result = false;
            }

            return Json(new { status = result });
        }

        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEvent(CalendarEventItem data)
        {
            string message;
            bool result;
            try
            {
                Appointment appointment = new Appointment(_service);
                appointment.Subject = data.Title;
                appointment.Body = data.Body;
                appointment.Start = data.Start;
                appointment.End = data.End;
                appointment.Location = data.Location;
                //appointment. = dataItem.Body;
                appointment.ReminderDueBy = DateTime.Now;
                // Save the appointment to your calendar.
                appointment.Save(SendInvitationsMode.SendToNone);
                message = "Add success";               
                result = true;
            }
            catch(Exception ex)
            {
                _log.Error(string.Format("Add Calendar Fail {0}",ex.Message));
                SetAlert(ex.Message,"error");
                result = false;
                message = "Add Fail";
            }

            return Json(new { status = result ,message=message});
        }
    }
}