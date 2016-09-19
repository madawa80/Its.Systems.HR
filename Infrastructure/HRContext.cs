using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Infrastructure
{
    public class HRContext : DbContext
    {
        public HRContext() : base("name=HRContext")
        {
            Database.SetInitializer<HRContext>(null);
            this.Configuration.LazyLoadingEnabled = false;
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<SessionParticipant> SessionParticipants { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<HrPerson> HrPersons { get; set; }
    }
}
