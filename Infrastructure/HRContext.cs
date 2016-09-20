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
        public HRContext() : base("HRContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new HRContextSeeder());
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

    public class HRContextSeeder : DropCreateDatabaseAlways<HRContext>
    {
        protected override void Seed(HRContext context)
        {
            //Tags
            var tags = new List<HrPerson>
            {
                new HrPerson() {Name = "Samme"},
                new HrPerson() {Name = "Madawa"},
            };

            foreach (var tag in tags)
                context.HrPersons.Add(tag);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
