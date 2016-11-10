using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilityManager;

        public AdminController(IPersonManager personManager, IUtilityManager utilityManager)
        {
            _personManager = personManager;
            _utilityManager = utilityManager;
        }


        public ActionResult Index(string searchString, bool includeDeleted = false)
        {
            var allParticipants = _personManager.GetAllParticipantsIncludingDeleted()
                .Include(n => n.SessionParticipants);

            if (!string.IsNullOrEmpty(searchString))
                allParticipants = allParticipants.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) ||
                                                                s.LastName.ToLower().Contains(searchString.ToLower()));
            // "Deleted" participants is by default hidden.
            if (!includeDeleted)
                allParticipants = allParticipants.Except(allParticipants.Where(n => n.IsDeleted));

            var result = new IndexParticipantViewModel()
            {
                Participants = new List<ParticipantWithCountOfSessions>(),
                IncludeDeleted = includeDeleted
            };

            foreach (var participant in allParticipants)
            {
                result.Participants.Add(new ParticipantWithCountOfSessions()
                {
                    ParticipantId = participant.Id,
                    FullName = participant.FullName,
                    Email = participant.Email,
                    CountOfSessions = participant.SessionParticipants.Count,
                    IsHrPerson = participant.IsHrPerson,
                    IsActiv = participant.IsActive,
                    IsDeleted = participant.IsDeleted
                });
            }

            return View(result);
        }

        public ActionResult Tags()
        {
            var allTags = _utilityManager.GetAllTags().Include(n => n.SessionTags).ToList();

            var viewmodel = new AdminTagsViewModel()
            {
                Tags = allTags
            };

            return View(viewmodel);
        }


        // AJAX METHODS BELOW
        public ActionResult UpdatePersonalHrStatus(int ParticipantId, bool isChecked)
        {
            var result = new { Success = false };

            if (_personManager.ChangeParticipantHrStatus(ParticipantId, isChecked))
                result = new { Success = true };

            return Json(result);
        }
        
        public ActionResult ChangePersonalDeletedStatus(int Participantid, bool isChecked)
        {
            var result = new { Success = false };

            if (_personManager.ChangeParticipantDeletedStatus(Participantid, isChecked))
                result = new { Success = true };

            return Json(result);
        }

        public ActionResult DeleteTag(int tagId)
        {
            var result = new { Success = false };

            if (_utilityManager.DeleteTag(tagId))
                result = new { Success = true };

            return Json(result);
        }

    }
    
}