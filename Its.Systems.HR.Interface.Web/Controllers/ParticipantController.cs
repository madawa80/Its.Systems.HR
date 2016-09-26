﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private IActivityManager _activityManager;
        private IPersonManager _personManager;
        //private readonly IDbRepository _repository;
        //private IDbRepository _repo;

        public ParticipantController(IActivityManager activityManager, IPersonManager personManager)
        {
            _activityManager = activityManager;
            _personManager = personManager;
            //_repository = repository;
        }

        // GET: Participant
        public ActionResult Index()
        {
            return View(_personManager.GetAllParticipants().ToList());
        }

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
                return HttpNotFound();

            var allSessions = _activityManager.GetAllSessions().OrderBy(n => n.Name);

            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullName,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Sessions = _activityManager.GetAllSessionsForParticipantById(participant.Id).ToList(),
                AllSessions = new SelectList(
                                            allSessions,
                                            "Id",
                                            "Name",
                                            allSessions.First().Id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveComments(int personId, string comments)
        {
            var result = new { Success = "True" };

            if (_personManager.SaveCommentsForParticipant(personId, comments))
                return Json(result, JsonRequestBehavior.AllowGet);

            // TODO: ErrorMessage
            result = new { Success = "False" };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Details", new { id = personId });
        }

        [HttpPost]
        public ActionResult SaveWishes(int personId, string wishes)
        {
            var result = new { Success = "True" };

            if (_personManager.SaveWishesForParticipant(personId, wishes))
                return Json(result, JsonRequestBehavior.AllowGet);

            result = new { Success = "False" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPersonToSession(int sessionId, int personId)
        {
            var result = new { Success = true, ErrorMessage = ""};

            if (!_activityManager.AddParticipantToSession(personId, sessionId))
                result = new { Success = false, ErrorMessage = "Personen är redan registrerad på kurstillfället."};

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemovePersonFromSession(int sessionId, int personId)
        {
            var result = new { Success = "True" };

            //if (_personManager.GetParticipantById(personId) == null || _activityManager.GetSessionById(sessionId) == null)
            //    result = new { Success = "Fail" };

            if (!_activityManager.RemoveParticipantFromSession(personId, sessionId))
                result = new { Success = "False" };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
