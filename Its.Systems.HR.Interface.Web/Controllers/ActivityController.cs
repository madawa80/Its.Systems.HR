using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Infrastructure.Repository;
using Its.Systems.HR.Interface.Web.ViewModels;
//using Quartz;
//using Quartz.Impl;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityManager _activityManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilitiesManager;

        public ActivityController(IActivityManager activityManager, IPersonManager personManager, IUtilityManager utilityManager)
        {
            _activityManager = activityManager;
            _personManager = personManager;
            _utilitiesManager = utilityManager;
        }

       
        public ActionResult SyncUsersWithUmuApi()
        {
            return View("Error");



            _personManager.AddItsPersonsToDb();
            return RedirectToAction("Index");
        }

        // find Activity 
        public ViewResult Index(string sortOrder, string searchString)
        {
            //ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.CurrentSearchString = searchString;


            var activities = _activityManager.GetAllActivities();

            if (!string.IsNullOrEmpty(searchString))
            {
                activities = activities.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    activities = activities.OrderByDescending(s => s.Name);
                    break;
                default:  // Name ascending 
                    activities = activities.OrderBy(s => s.Name);
                    break;
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
        public ActionResult CreateActivity(ActivityViewModel activity)
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

                        return RedirectToAction("AllSessionsForActivity", "ActivitySummary", new { id = result.Id });
                    }
                    else
                    {
                        ModelState.AddModelError("", "En aktivitet med samma namn existerar redan.");
                        return View(activity);
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
            if (activity == null)
            {
                return HttpNotFound();
            }
            var result = new ActivityViewModel { Name = activity.Name };
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

        [HttpPost]
        public ActionResult DeleteActivity(int activityId)
        {
            if (!_activityManager.DeleteActivityById(activityId))
                return new HttpNotFoundResult();

            var result = new { Success = "True" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //AJAX AUTOCOMPLETE
        public ActionResult AutoCompleteLocations(string term)
        {
            var locations = GetLocations(term);
            return Json(locations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCompleteParticipants(string term)
        {
            var participants = GetParticipants(term);
            return Json(participants, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCompleteTags(string term)
        {
            var tags = GetTags(term);
            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        // PRIVATE METHODS BELOW
        private IEnumerable<string> GetLocations(string searchString)
        {
            IEnumerable<string> locations =
                _utilitiesManager.GetAllLocations()
                    .Where(n => n.Name.ToUpper().Contains(searchString.ToUpper()))
                    .Select(a => a.Name);

            return locations;
        }

        private IEnumerable<string> GetParticipants(string searchString)
        {
            var participants = from element in _personManager.GetAllParticipants()
                         let fullNameWithCas = element.FirstName + " " + element.LastName + " " + element.CasId
                         where fullNameWithCas.Contains(searchString.ToUpper())
                         select element.FirstName + " " + element.LastName + " (" + element.CasId + ")";

            //IEnumerable<string> participants =
            //    _personManager.GetAllParticipants()
            //    .Where(n => sb.Append(n.FirstName.ToUpper() + n.LastName.ToUpper() + n.CasId.ToUpper(). sb.Contains(searchString.ToUpper())
            //    || n.LastName.ToUpper().Contains(searchString.ToUpper())
            //    || n.CasId.ToUpper().Contains(searchString.ToUpper()))
            //    .Select(a => a.FirstName + " " + a.LastName + " (" + a.CasId + ")");

            return participants;
        }

        private IEnumerable<string> GetTags(string searchString)
        {
            IEnumerable<string> tags =
                _utilitiesManager.GetAllTags()
                    .Where(n => n.Name.ToUpper().Contains(searchString.ToUpper()))
                    .Select(a => a.Name);

            return tags;
        }
    }
}

