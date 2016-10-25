using System.ComponentModel.DataAnnotations;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ReviewSessionViewModel
    {
        public int SessionId { get; set; }

        public string SessionName { get; set; }

        public int ParticipantId { get; set; }

        public string ParticipantName { get; set; }

        [Range(1, 5)]
        [Display(Name = "Betyg")]
        public int Rating { get; set; }

        [Display(Name = "Ev. kommentar")]
        public string Comments { get; set; }
    }
}