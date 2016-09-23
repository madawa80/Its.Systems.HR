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

        public ActionResult SaveComments(int personId)
        {
            // TODO: Possible security risk here!?
            if (_personManager.SaveCommentsForParticipant(personId, Request.Form["Comments"]))
                return RedirectToAction("Details", new { id = personId });

            // TODO: ErrorMessage
            return RedirectToAction("Details", new { id = personId });
        }

        public ActionResult SaveWishes(int personId)
        {
            // TODO: Possible security risk here!?
            if (_personManager.SaveWishesForParticipant(personId, Request.Form["Wishes"]))
                return RedirectToAction("Details", new { id = personId });

            // TODO: ErrorMessage
            return RedirectToAction("Details", new { id = personId });
        }

        public ActionResult AddPersonToSession(int personid)
        {
            var sessionId = Request.Form["Id"]; //session dropdown

            throw new NotImplementedException();
        }
    }
}
