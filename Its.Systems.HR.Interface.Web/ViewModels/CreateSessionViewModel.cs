using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class CreateSessionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Location Location { get; set; }
        [Required]
        public virtual HrPerson HrPerson { get; set; }
        [Required]
        public virtual Activity Activity { get; set; }


        public virtual List<SessionParticipant> SessionParticipants { get; set; }
    }
}
