using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Tag")]
    public class Tag
    {
        public int Id { get; set; }
        //[StringLength(100)]
        //[Index("NameIndex", IsUnique = true)]
        public string Name { get; set; }

        public virtual List<SessionTag> SessionTags { get; set; }
    }
}
