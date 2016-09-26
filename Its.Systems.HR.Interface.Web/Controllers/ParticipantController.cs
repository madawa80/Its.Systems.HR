using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private IActivityManager _activityManager;
        private IPersonManager _personManager;
        //private readonly IDbRepository _repository;
        //private IDbRepository _repo;

        public ParticipantController(IActivityManager activityManager, IPersonManager personManager)
        {
            _activityManager = activityManager;
            _personManager = personManager;
            //_repository = repository;
        }

        // GET: Participant
        public ActionResult Index()
        {
            return View(_personManager.GetAllParticipants().ToList());
        }

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
            {
                return HttpNotFound();
            }

            // TODO: Write better queries!
            // TODO: DONT USE try/catch for logic.......!
            IQueryable<Session> sessionsAvailable;
            int sessionsAvailableId;
            try
            {
                sessionsAvailable = _activityManager.GetAllSessions().Except(_activityManager.GetAllSessionsForParticipantById(participant.Id)).OrderBy(n => n.Name);
                sessionsAvailableId = sessionsAvailable.First().Id;
            }
            catch (Exception)
            {
                sessionsAvailable = new List<Session>().AsQueryable();
                sessionsAvailableId = 0;
                //throw;
            }
            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullName,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Sessions = _activityManager.GetAllSessionsForParticipantById(participant.Id).ToList(),
                AllSessions = new SelectList(
                                            sessionsAvailable,
                                            "Id",
                                            "Name",
                                            sessionsAvailableId)
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveComments(int personId)
        {
            // TODO: Possible security risk here!?
            if (_personManager.SaveCommentsForParticipant(personId, Request.Form["Comments"]))
                return RedirectToAction("Details", new { id = personId });

            // TODO: ErrorMessage
            return RedirectToAction("Details", new { id = personId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveWishes(int personId)
        {
            // TODO: Possible security risk here!?
            if (_personManager.SaveWishesForParticipant(personId, Request.Form["Wishes"]))
                return RedirectToAction("Details", new { id = personId });

            // TODO: ErrorMessage
            return RedirectToAction("Details", new { id = personId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddPersonToSession(int personId)
        {
            var sessionIdstring = Request.Form["Id"]; //session dropdown
            int sessionId = -1;
            int.TryParse(sessionIdstring, out sessionId);

            if (sessionId == -1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!_activityManager.AddParticipantToSession(personId, sessionId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return RedirectToAction("Details", new { id = personId });
        }

        public ActionResult RemovePersonFromSession(int sessionId, int personId)
        {
            var result = new { Success = "True" };

            //if (_personManager.GetParticipantById(personId) == null || _activityManager.GetSessionById(sessionId) == null)
            //    result = new { Success = "Fail" };

            if (!_activityManager.RemoveParticipantFromSession(personId, sessionId))
                result = new { Success = "Fail" };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
