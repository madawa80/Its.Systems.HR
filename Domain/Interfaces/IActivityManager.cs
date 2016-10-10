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
        /// Gets all the sessions for an activity by activityId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Session> GetAllSessionsForActivity(int id);

        /// <summary>
        /// Gets all participants for a session by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Participant> GetAllParticipantsForSession(int id);

        /// <summary>
        /// Gets all the sessions for a participant by participantId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IQueryable<Session> if found.</Session></returns>
        IQueryable<Session> GetAllSessionsForParticipantById(int id);

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
        /// Edits the activity passed.
        /// </summary>
        /// <param name="activityToEdit"></param>
        /// <returns>True if successfull.</returns>
        bool EditActivity(Activity activityToEdit);

        /// <summary>
        /// Deletes an activity by activityId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if successfull.</returns>
        bool DeleteActivityById(int id);

        //bool DeletePaticipantById(int id);

        /// <summary>
        /// Adds the session passed.
        /// </summary>
        /// <param name="session"></param>
        void AddSession(Session session);

        /// <summary>
        /// Gets a session by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Session if found.</returns>
        Session GetSessionById(int id);

        /// <summary>
        /// Get all the locations in the db.
        /// </summary>
        /// <returns>IQueryable of Location.</returns>
        IQueryable<Location> GetAllLocations();

        /// <summary>
        /// Gets all the sessions.
        /// </summary>
        /// <returns>IQueryable of Session.</returns>
        IQueryable<Session> GetAllSessions();

        /// <summary>
        /// Saves the comments passed for a session, by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comments"></param>
        /// <returns>True if successfull.</returns>
        bool SaveCommentsForSession(int id, string comments);

        /// <summary>
        /// Saves the evaluation passed for a session, by sessionId.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evaluation"></param>
        /// <returns>True if successfull.</returns>
        bool SaveEvaluationForSession(int id, string evaluation);

        /// <summary>
        /// Gets the session by sessionId, includes Location, HrPerson and Tags.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>The session with Location, HrPerson and Tags.</returns>
        Session GetSessionByIdWithIncludes(int sessionId);

        /// <summary>
        /// Gets all sessions, includes Location, HrPerson and Tags.
        /// </summary>
        /// <returns>IQueryble of Session with Location, HrPerson and Tags.</returns>
        IQueryable<Session> GetAllSessionsWithIncludes();

        /// <summary>
        /// Gets all the sessions for a specific year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>IQueryable of Session.</returns>
        IQueryable<Session> GetAllSessionsForYear(int year);

        /// <summary>
        /// Edits the session being passed.
        /// </summary>
        /// <param name="sessionToUpdate"></param>
        /// <returns>True if successfull.</returns>
        bool EditSession(Session sessionToUpdate);

        /// <summary>
        /// Adds a list of tags to a session by sessionId.
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="sessionId"></param>
        void AddSessionTags(List<Tag> tags, int sessionId);

        /// <summary>
        /// Deletes a session by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        bool DeleteSessionById(int sessionId);

        /// <summary>
        /// Adds a tag to a session, by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tagName"></param>
        /// <returns>The id of the created tag, or -1 if it already exists.</returns>
        int AddTagToSession(int sessionId, string tagName);

        /// <summary>
        /// Removes a tag by tagId from a session, by sessionId.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="tagId"></param>
        bool RemoveTagFromSession(int sessionId, int tagId);
    }
}