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

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
