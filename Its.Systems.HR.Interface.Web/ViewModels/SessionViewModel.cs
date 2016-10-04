using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Its.Systems.HR.Interface.Web.ViewModels
{
    public class SessionViewModel
    {



        public string ActivityName { get; set; }

        public List<SessionRow> SessionRows { get; set; }

    }

    public class SessionRow
       {
            public int Id { get; set; }
            public string Name { get; set; }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
}