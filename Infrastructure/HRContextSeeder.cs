using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Infrastructure
{
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

            context.SaveChanges();

            // Location
            var locations = new List<Location>
            {
                new Location() {Name = "Umeå", Address = "Kungsgatan 123"},
                new Location() {Name = "Stockholm", Address = "Kungens kurva 123"},
                new Location() {Name = "San Fransisco", Address = "Silicon Valley 123"},
                new Location() {Name = "Berlin", Address = "Alexanderplatz 123"},
                new Location() {Name = "London", Address = "Kings Cross 123"},
                new Location() {Name = "Skavsta", Address = "Flygplatsvägen 123"},
                new Location() {Name = "Skellefteå", Address = "Gata 123"},
                new Location() {Name = "Karlskoga", Address = "Gata 123"},
            };

            foreach (var location in locations)
                context.Locations.Add(location);

            context.SaveChanges();


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

            context.SaveChanges();

            //HR person
            //var HRS = new List<HrPerson>
            //{
            //    new HrPerson() {FirstName = "Samme",LastName = "Petersson"},
            //    new HrPerson() {FirstName = "Elina",LastName = "Eriksson"},
            //    new HrPerson() {FirstName  = "Raad",LastName = "Larsson"},
            //    new HrPerson() {FirstName  = "Jan",LastName = "Petersson"},
            //    new HrPerson() {FirstName = "Kim",LastName = "Henriksson"},
            //    new HrPerson() {FirstName = "Linda",LastName = "Bengtsson"},
            //    new HrPerson() {FirstName = "Ingen",LastName = "ansvarig"},

            //};

            //foreach (var HR in HRS)
            //    context.HrPersons.Add(HR);

            //context.SaveChanges();

            // Sessions
            var sessions = new List<Session>()
            {
                new Session()
                {
                    Name = "AirHack PowerHackathon 2017",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPerson = null,
                    Location = context.Locations.SingleOrDefault(n => n.Name == "Stockholm"),
                    Activity = context.Activities.SingleOrDefault(n => n.Name == "AirHack"),
                },
                new Session()
                {
                    Name = "Lunchföreläsning Kompetensutveckling i arbetet",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPerson = null,
                    Location = context.Locations.SingleOrDefault(n => n.Name == "Umeå"),
                    Activity = context.Activities.SingleOrDefault(n => n.Name == "Lunchföreläsning"),
                },
                new Session()
                {
                    Name = "JavaOne 2015",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPerson = null,
                    Location = context.Locations.SingleOrDefault(n => n.Name == "Stockholm"),
                    Activity = context.Activities.SingleOrDefault(n => n.Name == "JavaOne"),
                },
                new Session()
                {
                    Name = "JavaOne 2016",
                    StartDate = GenerateRandomStartDate(),
                    EndDate = GenerateRandomEndDate(),
                    HrPerson = null,
                    Location = context.Locations.SingleOrDefault(n => n.Name == "Umeå"),
                    Activity = context.Activities.SingleOrDefault(n => n.Name == "JavaOne"),
                },

            };

            foreach (var session in sessions)
                context.Sessions.Add(session);

            context.SaveChanges();


            //Participant

            var participants = new List<Participant>
            {
                new Participant() {
                    FirstName  = "Madawa",
                    LastName = "Abeywickrama",
                    Comments = "Erbjuden Jfokus 2015, men var inte intresserad.",
                    Wishes = "Enligt utvecklingssamtal 2016-03-19 vill Stina gärna delta i fler bokcirklar."
                },
                new Participant()
                {
                    FirstName  = "Sam",
                    LastName = "Thomsson",
                    Comments = "Erbjuden AirHack 2010, men var inte intresserad.",
                    Wishes = "Enligt utvecklingssamtal 2010-03-19 vill Sam gärna delta i fler Hackathons."
                },
                new Participant()
                {
                    FirstName = "Paul",
                    LastName = "Alverardo",
                    Comments = "",
                    Wishes = "Enligt Paul så har han inte tid med kompetensutveckling."
                },
                new Participant()
                {
                    FirstName  = "Jean",
                    LastName = "Smith",
                    Comments = "Jean har deltagit på alla tillfällen under 10 år",
                    Wishes = ""
                },
                new Participant()
                {
                    FirstName  = "Joe",
                    LastName = "Root"
                },
                new Participant()
                {
                    FirstName  = "Linda",
                    LastName = "Bengtsson"
                },
                new Participant()
                {
                    FirstName  = "Karin",
                    LastName = "Andersson"
                },
                new Participant()
                {
                    FirstName  = "Pelle",
                    LastName = "Persson"
                },

            };

            foreach (var participant in participants)
                context.Participants.Add(participant);

            context.SaveChanges();


            // SessionParticipants
            var sessionParticipants = new List<SessionParticipant>
            {
                new SessionParticipant()
                {
                    ParticipantId = 1,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="JavaOne 2016"),
                    Rating = 5,
                },
                new SessionParticipant()
                {
                    ParticipantId = 2,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="JavaOne 2016"),
                    Rating = 3,
                },
                new SessionParticipant()
                {
                    ParticipantId = 1,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="JavaOne 2015"),
                    Rating = 3,
                },
                new SessionParticipant()
                {
                    ParticipantId = 5,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="JavaOne 2015"),
                    Rating = 2,
                },
                new SessionParticipant()
                {
                    ParticipantId = 2,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="AirHack PowerHackathon 2017"),
                    Rating = 3,
                },
                new SessionParticipant()
                {
                    ParticipantId = 3,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="AirHack PowerHackathon 2017"),
                    Rating = 5,
                },
                new SessionParticipant()
                {
                    ParticipantId = 4,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="AirHack PowerHackathon 2017"),
                    Rating = 3,
                },
                new SessionParticipant()
                {
                    ParticipantId = 5,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="AirHack PowerHackathon 2017"),
                    Rating = 3,
                },
                new SessionParticipant()
                {
                    ParticipantId = 1,
                    Session = context.Sessions.SingleOrDefault(n => n.Name=="Lunchföreläsning Kompetensutveckling i arbetet"),
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
            var randomDate = DateTime.Now.AddMonths(_random.Next(1, 12)).AddDays(_random.Next(1, 365));
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