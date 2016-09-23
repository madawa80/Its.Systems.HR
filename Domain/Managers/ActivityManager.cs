using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Managers
{
    public class ActivityManager : IActivityManager
    {
        public IDbRepository db;

        public ActivityManager(IDbRepository repo)
        {
            db = repo;
        }

        public IQueryable<Activity> GetAllActivities()
        {
            return db.Get<Activity>();
        }

        public IQueryable<Activity> GetAllActivitiesWithSessions()
        {
            return db.Get<Activity>().Include(n => n.Sessions); //.Where(s => s.Activity == n)
        }

        public IQueryable<Session> GetAllSessionsForActivity(int id)
        {
            return db.Get<Session>().Where(n => n.ActivityId == id);
        }

        public IQueryable<Participant> GetAllParticipantsForSession(int id)
        {
            return db.Get<SessionParticipant>().Where(n => n.SessionId == id).Select(n => n.Participant);
        }

        public IQueryable<Session> GetAllSessionsForParticipantById(int id)
        {
            return db.Get<SessionParticipant>().Where(n => n.ParticipantId == id).Select(n => n.Session);
        }



        public Activity GetActivityById(int id)
        {
            return db.Get<Activity>().SingleOrDefault(n => n.Id == id);
        }

        public bool AddActivity(Activity activityToAdd)
        {
            var allActivities = db.Get<Activity>().ToList();

            if (allActivities.Any(n => n.Name == activityToAdd.Name))
                return false;

            db.Add<Activity>(activityToAdd);

            return true;
        }

        public bool EditActivity(Activity activityToEdit)
        {
            //var allActivities = db.Get<Activity>().ToList();

            //if (allActivities.Any(n => n.Name == activityToEdit.Name))
            //    return false;

            //TODO: Add error handling!?
            db.Context().Entry(activityToEdit).State = EntityState.Modified;
            db.SaveChanges();

            return true;
        }

        public bool DeleteActivityById(int id)
        {
            var activityFromDb = db.Get<Activity>().SingleOrDefault(n => n.Id == id);
            if (activityFromDb == null)
                return false;

            db.Delete(activityFromDb);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }

            return true;
        }

        public void AddSession(Session session)
        {
            db.Add<Session>(session);
        }

        public Session GetSessionById(int id)
        {
            return db.Get<Session>().SingleOrDefault(n => n.Id == id);
        }

        public IQueryable<Location> GetAllLocations()
        {
            return db.Get<Location>();
        }

        public IQueryable<Session> GetAllSessions()
        {
            return db.Get<Session>();
        }
    }
}