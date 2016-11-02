using System;
using System.Collections.Generic;

using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class AdminViewModel
    {
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string CasID { get; set; }

        public List<Participant> Participants { get; set; }
    }
}