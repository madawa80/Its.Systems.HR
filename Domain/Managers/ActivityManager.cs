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
        private readonly IDbRepository _db;

        public ActivityManager(IDbRepository repo)
        {
            _db = repo;
        }

        public IQueryable<Activity> GetAllActivities()
        {
            return _db.Get<Activity>();
        }

        public IQueryable<Activity> GetAllActivitiesWithSessions()
        {
            return _db.Get<Activity>().Include(n => n.Sessions); //.Where(s => s.Activity == n)
        }

        public Activity GetActivityById(int id)
        {
            return _db.Get<Activity>().SingleOrDefault(n => n.Id == id);
        }

        public bool AddActivity(Activity activityToAdd)
        {
            var allActivities = _db.Get<Activity>().ToList();

            if (allActivities.Any(n => n.Name == activityToAdd.Name))
                return false;

            _db.Add<Activity>(activityToAdd);

            return true;
        }

        public bool EditActivity(int activityId, string newName)
        {
            var allActivities = _db.Get<Activity>().ToList();

            if (allActivities.Any(n => n.Name == newName))
                return false;

            //TODO: Add error handling!?
            var activityToEdit = GetActivityById(activityId);
            activityToEdit.Name = newName;
            _db.Context().Entry(activityToEdit).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        public bool DeleteActivityById(int id)
        {
            var activityFromDb = _db.Get<Activity>().SingleOrDefault(n => n.Id == id);
            if (activityFromDb == null)
                return false;

            _db.Delete(activityFromDb);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }
    }
}
