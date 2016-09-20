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
            this.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer(new HRContextSeeder());
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        //public virtual DbSet<Activity> Activities { get; set; }
        //public virtual DbSet<Session> Sessions { get; set; }
        //public virtual DbSet<Participant> Participants { get; set; }
        //public virtual DbSet<SessionParticipant> SessionParticipants { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<HrPerson> HrPerson { get; set; }
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
                new HrPerson() {Name = "xxxxx"},
            };

            foreach (var tag in tags)
                context.HrPerson.Add(tag);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
