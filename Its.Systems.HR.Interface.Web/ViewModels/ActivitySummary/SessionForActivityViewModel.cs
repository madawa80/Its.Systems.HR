using System;
using System.Collections.Generic;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionForActivityViewModel
    {
        public string Comments { get; set; }
        public string Evaluation { get; set; }

        public DateTime StartDate{ get; set; }

        public DateTime EndDate { get; set; }

        public HrPerson HrPerson { get; set; }

        public int TotalPaticipants { get; set; }
        public int SessionId { get; set; }

        public string SessionName { get; set; }

        public List<Participant> Participants { get; set; }
        public Location Location { get; set; }
    }
}