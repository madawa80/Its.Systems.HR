using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class CreateSessionViewModel
    {
        //public int Id { get; set; }
        [Display(Name = "Namn på kurstillfälle")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }

        public string NameOfLocation { get; set; }
        [Required]
        public HrPerson HrPerson { get; set; }
        [Required]
        public Activity Activity { get; set; }


        public List<SessionParticipant> SessionParticipants { get; set; }
        public string AddedParticipants { get; set; }


        //public string[] addedParticipantsSelectBox { set; get; }

        //public IEnumerable<SelectListItem> addedParticipantsSelectBox { get; set; }
    }
}
