using System;
using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class Session
    {
        public Session()
        {
            this.SessionParticipants = new List<SessionParticipant>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int LocationId { get; set; }
        public int HrPersonId { get; set; }
        public int ActivityId { get; set; }
        public string Comments { get; set; }
        public string Evaluation { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual HrPerson HrPerson { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<SessionParticipant> SessionParticipants { get; set; }
    }
}
