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
        private IActivityManager _activityManager;
        private IPersonManager _personManager;
   
        public ActivitySummaryController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;
        
        }

     

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
          
            Activity activity;
            if (id != null)
            {
                activity = _activityManager.GetAllActivities().SingleOrDefault(n => n.Id == id);
            }

            else
            {
                activity = _activityManager.GetAllActivities().OrderBy(n=>n.Name).FirstOrDefault();
            }

            if (activity == null)
            {
                return HttpNotFound();
            }

            //var allSessionsForActivityResult = new List<Session>();
            //var allSessionsForActivity = _activityManager.GetAllSessionsForActivity(activity.Id).OrderBy(n => n.Name).ToList();

            //if (allSessionsForActivity.Count != 0)
            //    allSessionsForActivityResult = allSessionsForActivity;

            var viewModel = new ActivitySummaryViewModel()
            {
                //ActivityId = activity.Id,
                //ActivityName = activity.Name,
                //SessionId = session.Id,
                //SessionName = session.Name,
                //PaticipantId = participant.Id,
                //PaticipantName = participant.FullName,
                //Comments = session.Comments,
                //Evaluation = session.Evaluation,
                Activities= new SelectList(
                                            _activityManager.GetAllActivities().OrderBy(n => n.Name),
                                            "Id",
                                            "Name",
                                            _activityManager.GetAllActivities().OrderBy(n => n.Name).First().Id),

                //Sessions = new SelectList(
                //                            allSessionsForActivityResult,
                //                            "Id",
                //                            "Name",
                //                            session.Id),

                //SessionParticipants = _activityManager.GetAllParticipantsForSession(session.Id).OrderBy(n => n.FirstName),

            };

            return View(viewModel);
        }

        public ActionResult GetSessions(int activityId)
        {

            var allSessionsForActivityResult = new List<Session>();
            var allSessionsForActivity = _activityManager.GetAllSessionsForActivity(activityId).OrderBy(n => n.Name).ToList();

            SelectList obgsessions;
            if (allSessionsForActivity.Count != 0)
            {

                List<Session> sessions = new List<Session>();
                sessions = _activityManager.GetAllSessionsForActivity(activityId).ToList();
                obgsessions = new SelectList(sessions, "Id", "Name", 0);
                
            }
            else
            {
                obgsessions = new SelectList(new List<SelectListItem>(), "Id", "Name", 0);
            }
            

            return Json(obgsessions);
        }

        public ActionResult GetParticipants(int sessionId)
        {
            var theSession = _activityManager.GetSessionById(sessionId);

            var allParticipant = _activityManager.GetAllParticipantsForSession(sessionId).ToList();
            var result = new ParticipantViewModel()
            {
                Comments = theSession.Comments,
                Evaluation = theSession.Evaluation,
                StartDate = theSession.StartDate,
                EndDate = theSession.EndDate,
                //HRPerson = _personManager.GetHRPersonById(sessionId).FullName,
                TotalPaticipants =allParticipant.Count,
                Participants = allParticipant,
                SessionName = theSession.Name,
                SessionId = sessionId,
            };

            return PartialView("_ParticipantPartial", result);
        }

      
        [HttpPost]
        public ActionResult SaveComments(int sessionId, string comments)
        {
            var result = new { Success = true };
            if (_activityManager.SaveCommentsForSession(sessionId, comments))
                return Json(result, JsonRequestBehavior.AllowGet);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveEvaluation(int sessionId, string evaluation)
        {
            var result = new { Success = true };

            if (_activityManager.SaveEvaluationForSession(sessionId, evaluation))
                return Json(result, JsonRequestBehavior.AllowGet);

            result = new { Success = false };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


       


    }
}
