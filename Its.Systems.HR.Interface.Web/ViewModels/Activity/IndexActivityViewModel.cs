using System.Collections.Generic;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class IndexActivityViewModel
    {
        public List<ActivityWithCountOfSessions> Activities { get; set; }
    }

    public class ActivityWithCountOfSessions
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public int SessionCount { get; set; }
    }

}