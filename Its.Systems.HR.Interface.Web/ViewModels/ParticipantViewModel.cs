using System.Collections.Generic;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ParticipantViewModel
    {
        public string Comments { get; set; }
        public string Evaluation { get; set; }
        public List<Participant> Participants { get; set; }
    }
}