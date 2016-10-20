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
