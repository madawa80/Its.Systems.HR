﻿using System;
using System.Collections.Generic;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionForActivityViewModel
    {
        public string Comments { get; set; }
        public string Evaluation { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Its.Systems.HR.Domain.Model.Participant HrPerson { get; set; }
        public string Rating { get; set; }

        public int TotalPaticipants { get; set; }
        public int SessionId { get; set; }

        public string SessionNameWithActivity { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }

        public List<Its.Systems.HR.Domain.Model.Participant> Participants { get; set; }
        public Location Location { get; set; }
        public List<Tag> Tags { get; set; }
    }
}