using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Its.Systems.HR.Interface.Web.Models.Mapping;

namespace Its.Systems.HR.Interface.Web.Models
{
    public partial class testContext : DbContext
    {
        static testContext()
        {
            Database.SetInitializer<testContext>(null);
        }

        public testContext()
            : base("Name=testContext")
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<HrPerson> HrPersons { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionParticipant> SessionParticipants { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActivityMap());
            modelBuilder.Configurations.Add(new HrPersonMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new SessionMap());
            modelBuilder.Configurations.Add(new SessionParticipantMap());
            modelBuilder.Configurations.Add(new TagMap());
        }
    }
}
