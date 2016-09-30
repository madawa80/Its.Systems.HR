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
        public IQueryable Sessions { get; set; }
        public List<int> SessionParticipants { get; set; }

    }
}