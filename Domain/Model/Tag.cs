using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Tag")]
    public class Tag
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Index("NameIndex", IsUnique = true)]
        public string Name { get; set; }

        public virtual List<Session> Sessions { get; set; }
    }
}
