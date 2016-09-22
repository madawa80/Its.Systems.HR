using System.Data.Common;
using System.Data.Entity;
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
            //Database.SetInitializer(new HRContextSeeder());
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .HasMany(e => e.Sessions)
                .WithRequired(e => e.Activity)
                .HasForeignKey(e => e.ActivityId);

            //one-to-many 
            modelBuilder.Entity<Session>()
                        .HasRequired<Activity>(s => s.Activity) // Student entity requires Standard 
                        .WithMany(s => s.Sessions); // Standard entity includes many Students entities

            //one-to-many 
            modelBuilder.Entity<Session>()
                .HasRequired<Location>(s => s.Location)
                .WithMany(n => n.Sessions);

            //one-to-many 
            modelBuilder.Entity<Session>()
                .HasRequired<HrPerson>(s => s.HrPerson)
                .WithMany(n => n.Sessions);

            //one-to-many 
            modelBuilder.Entity<SessionParticipant>()
                .HasRequired<Session>(s => s.Session)
                .WithMany(n => n.SessionParticipants);

            //one-to-many 
            modelBuilder.Entity<SessionParticipant>()
                .HasRequired<Participant>(s => s.Participant)
                .WithMany(n => n.SessionParticipants);
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
