using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.Helpers.Extensions;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivitySummaryController : Controller
    {
        private const int MinYear = 2011;
        private readonly IActivityManager _activityManager;
        private readonly IPersonManager _personManager;
        private readonly ISessionManager _sessionManager;
        private readonly IUtilityManager _utilityManager;

        public ActivitySummaryController(IActivityManager manager, ISessionManager sessionManager,
            IPersonManager personManager, IUtilityManager utilityManager)
        {
            _activityManager = manager;
            _sessionManager = sessionManager;
            _personManager = personManager;
            _utilityManager = utilityManager;
        }


        public ActionResult SessionForActivity(int id)
        {
            var theSession = _sessionManager.GetSessionByIdWithIncludes(id);

            if (theSession == null)
                return View("Error");

            var allParticipant =
                _personManager.GetAllParticipantsForSession(id).OrderBy(n => n.FirstName).ToList();
            var allTagsForSession =
                _utilityManager.GetAllTagsForSessionById(id).ToList();
            var sessionRating =
                _utilityManager.GetRatingForSessionById(id);
            var loggedInUser =
                _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());
            bool userHasExpressedInterest = _personManager.GetASessionParticipant(theSession.Id, loggedInUser.Id) != null;

            var rawReviews = theSession.SessionParticipants.Where(n => n.Rating != 0);
            var reviews = new List<Review>();
            foreach (var rawReview in rawReviews)
            {
                reviews.Add(new Review()
                {
                    Rating = rawReview.Rating,
                    Name = _personManager.GetParticipantById(rawReview.ParticipantId).FullName,
                    Comments = rawReview.Comments
                });
            }

            var result = new SessionForActivityViewModel
            {
                Comments = theSession.Comments,
                Evaluation = theSession.Evaluation,
                StartDate = theSession.StartDate,
                EndDate = theSession.EndDate,
                Description = theSession.Description,
                IsOpenForExpressionOfInterest = theSession.IsOpenForExpressionOfInterest,
                HrPerson = theSession.HrPerson,
                Location = theSession.Location,
                TotalPaticipants = allParticipant.Count,
                Participants = allParticipant,
                SessionNameWithActivity = theSession.NameWithActivity,
                SessionId = id,
                ActivityName = theSession.Activity.Name,
                ActivityId = theSession.ActivityId,
                Tags = allTagsForSession,
                Rating = sessionRating.ToString(CultureInfo.CreateSpecificCulture("en-US")),
                Reviews = reviews,
                UserHasExpressedInterest = userHasExpressedInterest
            };

            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public ViewResult FilterSessions(string searchString, string yearSlider, string hrPerson, string nameOfLocation)
        {
            IQueryable<Session> allSessions;
            if (string.IsNullOrEmpty(searchString))
                allSessions = _sessionManager.GetAllSessionsWithIncludes();
            else
                allSessions =
                    _sessionManager.GetAllSessionsWithIncludes()
                        .Where(n => n.Name.Contains(searchString) || n.Activity.Name.Contains(searchString));
            // TODO: Take 10?


            // If yearSlider is min & max, then dont sort by startdate!
            var yearStart = 0;
            var yearEnd = 10000;
            if (!string.IsNullOrEmpty(yearSlider))
            {
                var years = yearSlider.Split(',');
                yearStart = int.Parse(years[0]);
                yearEnd = int.Parse(years[1]);

                if ((yearStart != MinYear) || (yearEnd != DateTime.Now.AddYears(1).Year))
                    allSessions =
                        allSessions.Where(
                            n => (n.StartDate.Value.Year >= yearStart) && (n.StartDate.Value.Year <= yearEnd));
            }

            if (!string.IsNullOrEmpty(hrPerson))
            {
                var hrPersonAsInt = int.Parse(hrPerson);
                allSessions = allSessions.Where(n => n.HrPersonId == hrPersonAsInt); //TODO error handling
            }

            if (!string.IsNullOrEmpty(nameOfLocation))
                allSessions = allSessions.Where(n => n.Location.Name == nameOfLocation);


            var allHrPersons = _personManager.GetAllHrPersons().OrderBy(n => n.FirstName).ToList();
            var result = new FilterSessionsViewModel
            {
                Sessions = allSessions.ToList(),
                HrPersons = new SelectList(allHrPersons, "Id", "FullName"),
                NameOfLocation = nameOfLocation,
                YearStart = yearStart,
                YearEnd = yearEnd,
                MinYear = MinYear,
                MaxYear = DateTime.Now.AddYears(1).Year // Maximum year set to this year + 1
            };

            return View(result);
        }

        public ViewResult FilterUpcomingSessions()
        {
            var upcomingSessions = new List<Session>();
            if (User.IsInRole("Admin"))
            {
                upcomingSessions = _sessionManager.GetAllSessionsWithIncludes()
                                    .Where(n => n.StartDate > DateTime.Now)
                                    .ToList();
            }
            else
            {
                upcomingSessions = _sessionManager.GetAllSessionsWithIncludes()
                                    .Where(n => n.StartDate > DateTime.Now && n.IsOpenForExpressionOfInterest)
                                    .ToList();
            }

            var result = new FilterSessionsViewModel
            {
                Sessions = upcomingSessions
            };

            return View(result);
        }

        public ActionResult AllSessionsForActivity(int id)
        {
            var allSessionsForActivity = _sessionManager
                .GetAllSessionsWithIncludes()
                .Where(n => n.ActivityId == id)
                .ToList();
            var activityName = _activityManager.GetActivityById(id).Name;

            var viewModel = new AllSessionsForActivityViewModel
            {
                ActivityId = id,
                ActivityName = activityName,
                Sessions = allSessionsForActivity
            };
            return View(viewModel);
        }

        // AJAX METHODS BELOW
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveSessionComments(int sessionId, string comments)
        {
            var result = new { Success = true };
            if (_sessionManager.SaveCommentsForSession(sessionId, comments))
                return Json(result);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SaveSessionEvaluation(int sessionId, string evaluation)
        {
            var result = new { Success = true };

            if (_sessionManager.SaveEvaluationForSession(sessionId, evaluation))
                return Json(result);

            result = new { Success = false };
            return Json(result);
        }

        public int SessionStatisticCount(int id)
        {
            var participantCount = _personManager.GetAllParticipantsForSession(id).Count();

            return participantCount;
        }
    }
}