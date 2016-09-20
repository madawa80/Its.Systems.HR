using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Participant")]
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        //TODO: More properties
        public string GetParticipantFullName()
        {
            return FirstName + " " + LastName;
        }
        public virtual List<SessionParticipant> SessionParticipants { get; set; }
    }
}