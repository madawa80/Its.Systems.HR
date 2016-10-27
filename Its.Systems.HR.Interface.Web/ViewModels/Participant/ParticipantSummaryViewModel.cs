using System.Collections.Generic;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ParticipantSummaryViewModel
    {
        public int PersonId { get; set; }

        public string FullName { get; set; }

        public string Comments { get; set; }

        public string Wishes { get; set; }

        public List<Session> Sessions { get; set; }

        public Session Session { get; set; }

        public IEnumerable<SelectListItem> AllSessions { get; set; }

        public List<int> Years { get; set; }

        //public List<ParticipantSessionRow> ParticipantSessionRows { get; set; }
    }
}