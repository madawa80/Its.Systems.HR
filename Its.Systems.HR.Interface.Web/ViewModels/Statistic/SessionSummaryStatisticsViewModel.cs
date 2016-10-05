using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionSummaryStatisticsViewModel
    {
        public List<int> Years { get; set; }
        public List<SessionStatisticsRow> SessionStatisticsRows { get; set; }

    }
}