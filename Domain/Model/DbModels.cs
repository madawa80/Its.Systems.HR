using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Model
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Session> Sessions { get; set; }

    }
    public class Session
    {
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


        //public virtual List<Participant> Participants { get; set; }

        //public List<Tag> Tags { get; set; }
    }

    public class Participant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //TODO: More properties

        //public virtual List<Session> Sessions { get; set; }
    }

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
    [Table("Location")]
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        //public List<Session> Sessions { get; set; }
    }

    

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
