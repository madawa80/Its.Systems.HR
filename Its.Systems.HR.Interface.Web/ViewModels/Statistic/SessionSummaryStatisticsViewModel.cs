using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionSummaryStatisticsViewModel
    {
        public List<int> Years { get; set; }
        public int TotalPaticipants;
        public int TotalSessions;
        public List<SessionStatisticsRow> SessionStatisticsRows { get; set; }

    }
}