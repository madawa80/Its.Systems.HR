using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class ActivitySummaryViewModel
    {


        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public int SessionId { get; set; }
        public string SessionName { get; set; }
        public int PaticipantId { get; set; }
        public string PaticipantName { get; set; }
        public string Comments { get; set; }
        public string Evaluation { get; set; }
        public IEnumerable<SelectListItem> Activities { get; set; }
        public IEnumerable<SelectListItem> Sessions { get; set; }
        public List<SelectListItem> SessionParticipants { get; set; }

    }
}