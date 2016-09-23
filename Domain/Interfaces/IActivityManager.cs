using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IActivityManager
    {
        IQueryable<Activity> GetAllActivities();

        IQueryable<Activity> GetAllActivitiesWithSessions();

        IQueryable<Session> GetAllSessionsForActivity(int id);

        IQueryable<Participant> GetAllParticipantsForSession(int id);

        IQueryable<Session> GetAllSessionsForParticipantById(int id);

        /// <summary>
        /// Gets an activity by Id
        /// </summary>
        /// <param name="id">Id for event</param>
        /// <returns>Activity if found, else null!</returns>
        Activity GetActivityById(int id);

        bool AddActivity(Activity activityToAdd);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityToEdit"></param>
        /// <returns></returns>
        bool EditActivity(Activity activityToEdit);

        bool DeleteActivityById(int id);

        void AddSession(Session session);

        Session GetSessionById(int id);

        IQueryable<Location> GetAllLocations();

        IQueryable<Session> GetAllSessions();
    }
}