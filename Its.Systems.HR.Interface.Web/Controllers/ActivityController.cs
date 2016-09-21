using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Interface.Web.Models;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {


        private IActivityManager _manager ;

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
             Activity  = _manager.GetAllActivities();
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Activity
        public ActionResult Index()
        {
            return View();
        }
    }
}

