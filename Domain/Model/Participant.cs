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
        public string CasId { get; set; }
        public bool IsHrPerson { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Wishes { get; set; }

        public List<SessionParticipant> SessionParticipants { get; set; }

        public string FullName => FirstName + " " + LastName;
        public string FullNameWithCas => FirstName + " " + LastName + " (" + CasId + ")";

        public string GetParticipantFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}