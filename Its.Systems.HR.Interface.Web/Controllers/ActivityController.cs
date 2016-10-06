using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Infrastructure.Repository;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {


        private IActivityManager _activityManager;
        private IPersonManager _personManager;


        public ActivityController(IActivityManager activityManager, IPersonManager personManager)
        {
            _activityManager = activityManager;
            _personManager = personManager;

        }

        // find Activity 
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var activities = _activityManager.GetAllActivities();

            if (!String.IsNullOrEmpty(searchString))
            {
                activities = _activityManager.GetAllActivities().Where(s => s.Name.Contains(searchString));
            }

            var result = new List<ActivityViewModel>();

            foreach (var activity in activities)
            {
                result.Add(new ActivityViewModel()
                {
                    Id = activity.Id,
                    Name = activity.Name,
                });
            }


            return View(result);
        }



        // GET: Create activity
        public ActionResult CreateActivity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity([Bind(Include = "Name")]ViewModels.ActivityViewModel activity)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (!_activityManager.GetAllActivities().Any(n => n.Name == activity.Name))
                    {
                        var result = new Activity()
                        {
                            Name = activity.Name,
                        };

                        _activityManager.AddActivity(result);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "aktivitet existerar redan ");
                        return View();
                    }


                }
            }
            catch (RetryLimitExceededException /* dex */)
            {

                ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }
            return View(activity);
        }

        //Edit an activity
        [HttpGet]
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activity = _activityManager.GetActivityById(id.Value);
            var result = new ActivityViewModel();
            result.Name = activity.Name;
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditActivity")]
        public ActionResult EditActivityPost(ActivityViewModel activityFromInput)
        {
            if (activityFromInput == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activityToUpdate = _activityManager.GetActivityById(activityFromInput.Id);

            try
            {
                if (!_activityManager.GetAllActivities().Any(n => n.Name == activityFromInput.Name))
                {
                    activityToUpdate.Name = activityFromInput.Name;
                    _activityManager.EditActivity(activityToUpdate);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            return View("EditActivity", new ActivityViewModel()
            {
                Name = activityToUpdate.Name
            });
        }



        //public ActionResult Delete(int? id, bool? saveChangesError = false)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (saveChangesError.GetValueOrDefault())
        //    {
        //        ViewBag.ErrorMessage = "Radera misslyckades. Försök igen, och om problemet kvarstår se systemadministratören .";
        //    }
        //    var activity = _activityManager.GetActivityById(id.Value);
        //    var result = new ActivityViewModel();
        //    result.Name = activity.Name;
        //    if (activity == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var activity = _activityManager.GetActivityById(id);
        //        _activityManager.DeleteActivityById(id);
        //    }
        //    catch (RetryLimitExceededException/* dex */)
        //    {

        //        return RedirectToAction("Delete", new { id = id, saveChangesError = true });
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult DeleteActivity(int activityId)
        {
            if (!_activityManager.DeleteActivityById(activityId))
                return new HttpNotFoundResult();

            var result = new { Success = "True" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateSession(int id = 0) //activityId
        {
            //var viewModel = new CreateSessionViewModel()
            //{
            //    LocationList = new SelectList(_activityManager.GetAllLocations(), "Id", "Name", 1)
            //};
            //ViewBag.LocationId = new SelectList(_activityManager.GetAllLocations().OrderBy(n => n.Name), "Id", "Name", 1);

            var allActivities = _activityManager.GetAllActivities().OrderBy(n => n.Name).ToList();
            var allSessionParticipants = _personManager.GetAllParticipants().OrderBy(n => n.FirstName).ToList();
            var allHrPersons = _personManager.GetAllHrPersons().OrderBy(n => n.FirstName).ToList();

            var selectedActivityId =
                (id == 0) ? allActivities.First().Id : id;

            ViewBag.ActivityId = new SelectList(allActivities, "Id", "Name", selectedActivityId);
            ViewBag.SessionParticipantId = new SelectList(
                allSessionParticipants,
                "Id",
                "FullName",
                allSessionParticipants.First().Id);
            ViewBag.HrPersonId = new SelectList(allHrPersons, "Id", "FullName");
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
                        participantsToAddFromDb = _personManager.GetAllParticipants().Where(n => participantsId.Contains(n.Id)).ToList();
                    }


                    // -> TAGS
                    var tagsToAdd = sessionVm.GenerateSessionTags;

                    AddNewTagsToDb(tagsToAdd);
                    // <- END TAGS


                    var activityName = _activityManager.GetActivityById(sessionVm.Activity.Id).Name;
                    int? locationId = GetIdForLocationOrCreateIfNotExists(sessionVm.NameOfLocation);

                    var result = new Session()
                    {
                        Name = activityName + " " + sessionVm.Name,
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
                    _activityManager.AddSession(result);


                    // TODO: Now add tags to the created session!...
                    _activityManager.AddSessionTags(tagsToAdd, result.Id);


                    return RedirectToAction("Index");
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

            var session = _activityManager.GetSessionByIdWithIncludes((int)id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var activity = _activityManager.GetActivityById(session.ActivityId);

            var viewModel = new EditSessionViewModel()
            {
                SessionId = session.Id,
                Activity = activity,
                NameOfSession = session.Name,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                HrPerson = session.HrPersonId,
                //this.ModelControl as CreerEtablissementModel ??
                NameOfLocation = (session.Location == null) ? string.Empty : session.Location.Name,
            };

            ViewBag.NameOfLocation = viewModel.NameOfLocation;

            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName", session.HrPersonId);
            ViewBag.ActivityId = new SelectList(_activityManager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", session.ActivityId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditSession")]
        public ActionResult EditSessionPost(EditSessionViewModel inputVm)
        {
            if (inputVm == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sessionToUpdate = _activityManager.GetSessionByIdWithIncludes(inputVm.SessionId);

            try
            {
                if (sessionToUpdate.Name == inputVm.NameOfSession || !_activityManager.GetAllSessions().Any(n => n.Name == inputVm.NameOfSession))
                {
                    int? location = GetIdForLocationOrCreateIfNotExists(inputVm.NameOfLocation);

                    int? hrPerson = inputVm.HrPerson;

                    sessionToUpdate.Name = inputVm.NameOfSession;
                    sessionToUpdate.ActivityId = inputVm.Activity.Id;
                    sessionToUpdate.StartDate = inputVm.StartDate;
                    sessionToUpdate.EndDate = inputVm.EndDate;
                    sessionToUpdate.LocationId = location;
                    sessionToUpdate.HrPersonId = hrPerson;

                    _activityManager.EditSession(sessionToUpdate);

                    return RedirectToAction("GetSession", "ActivitySummary", new { id = sessionToUpdate.Id });
                }

                ModelState.AddModelError("NameOfSession", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName", sessionToUpdate.HrPersonId);
            ViewBag.ActivityId = new SelectList(_activityManager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", sessionToUpdate.ActivityId);

            ViewBag.NameOfLocation = inputVm.NameOfLocation;

            return View("EditSession", inputVm);
        }

        [HttpPost]
        public ActionResult AddPersonToSession(int sessionId, int personId)
        {
            var session = _activityManager.GetSessionById(sessionId);

            var result = new
            {
                Success = true,
                ErrorMessage = "",
                PersonId = personId,
                SessionId = sessionId,
                SessionName = session.Name,
                StartDate = session.StartDate.ToShortDateString(),
                //Year = session.StartDate.Year,
                //Month = session.StartDate.Month,
                //Day = session.StartDate.Day,
            };

            if (!_activityManager.AddParticipantToSession(personId, sessionId))
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
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPersonToSessionFromActivitySummary(int sessionId, int personId)
        {
            var fullName = _personManager.GetParticipantById(personId).FullName;

            var result = new
            {
                Success = true,
                ErrorMessage = "",
                PersonId = personId,
                SessionId = sessionId,
                PersonFullName = fullName
            };

            if (!_activityManager.AddParticipantToSession(personId, sessionId))
                result = new
                {
                    Success = false,
                    ErrorMessage = "Personen är redan registrerad på tillfället.",
                    PersonId = 0,
                    SessionId = 0,
                    PersonFullName = "",
                };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemovePersonFromSession(int sessionId, int personId)
        {
            var result = new { Success = true };

            //if (_personManager.GetParticipantById(personId) == null || _activityManager.GetSessionById(sessionId) == null)
            //    result = new { Success = "Fail" };

            if (!_activityManager.RemoveParticipantFromSession(personId, sessionId))
                result = new { Success = false };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveSession(int id)
        {
            var result = new { Success = true };

            if (!_activityManager.DeleteSessionById(id))
                result = new { Success = false };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //AJAX AUTOCOMPLETE
        public ActionResult AutoCompleteLocations(string term)
        {
            var locations = GetLocations(term);
            return Json(locations, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<string> GetLocations(string searchString)
        {
            IEnumerable<string> locations =
                _activityManager.GetAllLocations().Where(n => n.Name.ToUpper().Contains(searchString.ToUpper())).Select(a => a.Name);

            return locations;
        }

        private int? GetIdForLocationOrCreateIfNotExists(string location)
        {
            if (string.IsNullOrEmpty(location))
                return null;

            // TODO MOVE TO MANAGER!
            int resultId = -1;

            Location locationExisting =
                _activityManager.GetAllLocations().SingleOrDefault(n => n.Name.ToLower() == location.ToLower());

            if (locationExisting == null)
            {
                resultId = _activityManager.AddLocation(location);
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
            var currentTags = _activityManager.GetAllTags().ToList();


            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
            {
                result.Add(tag);
            }

            _activityManager.AddTags(result);


            // NOTICE! Have to savechanges later!
        }

    }
}

