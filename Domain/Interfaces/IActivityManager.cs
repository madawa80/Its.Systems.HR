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
    }
}