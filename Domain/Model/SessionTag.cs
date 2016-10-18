using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Model
{
    [Table("SessionTag")]
    public class SessionTag
    {
        [Key, Column(Order = 0)]    
        public int SessionId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        // NAV PROPS
        public Session Session { get; set; }
        public Tag Tag { get; set; }

    }
}
