using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Its.Systems.HR.Infrastructure.Repository
{
    public class BaseRepository
    {
        public HRContext ctx;

        public BaseRepository(HRContext context)
        {
            ctx = context;
            
        //    if (Properties.Settings.Default.Debug)
        //    {
                //ctx.Database.Log = message => System.Diagnostics.Trace.TraceInformation(message);
        //    }
        }
    }
}
