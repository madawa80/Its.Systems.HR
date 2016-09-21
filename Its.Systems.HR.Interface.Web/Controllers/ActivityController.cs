﻿using System;
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


        private IActivityManager _manager ;
        private IPersonManager _personManager;
        private IDbRepository _repo;

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
                    foreach (var sameactivity in _manager.GetAllActivities())
                    {
                        
                    }
                    var activities = _manager.GetAllActivities().Where(s => s.Name.Contains(activity.Name)).Count();

                    if (!_manager.GetAllActivities().Any(n => n.Name == activity.Name))
                    {
                        var result = new Activity()
                        {
                            Name = activity.Name,
                        };

                        _manager.SaveActivities(result);
                        
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        ViewBag.Massage = "Denna aktivitet redan har skapats";
                        return RedirectToAction("Index");
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
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var activityToUpdate = _manager.GetActivityById(id.Value);
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
             
                    ModelState.AddModelError("", "Det går inte att spara ändringarna. Försök igen, och om problemet kvarstår se systemadministratören .");
                }
            }
            return View(activityToUpdate);
        }


        //Delete an activity
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
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var activity = _manager.GetActivityById(id);
                _repo.Delete(activity);
            }
            catch (RetryLimitExceededException/* dex */)
            {
               
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateSession()
        {
            //var viewModel = new CreateSessionViewModel()
            //{
            //    LocationList = new SelectList(_manager.GetAllLocations(), "Id", "Name", 1)
            //};
            ViewBag.LocationId = new SelectList(_manager.GetAllLocations().OrderBy(n => n.Name), "Id", "Name", 1);
            ViewBag.HrPersonId = new SelectList(_personManager.GetAllHrPersons().OrderBy(n => n.FirstName), "Id", "FullName", 1);
            ViewBag.ActivityId = new SelectList(_manager.GetAllActivities().OrderBy(n => n.Name), "Id", "Name", 1);
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
                    var result = new Session()
                    {
                        Name = sessionVm.Name,
                        ActivityId = sessionVm.Activity.Id,
                        StartDate = sessionVm.StartDate,
                        EndDate = sessionVm.EndDate,
                        LocationId = sessionVm.Location.Id,
                        HrPersonId = sessionVm.HrPerson.Id,
                    };
                    _manager.AddSession(result);

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(sessionVm);
        }
    }
}

