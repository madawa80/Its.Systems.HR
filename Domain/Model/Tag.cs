using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Tag")]
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<SessionTag> SessionTags { get; set; }
    }
}
