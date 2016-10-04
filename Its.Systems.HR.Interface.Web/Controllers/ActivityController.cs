using System;
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


        private IActivityManager _manager;
        private IPersonManager _personManager;


        public ActivityController(IActivityManager manager, IPersonManager personManager)
        {
            _manager = manager;
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

            var activities = _manager.GetAllActivities();

            if (!String.IsNullOrEmpty(searchString))
            {
                activities = _manager.GetAllActivities().Where(s => s.Name.Contains(searchString));
            }

            var result = new List<ListActivitiesViewModel>();

            foreach (var activity in activities)
            {
                result.Add(new ListActivitiesViewModel()
                {
                    Id = activity.Id,
                    Name = activity.Name,
                });
            }


            return View(result);
        }



        // GET: Create activity
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")]ViewModels.ActivityViewModel activity)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (!_manager.GetAllActivities().Any(n => n.Name == activity.Name))
                    {
                        var result = new Activity()
                        {
                            Name = activity.Name,
                        };

                        _manager.AddActivity(result);

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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activity = _manager.GetActivityById(id.Value);
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
        public ActionResult EditPost(ActivityViewModel activityFromInput)
        {
            if (activityFromInput == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activityToUpdate = _manager.GetActivityById(activityFromInput.Id);

            try
            {
                if (!_manager.GetAllActivities().Any(n => n.Name == activityFromInput.Name))
                {
                    activityToUpdate.Name = activityFromInput.Name;
                    _manager.EditActivity(activityToUpdate);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            return View("Edit", new ActivityViewModel()
            {
                Name = activityToUpdate.Name
            });
        }



        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Radera misslyckades. Försök igen, och om problemet kvarstår se systemadministratören .";
            }
            var activity = _manager.GetActivityById(id.Value);
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
        public ActionResult Delete(int id)
        {
            try
            {
                var activity = _manager.GetActivityById(id);
                _manager.DeleteActivityById(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteActivity(int activityId)
        {
            if (!_manager.DeleteActivityById(activityId))
                return new HttpNotFoundResult();

            var result = new { Success = "True" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateSession(int id = 0) //activityId
        {
            //var viewModel = new CreateSessionViewModel()
            //{
            //    LocationList = new SelectList(_manager.GetAllLocations(), "Id", "Name", 1)
            //};
            //ViewBag.LocationId = new SelectList(_manager.GetAllLocations().OrderBy(n => n.Name), "Id", "Name", 1);
            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName");

            var selectedActivityId =
                (id == 0) ? _manager.GetAllActivities().OrderBy(n => n.Name).First().Id : id;
            ViewBag.ActivityId = new SelectList(_manager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", selectedActivityId);

            ViewBag.SessionParticipantId = new SelectList(
                _personManager.GetAllParticipants().OrderBy(n => n.FirstName),
                "Id",
                "FullName",
                _personManager.GetAllParticipants().OrderBy(n => n.FirstName).First().Id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSession(CreateSessionViewModel sessionVm)
        {
            var test = Request.Form["AddedParticipants"];

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


                    var activityName = _manager.GetActivityById(sessionVm.Activity.Id).Name;
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
                        Tags = null // TODO: tagsToAdd...
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
                    _manager.AddSession(result);


                    //// TODO: Now add tags to the created session!...
                    //foreach (var tag in tagsToAdd)
                    //{
                    //    db.EventTags.Add(new EventTag()
                    //    {
                    //        Tag = db.Tags.SingleOrDefault(n => n.Name == tag.Name),
                    //        EventId = result.Id
                    //    });
                    //}


                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Aktiviteten existerar redan.");
            }
            return View(sessionVm);
        }

        public ViewResult FilterSessions(string searchString, string yearSlider, string hrPerson, string nameOfLocation)
        {
            IQueryable<Session> allSessions;
            if (string.IsNullOrEmpty(searchString))
                allSessions = _manager.GetAllSessionsWithIncludes();
            else
                allSessions = _manager.GetAllSessionsWithIncludes().Where(n => n.Name.Contains(searchString));
            // TODO: Take 10?

            if (!string.IsNullOrEmpty(yearSlider))
            {
                var years = yearSlider.Split(',');
                var yearStart = int.Parse(years[0]);
                var yearEnd = int.Parse(years[1]);

                allSessions = allSessions.Where(n => n.StartDate.Year >= yearStart && n.StartDate.Year <= yearEnd);
            }

            if (!string.IsNullOrEmpty(hrPerson))
            {
                var hrPersonAsInt = int.Parse(hrPerson);
                allSessions = allSessions.Where(n => n.HrPersonId == hrPersonAsInt); //TODO error handling
            }

            if (!string.IsNullOrEmpty(nameOfLocation))
            {
                allSessions = allSessions.Where(n => n.Location.Name == nameOfLocation);
            }

            var result = new FilterSessionsViewModel() { Sessions = allSessions.ToList() };
            //foreach (var session in allSessions)
            //{
            //    result.Sessions.Add(new Session()
            //    {
            //        Id = session.Id,
            //        Name = session.Name,
            //        StartDate = session.StartDate,
            //        EndDate = session.EndDate,
            //        Location = session.Location,
            //        HrPerson = session.HrPerson
            //        
            //    });
            //}

            // TODO: first item should be empty...
            var allHrPersons = _personManager.GetAllHrPersons().OrderBy(n => n.FirstName).ToList();
            result.HrPersons = new SelectList(
                allHrPersons,
                "Id",
                "FullName");

            return View(result);
        }

        [HttpGet]
        public ActionResult EditSession(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var session = _manager.GetSessionByIdWithIncludes((int)id);
            if (session == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var activity = _manager.GetActivityById(session.ActivityId);

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
            ViewBag.ActivityId = new SelectList(_manager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", session.ActivityId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditSession")]
        public ActionResult EditSessionPost(EditSessionViewModel inputVm)
        {
            if (inputVm == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sessionToUpdate = _manager.GetSessionByIdWithIncludes(inputVm.SessionId);

            try
            {
                if (sessionToUpdate.Name == inputVm.NameOfSession || !_manager.GetAllSessions().Any(n => n.Name == inputVm.NameOfSession))
                {
                    int? location = GetIdForLocationOrCreateIfNotExists(inputVm.NameOfLocation);

                    int? hrPerson = inputVm.HrPerson;

                    sessionToUpdate.Name = inputVm.NameOfSession;
                    sessionToUpdate.ActivityId = inputVm.Activity.Id;
                    sessionToUpdate.StartDate = inputVm.StartDate;
                    sessionToUpdate.EndDate = inputVm.EndDate;
                    sessionToUpdate.LocationId = location;
                    sessionToUpdate.HrPersonId = hrPerson;

                    _manager.EditSession(sessionToUpdate);

                    return RedirectToAction("FilterSessions", "Activity");
                }

                ModelState.AddModelError("NameOfSession", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName", sessionToUpdate.HrPersonId);
            ViewBag.ActivityId = new SelectList(_manager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", sessionToUpdate.ActivityId);

            ViewBag.NameOfLocation = inputVm.NameOfLocation;

            return View("EditSession", inputVm);
        }

        private int? GetIdForLocationOrCreateIfNotExists(string location)
        {
            if (string.IsNullOrEmpty(location))
                return null;

            // TODO MOVE TO MANAGER!
            int resultId = -1;

            Location locationExisting =
                _manager.GetAllLocations().SingleOrDefault(n => n.Name.ToLower() == location.ToLower());

            if (locationExisting == null)
            {
                resultId = _manager.AddLocation(location);
            }
            else
            {
                resultId = locationExisting.Id;
            }

            return resultId;
        }

        private void AddNewTagsToDb(List<Tag> tagsToAdd)
        {
            // tagsToAdd is the incoming stuff, with all the tags to add to EventTags in DB
            // but the list needs to be filtered for any existing tags in db.Tags!!
            var tagsToAddToDb = new List<Tag>(tagsToAdd);
            var currentTags = _manager.GetAllTags().ToList();


            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
            {
                result.Add(tag);
            }

            _manager.AddTags(result);


            // NOTICE! Have to savechanges later!
        }
    }
}

