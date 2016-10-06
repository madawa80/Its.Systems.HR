using System.Collections.Generic;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class AllSessionsForActivityViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }

        public List<Session> Sessions { get; set; }
    }
}