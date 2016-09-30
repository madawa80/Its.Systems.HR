using Its.Systems.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{


    public class SessionSummaryStatisticsController : Controller
    {

        private IActivityManager _activityManager;
        private IPersonManager _personManager;

        public SessionSummaryStatisticsController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;
        }

        //public ActionResult Details(int? year)
        //{
        //    Session session;

        //    activity = _activityManager.GetAllActivities().OrderBy(n => n.Name).FirstOrDefault();


            //var viewModel = new SessionSummaryStatisticsViewModel()
            //{
            //    Year = activity.Id,
            //    //ActivityName = activity.Name,
            //    //SessionId = session.Id,
            //    //SessionName = session.Name,
            //    //PaticipantId = participant.Id,
            //    //PaticipantName = participant.FullName,
            //    //Comments = session.Comments,
            //    //Evaluation = session.Evaluation,
            //    Sessions = new SelectList(
            //                               _activityManager.GetAllSessionsForYear(int Year).OrderBy(n => n.Name),
            //                               "Id",
            //                               "Name",
            //                               _activityManager.GetAllActivities().OrderBy(n => n.Name).First().Id),

            //    //Sessions = new SelectList(
                //                            allSessionsForActivityResult,
                //                            "Id",
                //                            "Name",
                //                            session.Id),

                //SessionParticipants = _activityManager.GetAllParticipantsForSession(session.Id).OrderBy(n => n.FirstName),

            //};



            //return View();

        }

    }

//}