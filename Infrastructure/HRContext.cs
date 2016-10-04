using System.Data.Common;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Infrastructure
{
    public class HRContext : DbContext
    {
        private const string ConnString = "HRContext";
        //private const string connString = "name=HRContext";

        public HRContext() : base(ConnString)
        {
            this.Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HRContext, Infrastructure.Migrations.Configuration>(ConnString));
            //Database.SetInitializer(new HRContextSeeder());
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Activity Sessions
            modelBuilder.Entity<Activity>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Activity)
                .HasForeignKey(e => e.ActivityId);

            // Session Activity (one-to-many)
            modelBuilder.Entity<Session>()
                        .HasRequired<Activity>(s => s.Activity) // Student entity requires Standard 
                        .WithMany(s => s.Sessions); // Standard entity includes many Students entities

            // Session Location (one-to-many)
            modelBuilder.Entity<Session>()
                .HasOptional<Location>(s => s.Location)
                .WithMany(n => n.Sessions);

            // SessionParticipant Sessions (one-to-many)
            modelBuilder.Entity<SessionParticipant>()
                .HasRequired<Session>(s => s.Session)
                .WithMany(n => n.SessionParticipants);

            // SessionParticipants Participants (one-to-many)
            modelBuilder.Entity<SessionParticipant>()
                .HasRequired<Participant>(s => s.Participant)
                .WithMany(n => n.SessionParticipants);

            // Session HrPerson
            modelBuilder.Entity<Session>()
                        .HasOptional<HrPerson>(s => s.HrPerson) // Student entity requires Standard 
                        .WithMany(s => s.Sessions); // Standard entity includes many Students entities

            // TODO: Write all relations in fluent!

            modelBuilder.Entity<Session>()
            .HasMany<Tag>(s => s.Tags)
            .WithMany(c => c.Sessions)
            .Map(cs =>
            {
                cs.MapLeftKey("SessionId");
                cs.MapRightKey("TagId");
                cs.ToTable("SessionTag");
            });

        }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<HrPerson> HrPersons { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<SessionParticipant> SessionParticipants { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
    }
}
