using System;
using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class Participant
    {
        public Participant()
        {
            this.SessionParticipants = new List<SessionParticipant>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Wishes { get; set; }
        public virtual ICollection<SessionParticipant> SessionParticipants { get; set; }
    }
}
