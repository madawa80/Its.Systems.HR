using System;
using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class SessionParticipant
    {
        public int SessionId { get; set; }
        public int ParticipantId { get; set; }
        public int Rating { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual Session Session { get; set; }
    }
}
