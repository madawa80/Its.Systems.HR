using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;

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

        public ActionResult Index()
        {
            return View(_personManager.GetAllParticipants().OrderBy(n => n.FirstName).ToList());
        }

        public ActionResult Details(int? id, string error)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
                return HttpNotFound();

            var allSessions = _sessionManager.GetAllSessions().Include(n => n.Activity).OrderBy(n => n.Activity.Name).ThenBy(n => n.Name).ToList();


            var yearslist = _sessionManager.GetAllSessionsForParticipantById(participant.Id)
                .Include(n => n.Activity)
                .OrderBy(n => n.StartDate.Value.Year)
                .Where(n => n.StartDate != null)
                .Select(n => n.StartDate.Value.Year).Distinct();

            var yearslisting = from element in yearslist
                         orderby element descending
                         select element;

            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullNameWithCas,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Years = yearslisting.ToList(),
                Sessions = _sessionManager.GetAllSessionsForParticipantById(participant.Id)
                            .Include(n => n.Activity)
                            .OrderBy(n => n.StartDate)
                            .ToList(),
                AllSessions = new SelectList(
                                            allSessions,
                                            "Id",
                                            "NameWithActivity",
                                            allSessions.First().Id)
            };

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }

            return View(viewModel);
        }

        public ActionResult ReviewSession(int? id = 44)
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
                CountThisYear = allParticipantSessions.Count(n => n.StartDate != null && n.StartDate.Value.Year == DateTime.Now.Year) //TODO: null error
            };

            return PartialView("_ParticipantStatisticSummaryPartial", result);
        }
    }
}
