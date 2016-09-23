using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Interface.Web.ViewModels;
using Its.Systems.HR.Domain.Interfaces;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivitySummaryController : Controller
    {
        private IActivityManager _manager;
        private IPersonManager _personManager;
   
        public ActivitySummaryController(IActivityManager manager, IPersonManager personManager)
        {
            _manager = manager;
            _personManager = personManager;
        
        }


        public ActionResult GetActivities()
        {
            var activities = _manager.GetAllActivities();

            var result = new List<ActivitySummaryViewModel>();

            foreach (var activity in activities)
            {
                result.Add(new ActivitySummaryViewModel()
                {
                    ActivityName = activity.Name
                });
            }


            return View(result);
        }

        public ActionResult GetSessions([Bind(Include = "ActivityId")]ViewModels.ActivitySummaryViewModel sumsessions)
        {
            var sessions = _manager.GetAllSessionsForActivity(sumsessions.ActivityId);

            var result = new List<ActivitySummaryViewModel>();

            foreach (var session in sessions)
            {
                result.Add(new ActivitySummaryViewModel()
                {
                    SessionName = session.Name
                });
            }


            return View(result);
        }


        public ActionResult GetParticipants([Bind(Include = "SessionId")]ViewModels.ActivitySummaryViewModel sumparticipants)
        {
            var participants = _manager.GetAllParticipantsForSession(sumparticipants.SessionId);

            var result = new List<ActivitySummaryViewModel>();

            foreach (var participant in participants)
            {
                result.Add(new ActivitySummaryViewModel()
                {
                    PaticipantName = participant.FullName
                });
            }


            return View(result);
        }


        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Radera misslyckades. Försök igen, och om problemet kvarstår se systemadministratören .";
            }
            var paticipant = _personManager.GetParticipantById(id.Value);
            var result = new ActivitySummaryViewModel();
            var fullname = paticipant.GetParticipantFullName();
            fullname = result.PaticipantName;
            if (paticipant == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var paticipant = _personManager.GetParticipantById(id);
                _personManager.DeletePaticipantById(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
           return RedirectToAction("Index");
        }


        //////public ActionResult Details(int id)
        //////{
        //////    if (id == null)
        //////    {
        //////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //////    }
        //////    var paticipant = _personManager.GetParticipantById(id);
        //////    if (paticipant  == null)
        //////    {
        //////        return HttpNotFound();
        //////    }
        //////    return View(paticipant);
        //////}

    }
}
