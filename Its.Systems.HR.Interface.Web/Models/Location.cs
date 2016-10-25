using System;
using System.Collections.Generic;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class Location
    {
        public Location()
        {
            this.Sessions = new List<Session>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
