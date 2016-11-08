using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.Helpers.Extensions;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class SessionController : Controller
    {
        private readonly IActivityManager _activityManager;
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilitiesManager;

        public SessionController(IActivityManager activityManager, ISessionManager sessionManager,
            IPersonManager personManager, IUtilityManager utilityManager)
        {
            _activityManager = activityManager;
            _sessionManager = sessionManager;
            _personManager = personManager;
            _utilitiesManager = utilityManager;
        }


        public ActionResult CreateSession(int id = 0) //activityId
        {
            var allActivities = _activityManager.GetAllActivities().OrderBy(n => n.Name).ToList();
            var allSessionParticipants = _personManager.GetAllParticipants().OrderBy(n => n.FirstName).ToList();
            var allHrPersons = _personManager.GetAllHrPersons().OrderBy(n => n.FirstName).ToList();

            var selectedActivityId =
                (id == 0) ? allActivities.First().Id : id;

            ViewBag.AllActivities = new SelectList(allActivities, "Id", "Name", selectedActivityId);
            ViewBag.AllSessionParticipants = new SelectList(
                allSessionParticipants,
                "Id",
                "FullName",
                allSessionParticipants.Skip(1).First().Id); // TODO: TAKE AWAY SKIP
            ViewBag.AllHrPersons = new SelectList(allHrPersons, "Id", "FullName");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateSession(CreateSessionViewModel sessionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<Participant> participantsToAddFromDb = new List<Participant>();
                    if (sessionVm.AddedParticipants != null)
                    {
                        string[] participants = sessionVm.AddedParticipants.Split(',');
                        List<int> participantsId = new List<int>();
                        foreach (var participant in participants)
                        {
                            participantsId.Add(int.Parse(participant));
                        }

                        //TODO: Make a proper join, this is inefficient
                        participantsToAddFromDb =
                            _personManager.GetAllParticipants().Where(n => participantsId.Contains(n.Id)).ToList();
                    }


                    // -> TAGS
                    var tagsToAdd = sessionVm.GenerateSessionTags;

                    _utilitiesManager.AddNewTagsToDb(tagsToAdd);
                    // <- END TAGS


                    int? locationId = _utilitiesManager.GetIdForLocationOrCreateIfNotExists(sessionVm.NameOfLocation);

                    var result = new Session()
                    {
                        Name = sessionVm.Name,
                        ActivityId = sessionVm.Activity.Id,
                        StartDate = sessionVm.StartDate,
                        EndDate = sessionVm.EndDate,
                        LocationId = locationId,
                        HrPersonId = sessionVm.HrPerson,
                        Description = sessionVm.Description,
                        IsOpenForExpressionOfInterest = sessionVm.IsOpenForExpressionOfInterest,
                        SessionParticipants = null,
                        SessionTags = null
                    };

                    if (sessionVm.AddedParticipants != null)
                    {
                        List<SessionParticipant> final = new List<SessionParticipant>();
                        foreach (var participant in participantsToAddFromDb)
                        {
                            final.Add(new SessionParticipant()
                            {
                                ParticipantId = participant.Id,
                                Session = result,
                                Rating = 0,
                            });
                            result.SessionParticipants = final;
                        }

                    }

                    // Save session in db
                    _sessionManager.AddSession(result);

                    // Now add tags to the created session!...
                    _sessionManager.AddSessionTags(tagsToAdd, result.Id);

                    return RedirectToAction("SessionForActivity", "ActivitySummary", new { id = result.Id });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Aktiviteten existerar redan.");
            }
            return View(sessionVm);
        }

        [HttpGet]
        public ActionResult EditSession(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var session = _sessionManager.GetSessionByIdWithIncludes((int)id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var activity = _activityManager.GetActivityById(session.ActivityId);

            // Get Tags for session
            var allTagsForSession =
                _utilitiesManager.GetAllTagsForSessionById((int)id).ToList();

            var viewModel = new EditSessionViewModel()
            {
                SessionId = session.Id,
                NameOfSessionWithActivity = session.NameWithActivity,
                Activity = activity,
                NameOfSession = session.Name,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                Description = session.Description,
                IsOpenForExpressionOfInterest = session.IsOpenForExpressionOfInterest,
                HrPerson = session.HrPersonId,
                NameOfLocation = (session.Location == null) ? string.Empty : session.Location.Name,
                AddedTags = allTagsForSession
            };

            ViewBag.NameOfLocation = viewModel.NameOfLocation;

            ViewBag.AllHrPersons = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id",
                "FullName", session.HrPersonId);
            ViewBag.AllActivities = new SelectList(_activityManager.GetAllActivities().OrderBy(n => n.Name), "Id",
                "Name", session.ActivityId);

            return View(viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("EditSession")]
        public ActionResult EditSessionPost(EditSessionViewModel inputVm)
        {
            if (inputVm == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sessionToUpdate = _sessionManager.GetSessionByIdWithIncludes(inputVm.SessionId);

            try
            {
                int? location = _utilitiesManager.GetIdForLocationOrCreateIfNotExists(inputVm.NameOfLocation);
                int? hrPerson = inputVm.HrPerson;

                sessionToUpdate.Name = inputVm.NameOfSession;
                sessionToUpdate.ActivityId = inputVm.Activity.Id;
                sessionToUpdate.StartDate = inputVm.StartDate;
                sessionToUpdate.EndDate = inputVm.EndDate;
                sessionToUpdate.Description = inputVm.Description;
                sessionToUpdate.IsOpenForExpressionOfInterest = inputVm.IsOpenForExpressionOfInterest;
                sessionToUpdate.LocationId = location;
                sessionToUpdate.HrPersonId = hrPerson;

                _sessionManager.EditSession(sessionToUpdate);

                return RedirectToAction("SessionForActivity", "ActivitySummary", new { id = sessionToUpdate.Id });

                //ModelState.AddModelError("NameOfSession", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("",
                    "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            ViewBag.AllHrPersons = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id",
                "FullName", sessionToUpdate.HrPersonId);
            ViewBag.AllActivities = new SelectList(_activityManager.GetAllActivities().OrderBy(n => n.Name), "Id",
                "Name", sessionToUpdate.ActivityId);

            ViewBag.NameOfLocation = inputVm.NameOfLocation;
            inputVm.AddedTags = _utilitiesManager.GetAllTagsForSessionById(inputVm.SessionId).ToList();
            return View("EditSession", inputVm);
        }

        [HttpPost]
        public ActionResult AddPersonToSession(int personId, int id) //id = sessionId
        {
            if (!_personManager.AddParticipantToSession(personId, id))
            {
                return RedirectToAction("Details", "Participant", new { id = personId, error = "Personen är redan tillagd på tillfället." });
            }

            return RedirectToAction("Details", "Participant", new { id = personId });
        }

        [HttpPost]
        public ActionResult AddExpressionOfInterest(int sessionId)
        {
            var loggedInUser = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());

            if (_personManager.AddExpressionOfInterest(sessionId, loggedInUser.Id))
                return RedirectToAction("SessionForActivity", "ActivitySummary", new { id = sessionId });
            
            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        public ActionResult RemoveExpressionOfInterest(int sessionId)
        {
            var loggedInUser = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());

            if (_personManager.RemoveExpressionOfInterest(sessionId, loggedInUser.Id))
                return RedirectToAction("SessionForActivity", "ActivitySummary", new { id = sessionId });

            return new HttpUnauthorizedResult();
        }

        // AJAX METHODS BELOW
        [HttpPost]
        public ActionResult RemoveExpressionOfInterestFromParticipantDetails(int sessionId, int personId)
        {
            // Check for authorization
            if (!User.IsInRole("Admin"))
            {
                var loggedInUser = _personManager.GetParticipantByCas(User.Identity.Name.ToCasId());
                if (loggedInUser.Id != personId)
                    return new HttpUnauthorizedResult();
            }
            
            if (_personManager.RemoveExpressionOfInterest(sessionId, personId))
            {
                return Json(new {Success = true});
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public ActionResult AddPersonToSessionFromActivitySummary(int sessionId, int? participantId)
        {
            //TODO: ERROR HANDLING!

            //    if (personName == "")
            //    {
            //        var failResult = new
            //        {
            //            Success = false,
            //            ErrorMessage = "Ogiltig inmatning",
            //            PersonId = 0,
            //            SessionId = sessionId,
            //            PersonFullName = "",
            //        };

            //        return Json(failResult);
            //    }
            //    else
            //    {
            //        var failResult = new
            //        {
            //            Success = false,
            //            ErrorMessage = "Välj person ur listan",
            //            PersonId = 0,
            //            SessionId = sessionId,
            //            PersonFullName = "",
            //        };

            //        return Json(failResult);
            //    }
            //}

            if (participantId == null)
            {
                var failResult = new
                {
                    Success = false,
                    ErrorMessage = "Misslyckades",
                    PersonId = 0,
                    SessionId = sessionId,
                    PersonFullName = "",
                };

                return Json(failResult);
            }

            var participant = _personManager.GetParticipantById((int)participantId);
            if (participant == null)
            {
                var failResult = new
                {
                    Success = false,
                    ErrorMessage = "Misslyckades",
                    PersonId = 0,
                    SessionId = sessionId,
                    PersonFullName = "",
                };

                return Json(failResult);
            }

            if (!_personManager.AddParticipantToSession(participant.Id, sessionId))
            {
                var failResult = new
                {
                    Success = false,
                    ErrorMessage = "Personen är redan registrerad",
                    PersonId = 0,
                    SessionId = 0,
                    PersonFullName = "",
                };

                return Json(failResult);
            }


            var succesResult = new
            {
                Success = true,
                ErrorMessage = "",
                PersonId = participant.Id,
                SessionId = sessionId,
                PersonFullName = participant.FullName,
            };

            return Json(succesResult);
        }

        public ActionResult RemovePersonFromSession(int sessionId, int personId)
        {
            var result = new { Success = true };

            if (!_personManager.RemoveParticipantFromSession(personId, sessionId))
                result = new { Success = false };

            return Json(result);
        }

        public ActionResult RemoveSession(int id)
        {
            var result = new { Success = true };

            if (!_sessionManager.DeleteSessionById(id))
                result = new { Success = false };

            return Json(result);
        }

        public ActionResult AddTagToSession(int sessionId, string tagName)
        {
            var result = new { Success = false, TagId = -1 };
            if (tagName.Length < 1)
                return Json(result);

            var tagIdFromDb = _sessionManager.AddTagToSession(sessionId, tagName);
            if (tagIdFromDb != null)
                result = new { Success = true, TagId = (int)tagIdFromDb };

            return Json(result);
        }

        public ActionResult RemoveTagFromSession(int sessionId, int tagId)
        {
            var result = new { Success = false };

            if (_sessionManager.RemoveTagFromSession(sessionId, tagId))
                result = new { Success = true };

            return Json(result);
        }
    }
}