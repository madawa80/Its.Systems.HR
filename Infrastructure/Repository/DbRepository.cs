using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;

namespace Its.Systems.HR.Infrastructure.Repository
{
    public class DbRepository : BaseRepository, IDbRepository
    {
        public DbRepository(HRContext context) : base(context) { }

        public IQueryable<T> Get<T>() where T : class
        {
            return ctx.Set<T>().AsQueryable<T>();
        }

        public DbSet<T> GetSet<T>() where T : class
        {
            return ctx.Set<T>();
        }

        public T Add<T>(T input) where T : class
        {
            try
            {
                ctx.Set<T>().Add(input);
                ctx.SaveChanges();
                return input;
            }
            catch (DbEntityValidationException exception)
            {
                //Trace.TraceInformation(exception.);
                foreach (var err in exception.EntityValidationErrors)
                {
                    Trace.TraceInformation(err.ToString());
                    foreach (var err2 in err.ValidationErrors)
                    {
                        Trace.TraceInformation(err2.ToString());
                        Trace.TraceInformation(err2.ErrorMessage);
                        Trace.TraceInformation(err2.PropertyName);
                    }
                }

                throw;
            }
        }

        public bool Delete<T>(T input) where T : class
        {
            ctx.Set<T>().Remove(input);
            ctx.SaveChanges();
            return true;
        }

        public bool SaveChanges()
        {
            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException exception)
            {
                //Trace.TraceInformation(exception.);
                foreach (var err in exception.EntityValidationErrors)
                {
                    Trace.TraceInformation(err.ToString());
                    foreach (var err2 in err.ValidationErrors)
                    {
                        Trace.TraceInformation(err2.ToString());
                        Trace.TraceInformation(err2.ErrorMessage);
                        Trace.TraceInformation(err2.PropertyName);
                    }
                }
                throw;
            }
        }

        public void Close()
        {
            var conn = ctx.Database.Connection;
            conn.Close();
        }

        public DbContext Context()
        {
            return ctx;
        }
    }
}
