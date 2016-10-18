using System.Collections.Generic;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class FilterSessionsViewModel
    {
        public List<Session> Sessions { get; set; }
        public IEnumerable<SelectListItem> HrPersons { get; set; }

        public Participant HrPerson { get; set; }
        public string NameOfLocation { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
    }
}