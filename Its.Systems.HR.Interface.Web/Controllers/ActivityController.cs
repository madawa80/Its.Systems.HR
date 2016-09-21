using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Infrastructure;
using Its.Systems.HR.Interface.Web.Models;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivityController : Controller
    {


        private HRContext db = new HRContext();

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

            var activities = from s in db.Activities
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                activities = db.Activities.Where(s => s.Name.Contains(searchString));
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
             student = db.Students.Find(id);
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

