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

        public AdminController(IPersonManager personManager)
        {
            _personManager = personManager;
        }


        public ActionResult Index(string searchString)
        {
            var allParticipants = _personManager.GetAllParticipants()
                .Include(n => n.SessionParticipants);

            if (!string.IsNullOrEmpty(searchString))
                allParticipants = allParticipants.Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()) ||
                                                                s.LastName.ToLower().Contains(searchString.ToLower()));

            var result = new IndexParticipantViewModel() { Participants = new List<ParticipantWithCountOfSessions>() };

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

    }
    
}