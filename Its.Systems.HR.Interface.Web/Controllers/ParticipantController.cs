﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;
       
        public ParticipantController(ISessionManager sessionManager, IPersonManager personManager)
        {
            _sessionManager = sessionManager;
            _personManager = personManager;
        }

        // GET: Participant
        public ActionResult Index()
        {
           return View(_personManager.GetAllParticipants().OrderBy(n => n.FirstName).ToList());
        }

        // GET: Participant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
                return HttpNotFound();

            var allSessions = _sessionManager.GetAllSessions().Include(n => n.Activity).OrderBy(n => n.Activity.Name).ThenBy(n => n.Name);

            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullName,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Sessions = _sessionManager.GetAllSessionsForParticipantById(participant.Id).Include(n => n.Activity).ToList(),
                AllSessions = new SelectList(
                                            allSessions,
                                            "Id",
                                            "NameWithActivity",
                                            allSessions.First().Id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SaveComments(int personId, string comments)
        {
            var result = new { Success = true };

            if (_personManager.SaveCommentsForParticipant(personId, comments))
                return Json(result);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result);
            //return RedirectToAction("Details", new { id = personId });
        }

        [HttpPost]
        public ActionResult SaveWishes(int personId, string wishes)
        {
            var result = new { Success = true };

            if (_personManager.SaveWishesForParticipant(personId, wishes))
                return Json(result);

            result = new { Success = false };
            return Json(result);
        }

        public ActionResult ParticipantStatisticSummary(int personid)
        {
            var allParticipantSessions = _sessionManager.GetAllSessionsForParticipantById(personid).ToList();
            var result = new ParticipantStatisticSummaryViewModel()
            {
                TotalCount = allParticipantSessions.Count,
                CountThisYear = allParticipantSessions.Count(n => n.StartDate.Year == DateTime.Now.Year)
            };

            return PartialView("_ParticipantStatisticSummaryPartial", result);
        }
    }
}
