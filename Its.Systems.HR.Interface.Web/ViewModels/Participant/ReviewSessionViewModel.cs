using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ReviewSessionViewModel
    {
        public int SessionId { get; set; }

        public string SessionName { get; set; }

        public int ParticipantId { get; set; }

        public string ParticipantName { get; set; }

        [Range(minimum:1, maximum:5)]
        [Display(Name = "Betyg")]
        public int Rating { get; set; }

        [Display(Name = "Ev. kommentar")]
        public string Comments { get; set; }
    }
}
