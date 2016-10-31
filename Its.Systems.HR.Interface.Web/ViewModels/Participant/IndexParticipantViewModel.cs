﻿using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class IndexParticipantViewModel
    {
        public List<ParticipantWithCountOfSessions> Participants { get; set; }
    }

    public class ParticipantWithCountOfSessions
    {
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string CasID { get; set; }
        public int CountOfSessions { get; set; }
    }
}