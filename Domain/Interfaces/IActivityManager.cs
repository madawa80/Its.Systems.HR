using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Domain.Interfaces
{
    public interface IActivityManager
    {
        /// <summary>
        /// Gets all the activities.
        /// </summary>
        /// <returns></returns>
        IQueryable<Activity> GetAllActivities();

        IQueryable<Activity> GetAllActivitiesWithSessions();

        /// <summary>
        /// Gets an activity by Id.
        /// </summary>
        /// <param name="id">Id for the activity</param>
        /// <returns>Activity if found.</returns>
        Activity GetActivityById(int id);

        /// <summary>
        /// Adds the activity passed.
        /// </summary>
        /// <param name="activityToAdd"></param>
        /// <returns>True if successfull.</returns>
        bool AddActivity(Activity activityToAdd);

        /// <summary>
        /// Edits the activity name by activityId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        bool EditActivity(int id, string newName);

        /// <summary>
        /// Deletes an activity by activityId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if successfull.</returns>
        bool DeleteActivityById(int id);
    }
}