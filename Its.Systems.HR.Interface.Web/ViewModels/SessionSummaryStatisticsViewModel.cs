using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionSummaryStatisticsViewModel
    {
        public List<int> Years { get; set; }
        public int TotalPaticipants;
        public int TotalSessions;
        public List<SessionStatisticsRow> SessionStatisticsRows { get; set; }

    }

    public class SessionStatisticsRow
    {
        public Session Session { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}