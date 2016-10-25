using System;
using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class HrPerson
    {
        public HrPerson()
        {
            this.Sessions = new List<Session>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
