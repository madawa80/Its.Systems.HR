using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;

        public ParticipantController(ISessionManager sessionManager, IPersonManager personManager)
        {
            _sessionManager = sessionManager;
            _personManager = personManager;
        }

        // GET: Participant
        public ActionResult Index()
        {
            return View(_personManager.GetAllParticipants().OrderBy(n => n.FirstName).ToList());
        }

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
                return HttpNotFound();

            var allSessions = _sessionManager.GetAllSessions().Include(n => n.Activity).OrderBy(n => n.Activity.Name).ThenBy(n => n.Name);

            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullNameWithCas,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Sessions = _sessionManager.GetAllSessionsForParticipantById(participant.Id).Include(n => n.Activity).ToList(),
                AllSessions = new SelectList(
                                            allSessions,
                                            "Id",
                                            "NameWithActivity",
                                            allSessions.First().Id)
            };

            return View(viewModel);
        }

        public ActionResult ReviewSession(int? id = 7)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var loggedInUser = _personManager.GetParticipantById(237);
            var session = _sessionManager.GetSessionByIdWithIncludes((int)id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentSessionParticipation = session.SessionParticipants.SingleOrDefault(n => n.ParticipantId == loggedInUser.Id && n.SessionId == id);
            if (currentSessionParticipation == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var reviewSessionViewModel = new ReviewSessionViewModel()
            {
                ParticipantId = loggedInUser.Id,
                ParticipantName = loggedInUser.FullNameWithCas,
                SessionId = session.Id,
                SessionName = session.Activity.Name + " " + session.Name,
                Rating = currentSessionParticipation.Rating,
                Comments = currentSessionParticipation.Comments
            };

            return View(reviewSessionViewModel);

        }

        [HttpPost]
        public ActionResult ReviewSession(ReviewSessionViewModel vm)
        {
            var loggedInUser = _personManager.GetParticipantById(237);

            if (_personManager.UpdateReviewForSessionParticipant(vm.SessionId, loggedInUser.Id, vm.Rating, vm.Comments))
                return RedirectToAction("SessionForActivity", "ActivitySummary", new { id = vm.SessionId});

            //ModelState.AddModelError("", "Något blev fel, prova gärna igen!");
            return View(vm);
        }


        // AJAX AND PARTIALS BELOW
        [HttpPost]
        public ActionResult SaveComments(int personId, string comments)
        {
            var result = new { Success = true };

            if (_personManager.SaveCommentsForParticipant(personId, comments))
                return Json(result);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result);
            //return RedirectToAction("Details", new { id = personId });
        }

        [HttpPost]
        public ActionResult SaveWishes(int personId, string wishes)
        {
            var result = new { Success = true };

            if (_personManager.SaveWishesForParticipant(personId, wishes))
                return Json(result);

            result = new { Success = false };
            return Json(result);
        }

        public ActionResult ParticipantStatisticSummary(int personid)
        {
            var allParticipantSessions = _sessionManager.GetAllSessionsForParticipantById(personid).ToList();
            var result = new ParticipantStatisticSummaryViewModel()
            {
                TotalCount = allParticipantSessions.Count,
                CountThisYear = allParticipantSessions.Count(n => n.StartDate.Year == DateTime.Now.Year)
            };

            return PartialView("_ParticipantStatisticSummaryPartial", result);
        }
    }
}
