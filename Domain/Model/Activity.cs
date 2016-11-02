using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Session> Sessions { get; set; }
    }
}