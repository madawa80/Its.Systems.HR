using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Model
{
    [Table("HrPerson")]
    public partial class HrPerson
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //TODO: More properties

        public string GetHrFullName()
        {
            return FirstName + " " + LastName;
        }

        public virtual List<Session> Sessions { get; set; }

        //public void AddActivity(string input)
        //{
        //    var result = new Activity()
        //    {
        //        Name = input,
        //    };
        //}
    }
}
