using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Domain.Model
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public string CasId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }


        public int Telephone { get; set; }
        public bool IsActive { get; set; }
    }

}
