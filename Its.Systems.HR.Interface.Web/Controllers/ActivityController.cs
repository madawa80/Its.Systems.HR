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

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {


        private IActivityManager _manager ;
        private IDbRepository _repo;

        public ActivityController(IActivityManager manager)
        {
            _manager = manager;
        }

        // GET: Activity information from database
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
           
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            return View(activities.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             var activity  = _manager.GetAllActivities().Where(s=>s.Id==id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Create activity
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity([Bind(Include = "Name")]ViewModels.CreateActivityViewModel activity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var result = new Activity()
                   {
                       Name = activity.Name,
                   };
                    _manager.SaveActivities(result);
                    
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
               
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(activity);
        }

        //Edit an activity

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activityToUpdate = _manager.GetAllActivities().Where(s => s.Id == id);
            if (TryUpdateModel(activityToUpdate, "",
               new string[] { "Name" }))
            {
                try
                {
                  
                    _repo.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
             
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(activityToUpdate);
        }
    }
}

