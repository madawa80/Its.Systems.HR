using System;
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


        public void SaveActivities(Activity activity)
        {
            db.Add(activity);
            db.SaveChanges();

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

        public Activity EditActivity(Activity activityToEdit)
        {
            //TODO: Add error handling!?
            db.Context().Entry(activityToEdit).State = EntityState.Modified;
            db.SaveChanges();

            return activityToEdit;
        }

        public bool DeleteActivity(int id)
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
    }
}