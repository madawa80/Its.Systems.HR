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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // nav props
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public int HrPersonId { get; set; }
        [Required]
        public virtual HrPerson HrPerson { get; set; }
        public int ActivityId { get; set; }
        [Required]
        public virtual Activity Activity { get; set; }


        public virtual List<SessionParticipant> SessionParticipants { get; set; }

        //public List<Tag> Tags { get; set; }
    }
}