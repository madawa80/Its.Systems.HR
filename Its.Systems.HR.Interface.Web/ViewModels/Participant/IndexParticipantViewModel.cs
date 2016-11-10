using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class IndexParticipantViewModel
    {
        public List<ParticipantWithCountOfSessions> Participants { get; set; }
        [Display(Name = "Inkludera gömda?")]
        public bool IncludeDeleted { get; set; }
    }

    public class ParticipantWithCountOfSessions
    {
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int CountOfSessions { get; set; }

        public bool IsHrPerson { get; set; }

        public bool IsActiv { get; set; }

        public bool IsDeleted { get; set; }

    }
}