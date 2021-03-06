﻿using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Infrastructure
{
    public class HRContext : DbContext
    {
        private const string ConnString = "HRContext";

        public HRContext() : base(ConnString)
        {
            this.Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HRContext, Infrastructure.Migrations.Configuration>(ConnString));

            // TODO: remove logging SQL to debug.write for release
            Database.Log = sql => Debug.Write(sql);
            //Database.SetInitializer(new HRContextSeeder());
        }

        public HRContext(DbConnection connection) : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // TODO: Maybe write all relations in fluent instead of using data annotations!?

            //// Activity Sessions
            //modelBuilder.Entity<Activity>()
            //    .HasMany(e => e.Sessions)
            //    .WithRequired(e => e.Activity)
            //    .HasForeignKey(e => e.ActivityId);

            //// Session Activity (one-to-many)
            //modelBuilder.Entity<Session>()
            //            .HasRequired<Activity>(s => s.Activity) // Student entity requires Standard 
            //            .WithMany(s => s.Sessions); // Standard entity includes many Students entities

            //// Session Location (one-to-many)
            //modelBuilder.Entity<Session>()
            //    .HasOptional<Location>(s => s.Location)
            //    .WithMany(n => n.Sessions);

            //// SessionParticipant Sessions (one-to-many)
            //modelBuilder.Entity<SessionParticipant>()
            //    .HasRequired<Session>(s => s.Session)
            //    .WithMany(n => n.SessionParticipants);

            //// SessionParticipants Participants (one-to-many)
            //modelBuilder.Entity<SessionParticipant>()
            //    .HasRequired<Participant>(s => s.Participant)
            //    .WithMany(n => n.SessionParticipants);

            //// Session table
            //modelBuilder.Entity<Session>()
            //    .HasOptional<Participant>(s => s.HrPerson);
            //    //.WithMany(s => s.Sessions);

        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<SessionParticipant> SessionParticipants { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        //public virtual DbSet<SessionTag> SessionTags { get; set; }
    }
}
