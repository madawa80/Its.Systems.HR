using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityManager _activityManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilitiesManager;

        public ActivityController(IActivityManager activityManager, IPersonManager personManager,
            IUtilityManager utilityManager)
        {
            _activityManager = activityManager;
            _personManager = personManager;
            _utilitiesManager = utilityManager;
        }
       

        [Authorize(Roles = "Admin")]
        public ViewResult Index(string searchString)
        {
            var activities = _activityManager.GetAllActivitiesWithSessions();

            if (!string.IsNullOrEmpty(searchString))
                activities = activities.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));

            var result = new IndexActivityViewModel() {Activities = new List<ActivityWithCountOfSessions>()};

            foreach (var activity in activities)
                result.Activities.Add(new ActivityWithCountOfSessions
                {
                    ActivityId = activity.Id,
                    Name = activity.Name,
                    SessionCount = activity.Sessions.Count
                });

            return View(result);
        }

        [AllowAnonymous]
        public ViewResult StartScreen()
        {
            return View("StartScreen");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateActivity()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateActivity(ActivityViewModel activity)
        {
            var result = new Activity()
            {
                Name = activity.Name,
            };

            var activityWasInserted = _activityManager.AddActivity(result);
            if (activityWasInserted)
            {
                return RedirectToAction("AllSessionsForActivity", "ActivitySummary", new { id = result.Id });
            }
            else
            {
                ModelState.AddModelError("", "En aktivitet med samma namn existerar redan.");
                return View(activity);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var activity = _activityManager.GetActivityById(id.Value);
            if (activity == null)
                return HttpNotFound();
            var result = new ActivityViewModel {Name = activity.Name};
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("EditActivity")]
        public ActionResult EditActivityPost(ActivityViewModel activityFromInput)
        {
            if (activityFromInput == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                if (_activityManager.EditActivity(activityFromInput.Id, activityFromInput.Name))
                    return RedirectToAction("Index");

                ModelState.AddModelError("Name", "Aktiviteten existerar redan.");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("",
                    "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
            }

            return View("EditActivity", new ActivityViewModel
            {
                Name = activityFromInput.Name
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteActivity(int activityId)
        {
            if (!_activityManager.DeleteActivityById(activityId))
                return new HttpNotFoundResult();

            var result = new {Success = "True"};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SyncUsersWithUmuApi()
        {
            //return View("Error");


            _personManager.AddItsPersons();
            _personManager.InactivateItsPersons();

            _personManager.UpdateEmail();

            return RedirectToAction("Index");
        }


        //AJAX AUTOCOMPLETE
        public ActionResult AutoCompleteLocations(string term)
        {
            var locations = GetLocations(term);
            return Json(locations.OrderBy(n => n), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCompleteLocationsParticipants(string term)
        {
            var participants = from element in _personManager.GetAllParticipants()
                               let fullName = element.FirstName + " " + element.LastName
                               where fullName.Contains(term.ToUpper())
                               select new { id = element.Id, label = element.FirstName + " " + element.LastName };

            return Json(participants.OrderBy(n => n.label), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoCompleteTags(string term)
        {
            var tags = GetTags(term);
            return Json(tags.OrderBy(n => n), JsonRequestBehavior.AllowGet);
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