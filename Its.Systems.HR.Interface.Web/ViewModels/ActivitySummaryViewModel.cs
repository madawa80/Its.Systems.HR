using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivitySummaryViewModel
    {


        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public string PaticipantName { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Session> Sessions { get; set; }
        public List<SessionParticipant> SessionParticipants { get; set; }

    }
}