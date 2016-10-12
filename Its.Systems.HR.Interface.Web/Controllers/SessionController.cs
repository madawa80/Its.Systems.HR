using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
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
                allSessionParticipants.First().Id);
            ViewBag.AllHrPersons = new SelectList(allHrPersons, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    AddNewTagsToDb(tagsToAdd);
                    // <- END TAGS


                    int? locationId = GetIdForLocationOrCreateIfNotExists(sessionVm.NameOfLocation);

                    var result = new Session()
                    {
                        Name = sessionVm.Name,
                        ActivityId = sessionVm.Activity.Id,
                        StartDate = sessionVm.StartDate,
                        EndDate = sessionVm.EndDate,
                        LocationId = locationId,
                        HrPersonId = sessionVm.HrPerson,
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

                    return RedirectToAction("SessionForActivity", "ActivitySummary", new {id = result.Id});
                }
            }
            catch (RetryLimitExceededException /* dex */)
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

            var session = _sessionManager.GetSessionByIdWithIncludes((int) id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var activity = _activityManager.GetActivityById(session.ActivityId);

            // Get Tags for session
            var sessionTagIdsForSession = session.SessionTags.Select(n => n.TagId);
            var allTagsForSession =
                _utilitiesManager.GetAllTags().Where(n => sessionTagIdsForSession.Contains(n.Id)).ToList();

            var viewModel = new EditSessionViewModel()
            {
                SessionId = session.Id,
                NameOfSessionWithActivity = session.NameWithActivity,
                Activity = activity,
                NameOfSession = session.Name,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                HrPerson = session.HrPersonId,
                //this.ModelControl as CreerEtablissementModel ??
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
        [ValidateAntiForgeryToken]
        [ActionName("EditSession")]
        public ActionResult EditSessionPost(EditSessionViewModel inputVm)
        {
            if (inputVm == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sessionToUpdate = _sessionManager.GetSessionByIdWithIncludes(inputVm.SessionId);

            try
            {
                if (sessionToUpdate.Name == inputVm.NameOfSession ||
                    !_sessionManager.GetAllSessions().Any(n => n.Name == inputVm.NameOfSession))
                {
                    int? location = GetIdForLocationOrCreateIfNotExists(inputVm.NameOfLocation);

                    int? hrPerson = inputVm.HrPerson;

                    sessionToUpdate.Name = inputVm.NameOfSession;
                    sessionToUpdate.ActivityId = inputVm.Activity.Id;
                    sessionToUpdate.StartDate = inputVm.StartDate;
                    sessionToUpdate.EndDate = inputVm.EndDate;
                    sessionToUpdate.LocationId = location;
                    sessionToUpdate.HrPersonId = hrPerson;

                    _sessionManager.EditSession(sessionToUpdate);

                    return RedirectToAction("SessionForActivity", "ActivitySummary", new {id = sessionToUpdate.Id});
                }

                ModelState.AddModelError("NameOfSession", "Aktiviteten existerar redan.");
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

            return View("EditSession", inputVm);
        }


        // AJAX METHODS BELOW
        [HttpPost]
        public ActionResult AddPersonToSession(int sessionId, int personId)
        {
            var session = _sessionManager.GetSessionByIdWithIncludes(sessionId);

            var result = new
            {
                Success = true,
                ErrorMessage = "",
                PersonId = personId,
                SessionId = sessionId,
                SessionName = session.NameWithActivity,
                StartDate = session.StartDate.ToShortDateString(),
                //Year = session.StartDate.Year,
                //Month = session.StartDate.Month,
                //Day = session.StartDate.Day,
            };

            if (!_personManager.AddParticipantToSession(personId, sessionId))
                result = new
                {
                    Success = false,
                    ErrorMessage = "Personen är redan registrerad på tillfället.",
                    PersonId = 0,
                    SessionId = 0,
                    SessionName = "",
                    StartDate = ""
                };
            //, Year = 0, Month = 0, Day = 0
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddPersonToSessionFromActivitySummary(int sessionId, string personName)
        {
            string personCasLogin;
            try
            {
                var firstParanthesis = personName.IndexOf('(') + 1;
                var lastParanthesis = personName.IndexOf(')');
                personCasLogin = personName.Substring(firstParanthesis, lastParanthesis - firstParanthesis);
            }
            catch (Exception)
            {
                
                throw;
            }
            
            var participant = _personManager.GetParticipantByCas(personCasLogin);
            //TODO:
            if (participant == null)
            {}//ERROR


            var result = new
            {
                Success = true,
                ErrorMessage = "",
                PersonId = participant.Id,
                SessionId = sessionId,
                PersonFullName = participant.FullName + " (" + personCasLogin + ")",
            };

            if (!_personManager.AddParticipantToSession(participant.Id, sessionId))
                result = new
                {
                    Success = false,
                    ErrorMessage = "Personen är redan registrerad på tillfället.",
                    PersonId = 0,
                    SessionId = 0,
                    PersonFullName = "",
                };

            return Json(result);
        }

        public ActionResult RemovePersonFromSession(int sessionId, int personId)
        {
            var result = new { Success = true };

            //if (_personManager.GetParticipantById(personId) == null || _activityManager.GetSessionById(sessionId) == null)
            //    result = new { Success = "Fail" };

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
            if (tagIdFromDb != -1)
                result = new { Success = true, TagId = tagIdFromDb };

            return Json(result);
        }

        public ActionResult RemoveTagFromSession(int sessionId, int tagId)
        {
            var result = new { Success = false };

            if (_sessionManager.RemoveTagFromSession(sessionId, tagId))
                result = new { Success = true };

            return Json(result);
        }


        // PRIVATE METHODS BELOW
        private int? GetIdForLocationOrCreateIfNotExists(string location)
        {
            if (string.IsNullOrEmpty(location))
                return null;

            // TODO MOVE TO MANAGER!
            int resultId = -1;

            Location locationExisting =
                _utilitiesManager.GetAllLocations().SingleOrDefault(n => n.Name.ToLower() == location.ToLower());

            if (locationExisting == null)
            {
                resultId = _utilitiesManager.AddLocation(location);
            }
            else
            {
                resultId = locationExisting.Id;
            }

            return resultId;
        }

        private void AddNewTagsToDb(List<Tag> tagsToAdd)
        {
            // tagsToAdd is the incoming stuff, with all the tags to add to Tags in DB
            // but the list needs to be filtered for any existing tags in db.Tags!!
            var tagsToAddToDb = new List<Tag>(tagsToAdd);
            var currentTags = _utilitiesManager.GetAllTags().ToList();


            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
            {
                result.Add(tag);
            }

            _utilitiesManager.AddTags(result);


            // NOTICE! Have to savechanges later!
        }
    }
}