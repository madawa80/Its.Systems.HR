using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Session")]
    public class Session
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }
        public string Evaluation { get; set; }

        public string NameWithActivity => Activity.Name + " " + Name;

        // nav props
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public int? HrPersonId { get; set; }
        public Participant HrPerson { get; set; }
        [Required]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public List<SessionParticipant> SessionParticipants { get; set; }
        public List<SessionTag> SessionTags { get; set; }
    }
}