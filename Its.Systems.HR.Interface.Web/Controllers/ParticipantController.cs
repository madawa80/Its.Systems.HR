using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;
using System.Collections.Generic;
using Its.Systems.HR.Interface.Web.Helpers.Extensions;
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


        public ActionResult Index(string searchString)
        {
            var allParticipants = _personManager.GetAllParticipants()
                .Include(n => n.SessionParticipants);

            if (!string.IsNullOrEmpty(searchString))
                allParticipants = allParticipants.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) || 
                                                                s.LastName.ToLower().Contains(searchString.ToLower()));

            var result = new IndexParticipantViewModel() {Participants = new List<ParticipantWithCountOfSessions>()};

            foreach (var participant in allParticipants)
            {
                result.Participants.Add(new ParticipantWithCountOfSessions()
                {
                    ParticipantId = participant.Id,
                    FullName = participant.FullName,
                    CasID = participant.CasId,
                    CountOfSessions = participant.SessionParticipants.Count
                });
            }

            return View(result);
        }

        public ActionResult Details(int? id, string error)
        {
            bool permissionToShowParticipantDetails = false;

            if (id == null)
            {
                var loggedInUserAsParticipant = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());
                if (loggedInUserAsParticipant == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
                id = loggedInUserAsParticipant.Id;

                permissionToShowParticipantDetails = true;
            }

            if (!User.IsInRole("Admin") && !permissionToShowParticipantDetails)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


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
                FullNameWithCas = participant.FullNameWithCas,
                FullName = participant.FullName,
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
                                            allSessions.First().Id),
                ParticipantCasId = participant.CasId
            };

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }

            return View(viewModel);
        }

        public ActionResult ReviewSession(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var loggedInUser = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());
            var session = _sessionManager.GetSessionByIdWithIncludes((int)id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentSessionParticipation = session.SessionParticipants.SingleOrDefault(n => n.ParticipantId == loggedInUser.Id && n.SessionId == id);
            if (currentSessionParticipation == null)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);


            var reviewSessionViewModel = new ReviewSessionViewModel()
            {
                ParticipantId = loggedInUser.Id,
                ParticipantName = loggedInUser.FullNameWithCas,
                SessionId = session.Id,
                SessionName = session.NameWithActivity,
                Rating = currentSessionParticipation.Rating,
                Comments = currentSessionParticipation.Comments
            };

            return View(reviewSessionViewModel);

        }

        [HttpPost]
        public ActionResult ReviewSession(ReviewSessionViewModel vm)
        {
            var loggedInUser = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());

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
