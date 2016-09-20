﻿using System;
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
        private Random _random = new Random();
        private DateTime _lastDateGenerated;
        

        protected override void Seed(HRContext context)
        {
            // Activity
            var activities = new List<Activity>
            {
                new Activity() {Name = "AirHack"},
                new Activity() {Name = "JavaOne"},
                new Activity() {Name = "JFokus"},
                new Activity() {Name = "Bokcirkel"},
                new Activity() {Name = "Lunchföreläsning"},
            };

            foreach (var activity in activities)
                context.Activities.Add(activity);

            // Location
            var locations = new List<Location>
            {
                new Location() {Name = "Umeå", Address = "Kungsgatan 123"},
                new Location() {Name = "Stockholm", Address = "Kungens kurva 123"},
                new Location() {Name = "San Fransisco", Address = "Silicon Valley 123"},
                new Location() {Name = "Berlin", Address = "Alexanderplatz 123"},
                new Location() {Name = "London", Address = "Kings Cross 123"},
            };

            foreach (var location in locations)
                context.Locations.Add(location);

            // Tags for Sessions
            var tags = new List<Tag>()
            {
                new Tag() {Name = "distans"},
                new Tag() {Name = "lunch"},
                new Tag() {Name = "c#"},
                new Tag() {Name = "java"},
                new Tag() {Name = "databaser"},
            };
            foreach (var tag in tags)
                context.Tags.Add(tag);

            // Sessions
            var sessions = new List<Session>()
            {
                new Session()
                {
                    Name = "2015",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPersonId = 1,
                    Location = locations.SingleOrDefault(n => n.Name == "Stockholm"),
                    Activity = activities.SingleOrDefault(n => n.Name == "JavaOne"),
                },
                new Session()
                {
                    Name = "2016",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPersonId = 1,
                    Location = locations.SingleOrDefault(n => n.Name == "Umeå"),
                    Activity = activities.SingleOrDefault(n => n.Name == "JavaOne"),
                },
            };

             foreach (var session in sessions)
                context.Sessions.Add(session);

            //HR person
            var HRS = new List<HrPerson>
            {
                new HrPerson() {FirstName = "Samme",LastName = "Petersson"},
                new HrPerson() {FirstName = "Elina",LastName = "Eriksson"},
                new HrPerson() {FirstName  = "Raad",LastName = "Larsson"},
                new HrPerson() {FirstName  = "Jan",LastName = "Petersson"},
                new HrPerson() {FirstName = "Kim",LastName = "Henriksson"},
            };

            foreach (var HR in HRS)
                context.HrPersons.Add(HR);

            //Participant

            var paticipants = new List<Participant>
            {
                new Participant() {FirstName  = "Madawa",LastName = "Abeywickrama"},
                new Participant() {FirstName  = "Sam",LastName = "Thomsson"},
                new Participant() {FirstName = "Paul",LastName = "Alverardo"},
                new Participant() {FirstName  = "Jean",LastName = "Smith"},
                new Participant() {FirstName  = "Joe",LastName = "Root"},
            };



            foreach (var participant in paticipants)
                context.Participants.Add(participant);

            // SessionParticipants
            var sessionParticipants = new List<SessionParticipant>
            {
                new SessionParticipant()
                {
                    ParticipantId = 1,
                    Session = sessions.SingleOrDefault(n => n.Name=="JavaOne"),
                    Rating = 5,
                }
            };

            foreach (var sessionParticipant in sessionParticipants)
                context.SessionParticipants.Add(sessionParticipant);


            context.SaveChanges();
            base.Seed(context);
        }


        private DateTime GenerateRandomStartDate()
        {
            var randomDate = DateTime.Now.AddMonths(_random.Next(1, 12)).AddDays(_random.Next(1,365));
            _lastDateGenerated = randomDate;

            return randomDate;
        }
        private DateTime GenerateRandomEndDate()
        {
            var randomEndDate = _lastDateGenerated.AddDays(_random.Next(1, 10));

            return randomEndDate;
        }
    }
}
