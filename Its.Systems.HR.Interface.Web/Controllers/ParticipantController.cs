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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = _personManager.GetAllParticipants().SingleOrDefault(n => n.Id == id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            var viewModel = new ParticipantSummaryViewModel()
            {
                PersonId = participant.Id,
                FullName = participant.FullName,
                Comments = participant.Comments,
                Wishes = participant.Wishes,
                Sessions = _activityManager.GetAllSessionsForParticipantById(participant.Id).ToList(),
                AllSessions = new SelectList(
                                            _activityManager.GetAllSessions().OrderBy(n => n.Name),
                                            "Id",
                                            "Name",
                                            _activityManager.GetAllSessions().OrderBy(n => n.Name).First().Id)
            };

            return View(viewModel);
        }

        public ActionResult SaveComments(int personId)
        {
            // TODO: Possible security risk here!?
            if (_personManager.SaveCommentsForParticipant(personId, Request.Form["Comments"]))
                return RedirectToAction("Details", new { id = personId });

            // TODO: ErrorMessage
            return RedirectToAction("Details", new {id = personId });
        }

    }
}
