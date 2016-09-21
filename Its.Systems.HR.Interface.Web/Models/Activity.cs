using Its.Systems.HR.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Its.Systems.HR.Interface.Web.Models
{
    public class Activity
    {
        
            public string Name { get; set; }
            public virtual List<Session> Sessions { get; set; }
      
    }
}