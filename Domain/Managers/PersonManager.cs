using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class PersonManager : IPersonManager
    {
        public IDbRepository db;
        public PersonManager(IDbRepository repo)
        {
            db = repo;
        }

        //public IQueryable<Person> GetPersons()
        //{
        //    db.Get<Person>().Include().Where().
        //}

        public IQueryable<Participant> GetAllParticipants()
        {
            return db.Get<Participant>();
        }

        public IQueryable<HrPerson> GetAllHrPersons()
        {
            return db.Get<HrPerson>();
        }
    }

    public class ActivitiesManager : IActivityManager
    {
        public IDbRepository db;
        public ActivitiesManager(IDbRepository repo)
        {
            db = repo;
        }

        public IQueryable<Activity> GetAllActivities()
        {
            return db.Get<Activity>();
        }

        public IQueryable<Activity> GetAllActivitiesWithSessions()
        {
            return db.Get<Activity>();
        }

        public IQueryable<Session> GetAllSessionsForActivity(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Participant> GetAllParticipantsForSession(int id)
        {
            throw new NotImplementedException();
        }
    }
}
