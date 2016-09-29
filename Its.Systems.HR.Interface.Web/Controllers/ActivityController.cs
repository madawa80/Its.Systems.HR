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

        public ActionResult CreateSession()
        {
            //var viewModel = new CreateSessionViewModel()
            //{
            //    LocationList = new SelectList(_manager.GetAllLocations(), "Id", "Name", 1)
            //};
            //ViewBag.LocationId = new SelectList(_manager.GetAllLocations().OrderBy(n => n.Name), "Id", "Name", 1);
            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName", 1);
            ViewBag.ActivityId = new SelectList(_manager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", 1);
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

                    var activityName = _manager.GetActivityById(sessionVm.Activity.Id).Name;

                    int locationId = GetIdForLocationOrCreateIfNotExists(sessionVm.NameOfLocation);

                    var result = new Session()
                    {
                        Name = activityName + " " + sessionVm.Name,
                        ActivityId = sessionVm.Activity.Id,
                        StartDate = sessionVm.StartDate,
                        EndDate = sessionVm.EndDate,
                        LocationId = locationId,
                        HrPersonId = sessionVm.HrPerson.Id,
                        SessionParticipants = null,
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

                    _manager.AddSession(result);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Aktiviteten existerar redan.");
            }
            return View(sessionVm);
        }

        public ViewResult FilterSessions()
        {
            var allSessions = _manager.GetAllSessionsWithIncludes();
            

            var result = new FilterSessionsViewModel() {Sessions = new List<Session>()};

            foreach (var session in allSessions)
            {
                result.Sessions.Add(new Session()
                {
                    Id = session.Id,
                    Name = session.Name,
                    StartDate = session.StartDate,
                    EndDate = session.EndDate,
                    Location = session.Location,
                    HrPerson = session.HrPerson
                    // TODO: + Count of participants etc
                });
            }


            return View(result);
        }

        private int GetIdForLocationOrCreateIfNotExists(string location)
        {
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
    }
}

