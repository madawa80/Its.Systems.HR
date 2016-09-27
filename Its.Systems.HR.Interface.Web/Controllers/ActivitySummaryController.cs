using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Interface.Web.ViewModels;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

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

     

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Activity activity = _manager.GetAllActivities().SingleOrDefault(n => n.Id == id);
            Session session = _manager.GetAllSessions().SingleOrDefault(n => n.Id == id);
            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);

            if (activity == null || session== null || participant == null)
            {
                return HttpNotFound();
            }

            var allSessionsForActivityResult = new List<Session>();
            var allSessionsForActivity = _manager.GetAllSessionsForActivity(activity.Id).OrderBy(n => n.Name).ToList();

            if (allSessionsForActivity.Count != 0)
                allSessionsForActivityResult = allSessionsForActivity;

            var viewModel = new ActivitySummaryViewModel()
            {
                ActivityId = activity.Id,
                ActivityName = activity.Name,
                SessionId = session.Id,
                SessionName = session.Name,
                PaticipantId = participant.Id,
                PaticipantName = participant.FullName,
                Comments = session.Comments,
                Evaluation = session.Evaluation,
                Activities= new SelectList(
                                            _manager.GetAllActivities().OrderBy(n => n.Name),
                                            "Id",
                                            "Name",
                                            _manager.GetAllActivities().OrderBy(n => n.Name).First().Id),

                Sessions = new SelectList(
                                            allSessionsForActivityResult,
                                            "Id",
                                            "Name",
                                            2),

                SessionParticipants = _manager.GetAllParticipantsForSession(2).OrderBy(n => n.FirstName),

            };

            return View(viewModel);
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


        public ActionResult SaveComments(int sessionId)
        {
            // TODO: Possible security risk here!?
            if (_manager.SaveCommentsForSession(sessionId, Request.Form["Comments"]))
                return RedirectToAction("CreateActivitySummary", new { id = sessionId });

            // TODO: ErrorMessage
            return RedirectToAction("CreateActivitySummary", new { id = sessionId});
        }
        public ActionResult SaveEvaluation(int sessionId)
        {
            // TODO: Possible security risk here!?
            if (_manager.SaveEvaluationForSession(sessionId, Request.Form["Comments"]))
                return RedirectToAction("CreateActivitySummary", new { id = sessionId });

            // TODO: ErrorMessage
            return RedirectToAction("CreateActivitySummary", new { id = sessionId });
        }
    }
}
