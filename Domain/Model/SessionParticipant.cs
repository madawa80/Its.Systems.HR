using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("SessionParticipant")]
    public class SessionParticipant
    {
        [Key, Column(Order = 0)]
        public int SessionId { get; set; }
        [Key, Column(Order = 1)]
        public int ParticipantId { get; set; }
        public int Rating { get; set; }

        public virtual Session Session { get; set; }
        public virtual Participant Participant { get; set; }

      
    }
}