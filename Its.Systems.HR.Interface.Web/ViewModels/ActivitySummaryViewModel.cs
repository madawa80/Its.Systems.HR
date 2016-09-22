using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivitySummaryViewModel
    {
        public List<Activity> Activities { get; set; }
        public List<Session> Sessions { get; set; }
        public List<SessionParticipant> SessionParticipants { get; set; }

    }
}